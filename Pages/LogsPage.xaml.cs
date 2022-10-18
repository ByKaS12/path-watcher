using path_watcher.Mocks;
using path_watcher.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace path_watcher.Pages
{
    /// <summary>
    /// Логика взаимодействия для LogsPage.xaml
    /// </summary>
    public partial class LogsPage : Page
    {
        private ApplicationContext Context;
        private BaseRepository db;

        public LogsPage()
        {
            InitializeComponent();
            this.KeyDown += LogsPage_KeyDown;
            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            logsView.ItemsSource = db.GetLogs();
        }

        private void LogsPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                logsView.ItemsSource = db.GetLogs();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

            logsView.ItemsSource = db.GetLogs();
        }
    }
}
