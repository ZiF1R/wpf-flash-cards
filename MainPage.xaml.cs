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
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        Frame rootFrame = mainWindow.MainFrame;
        private SqlConnection CurrentConnection;

        public MainPage(SqlConnection currentConnection)
        {
            this.CurrentConnection = currentConnection;
            InitializeComponent();
            SecondFrame.Content = new ProfilePage(CurrentConnection);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new ProfilePage(CurrentConnection);
        }

        private void Folders_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new FoldersPage(MainPageGrid);
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new SettingsPage();
        }
    }
}
