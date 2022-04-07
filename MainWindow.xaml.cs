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
        private ResourceDictionary currentLang = new ResourceDictionary();
        public SqlConnection CurrentConnection;
        public DataStorage Storage;

        public MainWindow()
        {
            InitializeComponent();
            this.currentLang.Source = new Uri("pack://application:,,,/lang/en.xaml");
            var sri = Application.GetResourceStream(new Uri("pack://application:,,,/icons/cursor.cur", UriKind.RelativeOrAbsolute));
            var customCursor = new Cursor(sri.Stream);
            this.Cursor = customCursor;

            this.DataBaseConection();
            Storage = new DataStorage();
            MainFrame.Content = new LoginPage();
        }

        private void DataBaseConection()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();

            connectionStringBuilder.DataSource = @"DESKTOP-LT1S3LJ\ZIF1R";
            connectionStringBuilder.InitialCatalog = "FlashCards";
            connectionStringBuilder.UserID = @"DESKTOP-LT1S3LJ\HP";
            connectionStringBuilder.IntegratedSecurity = true;
            connectionStringBuilder.Password = "";

            this.CurrentConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
            try
            {
                this.CurrentConnection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                this.CurrentConnection.Close();
            }
        }

        private void AppLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string lang = Regex.Replace(AppLanguage.SelectedItem.ToString(), @".*: ", "").ToLower();
            Uri newSource = new Uri($"pack://application:,,,/lang/{lang}.xaml");
            ResourceDictionary newResource = new ResourceDictionary();

            try
            {
                newResource.Source = newSource;
                Application.Current.Resources.MergedDictionaries.Remove(currentLang);
                Application.Current.Resources.MergedDictionaries.Add(newResource);
                currentLang.Source = newSource;
            }
            catch { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CurrentConnection.Close();
        }
    }
}
