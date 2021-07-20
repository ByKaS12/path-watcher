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
using System.Windows.Shapes;

namespace path_watcher
{
    /// <summary>
    /// Логика взаимодействия для LogFileWindow.xaml
    /// </summary>
    public partial class LogFileWindow : Window
    {
        private ApplicationContext Context;
        private BaseRepository db;
        public Models.File file;
        public LogFileWindow()
        {
            InitializeComponent();
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            RefreshButton.Click += RefreshButton_Click;
            this.KeyDown += LogFileWindow_KeyDown;
            
        }

        private void LogFileWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F5)
                LogFileView.ItemsSource = db.GetLogs().FindAll(x => x.FileId == file.Id);

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

            LogFileView.ItemsSource = db.GetLogs().FindAll(x => x.FileId == file.Id);
        }
    }
}
