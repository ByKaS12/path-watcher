using path_watcher.Mocks;
using path_watcher.Models;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace path_watcher.Pages
{
    /// <summary>
    /// Логика взаимодействия для FilesPage.xaml
    /// </summary>
    /// 
    public partial class FilesPage : Page
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);
        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;
        public static bool ShowFileProperties(string Filename)
        {
            SHELLEXECUTEINFO info = new();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = Filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            return ShellExecuteEx(ref info);
        }
        private readonly ApplicationContext Context;
        private readonly BaseRepository db;

        public FilesPage()
        {

            InitializeComponent();
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            MenuItem OpenFile = new();
            OpenFile.Click += OpenFile_Click;
            OpenFile.Header = "Открыть файл";
            MenuItem OpenDir = new();
            OpenDir.Click += OpenDir_Click;
            MenuItem OpenLogFile = new();
            OpenLogFile.Click += OpenLogFile_Click;
            OpenLogFile.Header = "Открыть изменения файла";
            MenuItem OpenPropFile = new();
            OpenPropFile.Click += OpenPropFile_Click;
            OpenPropFile.Header = "Открыть свойства файла";
            KeyDown += FilesPage_KeyDown;
            OpenDir.Header = "Открыть расположение файла";
            filesView.ItemsSource = db.GetFiles();
            filesView.ContextMenu.Items.Insert(0, OpenLogFile);
            filesView.ContextMenu.Items.Insert(0, OpenDir);
            filesView.ContextMenu.Items.Insert(0, OpenPropFile);
            filesView.ContextMenu.Items.Insert(0, OpenFile);






        }

        private void FilesPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                filesView.ItemsSource = db.GetFiles();
            }
        }

        private void OpenPropFile_Click(object sender, RoutedEventArgs e)
        {
            if (filesView.SelectedItem != null)
            {
                File item = filesView.SelectedItem as Models.File;
                _ = ShowFileProperties(item.FullPath);


            }
        }

        private void OpenLogFile_Click(object sender, RoutedEventArgs e)
        {
            if (filesView.SelectedItem != null)
            {
                File item = filesView.SelectedItem as Models.File;
                LogFileWindow logFileWindow = new()
                {
                    file = item
                };
                logFileWindow.LogFileView.ItemsSource = db.GetLogs().FindAll(x => x.FileId == item.Id);
                /*TODO Get mainwindow and add owner to logfileWindow*/

                logFileWindow.Show();


            }
        }

        private void OpenDir_Click(object sender, RoutedEventArgs e)
        {
            if (filesView.SelectedItem != null)
            {
                File item = filesView.SelectedItem as Models.File;
                _ = System.Diagnostics.Process.Start("explorer", item.Directory.FullPath);


            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {

            if (filesView.SelectedItem != null)
            {
                File item = filesView.SelectedItem as Models.File;
                _ = System.Diagnostics.Process.Start("explorer", item.FullPath);


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            filesView.ItemsSource = db.GetFilesByFilename(textBoxFilename.Text);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

            filesView.ItemsSource = db.GetFilesByFilename(textBoxFilename.Text);
        }
    }
}
