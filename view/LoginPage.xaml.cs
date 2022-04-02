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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        Frame rootFrame;

        public LoginPage(Frame frame)
        {
            InitializeComponent();
            this.rootFrame = frame;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage(this.rootFrame));
        }

        private void GoToRegistration_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(this.rootFrame));
        }
    }
}
