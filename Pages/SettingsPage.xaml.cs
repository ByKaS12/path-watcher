using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using path_watcher.Mocks;
using path_watcher.Models;
using path_watcher.Static;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace path_watcher.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly ApplicationContext Context;
        private readonly BaseRepository db;
        private readonly ObservableCollection<string> paths;
        private readonly ObservableCollection<string> filters; // TODO: add inputs for filters
        private static bool SetAutoRunValue(bool autorun, string path)
        {
            const string name = "Path-Watcher";
            string ExePath = path;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                {
                    reg.SetValue(name, ExePath);
                }
                else
                {
                    reg.DeleteValue(name);
                }
                reg.Flush();
                reg.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public SettingsPage()
        {
            InitializeComponent();
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            IsAutoRun.Click += IsAutoRun_Click;
            paths = new ObservableCollection<string>(Config.GetStringArray("paths"));
            watchersList.ItemsSource = paths;
        }

        private List<FileInfo> files;
        private string Path;
        private void IsAutoRun_Click(object sender, RoutedEventArgs e)
        {
            _ = IsAutoRun.IsChecked == true
                ? SetAutoRunValue(true, Assembly.GetExecutingAssembly().Location)
                : SetAutoRunValue(false, Assembly.GetExecutingAssembly().Location);
        }
        public void Test()
        {

            // files.ForEach(delegate (FileInfo fi) { db.AddToDbFile(fi, Path); });

        }

        private void AddWatcher_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new()
            {
                IsFolderPicker = true,
                Multiselect = false
            };

            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok) // TODO: check if already added
            {
                DirectoryInfo diTop = new(dialog.FileName);

                db.AddToDbDir(diTop); // TODO: don't create if exists


                files = diTop.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();


                Path = diTop.FullName;

                List<Models.File> list = db.getListFileModel(files, Path);
                db.Inserts(list);

                SuperVisor.AddWatcher(diTop.FullName);
                paths.Add(diTop.FullName);
                Config.SetStringArray("paths", paths.ToArray());
            }
        }

        private void RemoveWatcher_Click(object sender, RoutedEventArgs e)
        {
            string selected = (sender as Button).DataContext as string;
            SuperVisor.DeleteWatcher(selected);
            _ = paths.Remove(selected);
            Config.SetStringArray("paths", paths.ToArray());
        }

        private void DeleteLogs_Click(object sender, RoutedEventArgs e)
        {
            db.DeleteLogs();
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            db.ExportToExcel();

        }

        private void IsAutoRun_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotificationButton_Click(object sender, RoutedEventArgs e)
        {
            // var shell = new Shell32.Shell();
            // shell.Explore("ms-settings:notifications");
            _ = Process.Start("explorer.exe", "ms-settings:notifications");
            // Process.Start("ms-settings:notifications");
        }
    }
}
