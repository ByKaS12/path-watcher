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

        public SettingsPage()
        {
            InitializeComponent();
            Context = new ApplicationContext();
            db = new BaseRepository(Context);

            paths = new ObservableCollection<string>(Config.GetStringArray("paths"));
            watchersList.ItemsSource = paths;
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

                foreach (var fi in diTop.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    db.AddToDbFile(fi, diTop.FullName);
                }

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
    }
}
