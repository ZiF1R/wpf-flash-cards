using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace course_project1.view
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        Frame rootFrame = mainWindow.MainFrame;
        SqlConnection CurrentConnection = mainWindow.CurrentConnection;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand loginCommand = CurrentConnection.CreateCommand();
            loginCommand.CommandText =
                $"SELECT * " +
                $"FROM USERS " +
                $"WHERE USERS.EMAIL = '{EmailInput.Value.Trim()}' AND USERS.PASS = '{PasswordInput.Value.Trim()}'";
            SqlDataReader loginCommandReader = loginCommand.ExecuteReader();

            bool loginSuccess = loginCommandReader.Read();
            while (loginCommandReader.Read())
            {
                int uid = loginCommandReader.GetInt32(0);
                string nickname = loginCommandReader.GetString(1);
                string name = loginCommandReader.GetString(2);
                string username = loginCommandReader.GetString(3);
                string email = loginCommandReader.GetString(4);
                string password = loginCommandReader.GetString(5);
            }
            loginCommandReader.Close();

            if (loginSuccess)
            {
                NavigationService.Navigate(new MainPage());
            }
            else
            {
                MessageBox.Show("Login or password wrong!", "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /// *
            NavigationService.Navigate(new MainPage());
        }

        private void GoToRegistration_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
