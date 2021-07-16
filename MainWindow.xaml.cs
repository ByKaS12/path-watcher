using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using path_watcher.Mocks;
using path_watcher.Interfaces;
using System.IO;
using System;
using path_watcher.Models;

namespace path_watcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationContext Context;
        private BaseRepository db;
        
        public MainWindow()
        {
            
            InitializeComponent();
           Context = new ApplicationContext();
          db = new BaseRepository(Context);
            Watchers = new SuperVisor();
        }
        


        private SuperVisor Watchers;
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Multiselect = false;
            CommonFileDialogResult result = dialog.ShowDialog();
            if(result == CommonFileDialogResult.Ok)
            {
                
                Watchers.AddWatcher(dialog.FileName);
                DirectoryInfo diTop = new(dialog.FileName);
                db.AddToDbDir(diTop);
                foreach (var fi in diTop.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    db.AddToDbFile(fi, diTop.FullName);
                }
                              
            }



        }


        }
    }


