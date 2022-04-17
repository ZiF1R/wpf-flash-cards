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
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        Frame rootFrame = mainWindow.MainFrame;
        SqlConnection CurrentConnection = mainWindow.CurrentConnection;
        DataStorage Storage = mainWindow.Storage;

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            EmailInput.Value = EmailInput.Value.Trim();
            PasswordInput.Value = PasswordInput.Value.Trim();

            if (EmailInput.Value == "" || PasswordInput.Value == "")
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (!Regex.IsMatch(EmailInput.Value, @"([\w\d-_]+)\@([\w\d]+)\.(\w){2,}"))
            {
                MessageBox.Show("Email-адрес имеет не правильный формат!");
                return;
            }

            bool isLoginSuccess = Storage.user.LoadUser(EmailInput.Value, PasswordInput.Value, this.CurrentConnection);
            if (!isLoginSuccess)
            {
                MessageBox.Show("Неправильный email-адрес или пороль!");
                return;
            }
            else
            {
                Storage.LoadCategories(CurrentConnection);
                Storage.settings.LoadSettings(CurrentConnection);
                Storage.LoadFolders(CurrentConnection);
                mainWindow.AppLanguage.SelectedIndex = mainWindow.Storage.settings.currentLangId - 1;

                NavigationService.Navigate(new MainPage());
            }
        }

        private void GoToRegistration_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }
    }
}
