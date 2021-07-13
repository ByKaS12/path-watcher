using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using path_watcher.Mocks;
using path_watcher.Interfaces;
using System.IO;

namespace path_watcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBaseRepository<Models.Directory> Directories { get; set; }
        private IBaseRepository<Models.File> Files { get; set; }
        private IBaseRepository<Models.Log> Logs { get; set; }

        public MainWindow()
        {
            InitializeComponent();

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
                
                
                

            }


        }
    }
    }

