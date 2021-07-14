using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using path_watcher.Mocks;
using path_watcher.Interfaces;
using System.IO;
using System;

namespace path_watcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EnumFiles EnumFiles { get; set; }
        private Models.ApplicationContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new Models.ApplicationContext();
            BaseRepository<Models.Directory> baseRepository = new BaseRepository<Models.Directory>(db);
            BaseRepository<Models.File> baseFile = new BaseRepository<Models.File>(db);
            BaseRepository<Models.Log> baseLog = new BaseRepository<Models.Log>(db);
            EnumFiles = new EnumFiles(baseRepository,baseFile,baseLog);
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
                EnumFiles.AddToDbDir(diTop);
                try
                { 

                    foreach (var di in diTop.EnumerateDirectories("*"))
                    {
                        try
                        {
                            foreach (var fi in di.EnumerateFiles("*", SearchOption.AllDirectories))
                            {
                                try
                                {


                                }
                                catch (UnauthorizedAccessException unAuthFile)
                                {
                                    Console.WriteLine($"unAuthFile: {unAuthFile.Message}");
                                }
                            }
                        }
                        catch (UnauthorizedAccessException unAuthSubDir)
                        {
                            Console.WriteLine($"unAuthSubDir: {unAuthSubDir.Message}");
                        }
                    }
                }
                catch (DirectoryNotFoundException dirNotFound)
                {
                    Console.WriteLine($"{dirNotFound.Message}");
                }
                catch (UnauthorizedAccessException unAuthDir)
                {
                    Console.WriteLine($"unAuthDir: {unAuthDir.Message}");
                }
                catch (PathTooLongException longPath)
                {
                    Console.WriteLine($"{longPath.Message}");
                }
            }



        }


        }
    }


