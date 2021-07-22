using path_watcher.Mocks;
using path_watcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using path_watcher.Static;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Win32;
using System.Reflection;
using System.Threading;

namespace path_watcher.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private ApplicationContext Context;
        private BaseRepository db;
        private ObservableCollection<string> paths;
        private ObservableCollection<string> filters; // TODO: add inputs for filters
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
        List<FileInfo> files;
        string Path;
        private void IsAutoRun_Click(object sender, RoutedEventArgs e)
        {
            if (IsAutoRun.IsChecked == true) SetAutoRunValue(true, Assembly.GetExecutingAssembly().Location);
            else SetAutoRunValue(false, Assembly.GetExecutingAssembly().Location);
        }
        public  void Test()
        {
            var list = db.getListFileModel(files, Path);
            db.Inserts(list);
           // files.ForEach(delegate (FileInfo fi) { db.AddToDbFile(fi, Path); });
            
        }

        private void AddWatcher_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Multiselect = false;

            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok) // TODO: check if already added
            {
                DirectoryInfo diTop = new(dialog.FileName);

                db.AddToDbDir(diTop); // TODO: don't create if exists
                files = diTop.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
                Path = diTop.FullName;
                Thread myThread = new Thread(new ThreadStart(Test));
                myThread.Priority = ThreadPriority.Highest;
                myThread.Start(); // запускаем поток
                //foreach (var fi in diTop.EnumerateFiles("*.*", SearchOption.AllDirectories))
                //{
                //    db.AddToDbFile(fi, diTop.FullName);
                //}
                SuperVisor.AddWatcher(diTop.FullName);
                paths.Add(diTop.FullName);
                Config.SetStringArray("paths", paths.ToArray());
            }
        }

        private void RemoveWatcher_Click(object sender, RoutedEventArgs e)
        {
            string selected = (sender as Button).DataContext as string;
            SuperVisor.DeleteWatcher(selected);
            paths.Remove(selected);
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
            Process.Start("explorer.exe", "ms-settings:notifications");
            // Process.Start("ms-settings:notifications");
        }
    }
}
