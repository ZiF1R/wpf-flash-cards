using course_project1.controls.ModalWindows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        string ConnectionString;
        DataStorage Storage;
        Grid MainWindowGrid;

        public LoginPage(Grid mainWindowGrid, string connectionString, DataStorage storage)
        {
            MainWindowGrid = mainWindowGrid;
            ConnectionString = connectionString;
            Storage = storage;
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Validator.ValidateEmail(EmailInput.Value);
                Validator.ValidatePassword(PasswordInput.Value);

                bool isLoginSuccess = Storage.user.LoadUser(EmailInput.Value, PasswordInput.Value, this.ConnectionString);
                if (!isLoginSuccess)
                {
                    CustomMessage.Show((string)Application.Current.FindResource("LoginError"));
                    return;
                }
                else
                {
                    Storage.LoadCategories(ConnectionString);
                    Storage.settings.LoadSettings(ConnectionString, Storage.user.Uid);
                    Storage.LoadFolders(ConnectionString);
                    mainWindow.AppLanguage.SelectedIndex = Storage.settings.currentLangId - 1;

                    NavigationService.Navigate(new MainPage(MainWindowGrid, ConnectionString, Storage));
                }
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
                return;
            }
        }

        private void GoToRegistration_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(MainWindowGrid, ConnectionString, Storage));
        }
    }
}
