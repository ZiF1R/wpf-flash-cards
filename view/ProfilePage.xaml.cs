using System;
using System.Collections.Generic;
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
        Frame rootFrame;

        public ProfilePage(Frame frame)
        {
            InitializeComponent();
            rootFrame = frame;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            this.rootFrame.Content = new LoginPage(this.rootFrame);
        }
    }
}
