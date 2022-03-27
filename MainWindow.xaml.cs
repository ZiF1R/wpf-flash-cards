using course_project1.view;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
            this.currentLang.Source = new Uri("pack://application:,,,/lang/en.xaml");
            var sri = Application.GetResourceStream(new Uri("pack://application:,,,/icons/cursor.cur", UriKind.RelativeOrAbsolute));
            var customCursor = new Cursor(sri.Stream);
            this.Cursor = customCursor;

            MainFrame.Content = new LoginPage(MainFrame);
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
    }
}
