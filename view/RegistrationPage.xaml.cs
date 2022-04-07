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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        Frame rootFrame = mainWindow.MainFrame;
        SqlConnection CurrentConnection = mainWindow.CurrentConnection;

        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand registrationCommand = CurrentConnection.CreateCommand();
            registrationCommand.CommandText =
                $"SELECT * " +
                $"FROM USERS " +
                $"WHERE USERS.EMAIL = '{EmailInput.Value.Trim()}'";
            SqlDataReader registrationCommandReader = registrationCommand.ExecuteReader();

            bool isUniqueEmail = !registrationCommandReader.Read();
            registrationCommandReader.Close();

            if (isUniqueEmail)
            {
                /// * additional validation for each input

                NicknameInput.Value = NicknameInput.Value.Trim();
                SurnameInput.Value = SurnameInput.Value.Trim();
                NameInput.Value = NameInput.Value.Trim();
                EmailInput.Value = EmailInput.Value.Trim();
                PasswordInput.Value = PasswordInput.Value.Trim();
                ConfirmPasswordInput.Value = ConfirmPasswordInput.Value.Trim();

                if (PasswordInput.Value == ConfirmPasswordInput.Value)
                {
                    registrationCommand.CommandText =
                        "INSERT INTO USERS VALUES" +
                        $"('{NicknameInput.Value}', '{SurnameInput.Value}', '{NameInput.Value}', '{EmailInput.Value}', '{PasswordInput.Value}')";
                    try
                    {
                        registrationCommandReader = registrationCommand.ExecuteReader();
                        NavigationService.Navigate(new LoginPage());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Password value and confirm password value must be equal!");
                }
            }
            else
            {
                MessageBox.Show("User with this email already exist!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToLogin_MouseUp(object sender, MouseButtonEventArgs e)
        {

            NavigationService.Navigate(new LoginPage());
        }
    }
}
