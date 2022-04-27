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

            NicknameInput.Placeholder = Storage.user.Nickname;
            SurnameInput.Placeholder = Storage.user.Surname;
            NameInput.Placeholder = Storage.user.Name;
            EmailInput.Placeholder = Storage.user.Email;
            PasswordInput.Placeholder = DataEncriptor.Decrypt(Storage.user.Password);
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Storage.settings.SetAppTheme(Storage.settings.GetThemeName(1, ConnectionString));
            Storage.settings.SetAppLang(Storage.settings.GetLangName(1, ConnectionString));
            Storage.Clear();
            mainWindow.AppLanguage.SelectedIndex = 0;

            this.rootFrame.Content = new LoginPage(MainWindowGrid, ConnectionString, Storage);
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Validator.ValidateInput(NicknameInput, true);
                Validator.ValidateInput(SurnameInput);
                Validator.ValidateInput(NameInput);
                Validator.ValidatePassword(PasswordInput);

                if (
                    NicknameInput.Placeholder == Storage.user.Nickname &&
                    SurnameInput.Placeholder == Storage.user.Surname &&
                    NameInput.Placeholder == Storage.user.Name &&
                    PasswordInput.Placeholder == DataEncriptor.Decrypt(Storage.user.Password)
                ) return;

                Storage.user.ChangeUserData(
                    NicknameInput.Placeholder, SurnameInput.Placeholder,
                    NameInput.Placeholder, PasswordInput.Placeholder, this.ConnectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
