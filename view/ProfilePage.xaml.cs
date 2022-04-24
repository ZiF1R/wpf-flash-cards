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

        public ProfilePage(string connectionString, DataStorage storage)
        {
            ConnectionString = connectionString;
            Storage = storage;
            InitializeComponent();

            NicknameInput.Placeholder = Storage.user.Nickname;
            SurnameInput.Placeholder = Storage.user.Surname;
            NameInput.Placeholder = Storage.user.Name;
            EmailInput.Placeholder = Storage.user.Email;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            Storage.settings.SetAppTheme(Storage.settings.GetThemeName(1, ConnectionString));
            Storage.settings.SetAppLang(Storage.settings.GetLangName(1, ConnectionString));
            Storage = new DataStorage();
            mainWindow.AppLanguage.SelectedIndex = 0;

            this.rootFrame.Content = new LoginPage(ConnectionString, Storage);
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            NicknameInput.Placeholder = NicknameInput.Placeholder.Trim();
            SurnameInput.Placeholder = SurnameInput.Placeholder.Trim();
            NameInput.Placeholder = NameInput.Placeholder.Trim();

            if (
                NicknameInput.Placeholder == "" ||
                SurnameInput.Placeholder == "" ||
                NameInput.Placeholder == ""
            )
            {
                MessageBox.Show("Поля не могут быть пустыми!");
                return;
            }

            if (
                NicknameInput.Placeholder == Storage.user.Nickname &&
                SurnameInput.Placeholder == Storage.user.Surname &&
                NameInput.Placeholder == Storage.user.Name
            ) return;

            Storage.user.ChangeUserData(NicknameInput.Placeholder, SurnameInput.Placeholder, SurnameInput.Placeholder, this.ConnectionString);
        }
    }
}
