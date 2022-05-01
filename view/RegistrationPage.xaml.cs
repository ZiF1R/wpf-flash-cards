using course_project1.controls;
using course_project1.controls.ModalWindows;
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
        string ConnectionString;
        DataStorage Storage;
        Grid MainWindowGrid;

        public RegistrationPage(Grid mainWindowGrid, string connectionString, DataStorage storage)
        {
            MainWindowGrid = mainWindowGrid;
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
                NavigationService.Navigate(new LoginPage(MainWindowGrid, ConnectionString, Storage));
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
            }
        }

        private bool ValidateForm()
        {
            try
            {
                ValidateInput(NicknameInput, "NicknameFormatError", true);
                ValidateInput(SurnameInput, "SurnameFormatError");
                ValidateInput(NameInput, "NameFormatError");
                Validator.ValidateEmail(EmailInput);
                Validator.ValidatePassword(PasswordInput);

                if (PasswordInput.Value != ConfirmPasswordInput.Value)
                {
                    CustomMessage.Show((string)Application.Current.FindResource("ConfirmPasswordError"));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
                return false;
            }
        }

        private void ValidateInput(CustomTextBox textBox, string errorFormatMessageResourceName, bool specialFormat = false)
        {
            try
            {
                Validator.ValidateInput(textBox, specialFormat);
            }
            catch (FormatException ex)
            {
                throw new FormatException((string)Application.Current.FindResource(errorFormatMessageResourceName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GoToLogin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LoginPage(MainWindowGrid, ConnectionString, Storage));
        }
    }
}
