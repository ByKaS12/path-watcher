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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            Context = new ApplicationContext();
            db = new BaseRepository(Context);
            logsView.ItemsSource = db.GetLogs();
        }
    }
}
