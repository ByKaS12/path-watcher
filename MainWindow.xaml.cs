using Hardcodet.Wpf.TaskbarNotification;
using path_watcher.Mocks;
using path_watcher.Models;
using path_watcher.Pages;
using path_watcher.Static;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace path_watcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<string, Type> pages = new();
        private readonly TaskbarIcon tbi;
        private ApplicationContext Context;
        private BaseRepository db;

        public MainWindow()
        {
            InitializeComponent();
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            tbi = (TaskbarIcon)FindResource("NotifyIcon");
            string str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            System.Windows.Resources.StreamResourceInfo s = Application.GetResourceStream(new Uri("Icons/eye.ico", UriKind.Relative));
            System.Drawing.Icon ico = new(s.Stream);
            tbi.Icon = ico;
            tbi.TrayMouseDoubleClick += Tbi_TrayMouseDoubleClick;
            tbi.TrayRightMouseDown += Tbi_TrayRightMouseDown;
            tbi.TrayToolTipOpen += Tbi_TrayToolTipOpen;
            tbi.TrayToolTipClose += Tbi_TrayToolTipClose;
            Activated += MainWindow_Activated;
            Deactivated += MainWindow_Deactivated;
            SuperVisor.MountWatchers(Config.GetStringArray("paths"));
            pages.Add("FilesPage", typeof(FilesPage));
            pages.Add("SettingsPage", typeof(SettingsPage));
            pages.Add("LogsPage", typeof(LogsPage));

            nv.SelectedItem = nv.MenuItems.OfType<ModernWpf.Controls.NavigationViewItem>().First();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {

            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
            tbi.Visibility = Visibility.Hidden;
        }
        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {

                ShowInTaskbar = false;
                tbi.Visibility = Visibility.Visible;
            }
        }

        private void Tbi_TrayToolTipClose(object sender, RoutedEventArgs e)
        {
            // tbi.TrayToolTip = null;

        }

        private void Tbi_TrayToolTipOpen(object sender, RoutedEventArgs e)
        {
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            List<Log> list = db.GetLogs();
            list.Sort((a, b) => b.DateEvent.CompareTo(a.DateEvent));
            List<Log> SortList = list.Count > 5 ? list.GetRange(0, 5) : list.GetRange(0, list.Count);
            ModernWpf.Controls.ListView listView = tbi.TrayToolTip as ModernWpf.Controls.ListView;

            listView.ItemsSource = SortList;
            listView.Background = System.Windows.Media.Brushes.White;

        }

        private void Tbi_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {

                tbi.TrayToolTip.Visibility = Visibility.Visible;

            }
        }

        private void Tbi_TrayRightMouseDown(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {

                tbi.ContextMenu = new ContextMenu();

            }




        }

        private void Tbi_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
                ShowInTaskbar = true;
                tbi.Visibility = Visibility.Hidden;
                _ = Activate();

            }
        }



        private void NavigationView_SelectionChanged(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                _ = contentFrame.Navigate(pages["SettingsPage"]);
            }
            else
            {
                ModernWpf.Controls.NavigationViewItem selectedItem = (ModernWpf.Controls.NavigationViewItem)args.SelectedItem;
                string selectedItemTag = (string)selectedItem.Tag;
                Type pageType = pages[selectedItemTag];
                _ = contentFrame.Navigate(pageType);
            }
        }
    }
}


