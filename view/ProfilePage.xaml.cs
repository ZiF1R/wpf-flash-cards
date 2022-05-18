using course_project1.controls;
using course_project1.controls.ModalWindows;
using course_project1.storage;
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
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        Frame rootFrame = mainWindow.MainFrame;
        DataStorage Storage;
        string ConnectionString;
        Grid MainWindowGrid;

        public ProfilePage(Grid mainWindowGrid, string connectionString, DataStorage storage)
        {
            MainWindowGrid = mainWindowGrid;
            ConnectionString = connectionString;
            Storage = storage;
            InitializeComponent();

            NicknameInput.Value = Storage.user.Nickname;
            SurnameInput.Value = Storage.user.Surname;
            NameInput.Value = Storage.user.Name;
            EmailInput.Value = Storage.user.Email;
            PasswordInput.Value = DataEncriptor.Decrypt(Storage.user.Password);
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.rootFrame.Content = new LoginPage(MainWindowGrid, ConnectionString, Storage);
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NicknameInput.Value = NicknameInput.Value.Trim();
                SurnameInput.Value = SurnameInput.Value.Trim();
                NameInput.Value = NameInput.Value.Trim();

                Validator.ValidateInput(NicknameInput.Value, "NicknameFormatError", true);
                Validator.ValidateInput(SurnameInput.Value, "SurnameFormatError");
                Validator.ValidateInput(NameInput.Value, "NameFormatError");
                Validator.ValidatePassword(PasswordInput.Value);

                if (
                    NicknameInput.Value == Storage.user.Nickname &&
                    SurnameInput.Value == Storage.user.Surname &&
                    NameInput.Value == Storage.user.Name &&
                    PasswordInput.Value == DataEncriptor.Decrypt(Storage.user.Password)
                ) return;

                Storage.user.ChangeUserData(
                    NicknameInput.Value, SurnameInput.Value,
                    NameInput.Value, PasswordInput.Value, this.ConnectionString);
            }
            catch (FormatException ex)
            {
                CustomMessage.Show(ex.Message);
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
            }
        }
    }
}
