using course_project1.controls.ModalWindows;
using course_project1.storage;
using course_project1.view;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace course_project1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ConnectionString;
        private DataStorage Storage;
 
        public MainWindow()
        {
            InitializeComponent();
            var sri = Application.GetResourceStream(new Uri("pack://application:,,,/icons/cursor.cur", UriKind.RelativeOrAbsolute));
            this.Cursor = new Cursor(sri.Stream);

            try
            {
                this.DataBaseConection();
                Storage = new DataStorage();
                LoadLangs();
                MainFrame.Content = new LoginPage(MainWindowGrid, ConnectionString, Storage);
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
                this.Close();
            }
        }

        private void LoadLangs()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT LANG FROM LANGS ORDER BY LANG_ID";

                SqlDataReader commandReader = command.ExecuteReader();
                if (!commandReader.HasRows)
                {
                    commandReader.Close();
                    MessageBox.Show("Cannot load langs!");
                    return;
                };

                int langNum = 1;
                while (commandReader.Read())
                {
                    string lang = commandReader.GetString(0);
                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = lang;
                    if (langNum == Storage.settings.currentLangId)
                        item.IsSelected = true;
                    AppLanguage.Items.Add(item);
                    langNum++;
                }
                commandReader.Close();
                connection.Close();
            }
        }

        private void DataBaseConection()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.DataSource = @"DESKTOP-LT1S3LJ\ZIF1R";
            connectionStringBuilder.InitialCatalog = "FlashCards";
            connectionStringBuilder.UserID = @"DESKTOP-LT1S3LJ\HP";
            connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.Password = "";

            this.ConnectionString = connectionStringBuilder.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                }
                catch
                {
                    connection.Close();

                    /// try to create db with code
                    throw new Exception("Database connection error!");
                }
            }
        }

        private void AppLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Storage.user.Email != "")
            {
                Storage.settings.ChangeAppLang(AppLanguage.SelectedIndex + 1, ConnectionString, Storage.user.Uid);
                return;
            }
            else
            {
                string lang = Regex.Replace(AppLanguage.SelectedItem.ToString(), @".*: ", "").ToLower();
                Uri newSource = new Uri($"pack://application:,,,/lang/{lang}.xaml");
                ResourceDictionary newResource = new ResourceDictionary();

                try
                {
                    newResource.Source = newSource;
                    Application.Current.Resources.MergedDictionaries.Remove(Storage.settings.currentLang);
                    Application.Current.Resources.MergedDictionaries.Add(newResource);
                    Storage.settings.currentLang.Source = newSource;
                }
                catch { }
            }
        }
    }
}
