using Microsoft.WindowsAPICodePack.Dialogs;
using path_watcher.Mocks;
using path_watcher.Models;
using path_watcher.Static;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для FilesPage.xaml
    /// </summary>
    public partial class FilesPage : Page
    {
        private ApplicationContext Context;
        private BaseRepository db;

        public FilesPage()
        {
            InitializeComponent();

            Context = new ApplicationContext();
            db = new BaseRepository(Context);

            filesView.ItemsSource = db.GetFiles();
        }

    }
}
