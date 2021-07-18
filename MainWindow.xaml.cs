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

namespace path_watcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, Type> pages = new Dictionary<string, Type>();

        public MainWindow()
        {
            InitializeComponent();

            pages.Add("FilesPage", typeof(FilesPage));
            pages.Add("SettingsPage", typeof(SettingsPage));
            pages.Add("LogsPage", typeof(LogsPage));

            nv.SelectedItem = nv.MenuItems.OfType<ModernWpf.Controls.NavigationViewItem>().First();
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


