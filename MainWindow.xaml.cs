using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using path_watcher.Mocks;
using path_watcher.Interfaces;
using System.IO;
using System;
using path_watcher.Models;
using System.Linq;
using System.Collections.Generic;
using path_watcher.Pages;
using path_watcher.Static;
using Hardcodet.Wpf.TaskbarNotification;
using System.Reflection;

namespace path_watcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, Type> pages = new Dictionary<string, Type>();
        TaskbarIcon tbi;
        private ApplicationContext Context;
        private BaseRepository db;
       
        public MainWindow()
        {
            InitializeComponent();

            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            tbi = (TaskbarIcon)FindResource("NotifyIcon");
            var str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            tbi.Icon = new System.Drawing.Icon(@"C:\Users\SEEGa\source\repos\theDimZone\path-watcher\Icons\eye.ico");
            tbi.TrayMouseDoubleClick += Tbi_TrayMouseDoubleClick;
            tbi.TrayRightMouseDown += Tbi_TrayRightMouseDown;
            tbi.TrayToolTipOpen += Tbi_TrayToolTipOpen;
            tbi.TrayToolTipClose += Tbi_TrayToolTipClose;
            this.Activated += MainWindow_Activated;
            this.Deactivated += MainWindow_Deactivated;
            SuperVisor.MountWatchers(Config.GetStringArray("paths"));
            pages.Add("FilesPage", typeof(FilesPage));
            pages.Add("SettingsPage", typeof(SettingsPage));
            pages.Add("LogsPage", typeof(LogsPage));

            nv.SelectedItem = nv.MenuItems.OfType<ModernWpf.Controls.NavigationViewItem>().First();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Tbi_TrayToolTipClose(object sender, RoutedEventArgs e)
        {
           // tbi.TrayToolTip = null;

        }

        private void Tbi_TrayToolTipOpen(object sender, RoutedEventArgs e)
        {
           // tbi.TrayToolTip = new();
            var list = db.GetLogs();
            list.Sort((a, b) => b.DateEvent.CompareTo(a.DateEvent));
            List<Log> SortList;
            if (list.Count > 5) SortList = list.GetRange(0, 5); else SortList = list.GetRange(0, list.Count);
            var listView = tbi.TrayToolTip as ModernWpf.Controls.ListView;
            
            listView.ItemsSource = SortList;
           listView.Background = System.Windows.Media.Brushes.White;
            
        }

        private void Tbi_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {

                tbi.TrayToolTip.Visibility = Visibility.Visible;

            }
        }

        private void Tbi_TrayRightMouseDown(object sender, RoutedEventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                
                tbi.ContextMenu = new ContextMenu();
                
            }


            

        }

        private void Tbi_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                this.ShowInTaskbar = true;
                tbi.Visibility = Visibility.Hidden;
                this.Activate();

            }
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {

                this.ShowInTaskbar = false;
                tbi.Visibility = Visibility.Visible;
            }
        }

        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(pages["SettingsPage"]);
            }
            else
            {
                var selectedItem = (ModernWpf.Controls.NavigationViewItem)args.SelectedItem;
                string selectedItemTag = (string)selectedItem.Tag;
                Type pageType = pages[selectedItemTag];
                contentFrame.Navigate(pageType);
            }
        }
    }
}


