using course_project1.storage;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        string ConnectionString;
        DataStorage Storage;

        public RegistrationPage(string connectionString, DataStorage storage)
        {
            this.ConnectionString = connectionString;
            this.Storage = storage;
            InitializeComponent();
        }

        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = ValidateForm();
            if (!isValid) return;

            try
            {
                Storage.user = new User(
                    NicknameInput.Value,
                    SurnameInput.Value,
                    NameInput.Value,
                    EmailInput.Value,
                    PasswordInput.Value,
                    ConnectionString
                );
                Storage.settings.CreateUserSettings(ConnectionString, Storage.user.Uid);
                NavigationService.Navigate(new LoginPage(ConnectionString, Storage));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidateForm()
        {
            NicknameInput.Value = NicknameInput.Value.Trim();
            SurnameInput.Value = SurnameInput.Value.Trim();
            NameInput.Value = NameInput.Value.Trim();
            EmailInput.Value = EmailInput.Value.Trim();
            PasswordInput.Value = PasswordInput.Value.Trim();
            ConfirmPasswordInput.Value = ConfirmPasswordInput.Value.Trim();

            if (
                NicknameInput.Value.Length == 0 ||
                SurnameInput.Value.Length == 0 ||
                NameInput.Value.Length == 0 ||
                EmailInput.Value.Length == 0 ||
                PasswordInput.Value.Length == 0
            )
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return false;
            }

            if (!Regex.IsMatch(EmailInput.Value, @"([\w\d-_]+)\@([\w\d]+)\.(\w){2,}"))
            {
                MessageBox.Show("Неправильный формат email-адреса. " +
                    "Email-адрес может содержать только цифры, буквы, а также знак подчеркивания и тире!");
                return false;
            }

            if (!Regex.IsMatch(PasswordInput.Value, @"([\w\d-_]){6,}"))
            {
                MessageBox.Show("Длина пороля должена быть не менее 6 символов. " +
                    "Пороль может содержать только цифры, буквы, а также знак подчеркивания и тире!");
                return false;
            }

            if (PasswordInput.Value != ConfirmPasswordInput.Value)
            {
                MessageBox.Show("Подтвердите пороль!");
                return false;
            }

            return true;
        }

        private void GoToLogin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LoginPage(ConnectionString, Storage));
        }
    }
}
