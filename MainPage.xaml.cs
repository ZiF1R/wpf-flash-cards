using course_project1.view;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace course_project1
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        Frame rootFrame = mainWindow.MainFrame;
        DataStorage Storage;
        string ConnectionString;

        public MainPage(string connectionString, DataStorage storage)
        {
            this.ConnectionString = connectionString;
            this.Storage = storage;
            InitializeComponent();
            SecondFrame.Content = new ProfilePage(ConnectionString, Storage);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new ProfilePage(ConnectionString, Storage);
        }

        private void Folders_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new FoldersPage(MainPageGrid, SecondFrame, ConnectionString, Storage);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new SettingsPage(ConnectionString, Storage);
        }

        private void ContactWithUs_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("mailto:do-alex03@mail.ru");
        }
    }
}
