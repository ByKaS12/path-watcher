using path_watcher.Mocks;
using path_watcher.Models;
using System.Windows;
using System.Windows.Input;

namespace path_watcher
{
    /// <summary>
    /// Логика взаимодействия для LogFileWindow.xaml
    /// </summary>
    public partial class LogFileWindow : Window
    {
        private readonly ApplicationContext Context;
        private readonly BaseRepository db;
        public Models.File file;
        public LogFileWindow()
        {
            InitializeComponent();
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            RefreshButton.Click += RefreshButton_Click;
            KeyDown += LogFileWindow_KeyDown;

        }

        private void LogFileWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                LogFileView.ItemsSource = db.GetLogs().FindAll(x => x.FileId == file.Id);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

            LogFileView.ItemsSource = db.GetLogs().FindAll(x => x.FileId == file.Id);
        }
    }
}
