﻿using System;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        Frame rootFrame;

        public RegistrationPage(Frame frame)
        {
            InitializeComponent();
            rootFrame = frame;
        }

        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage(this.rootFrame));
        }

        private void GoToLogin_MouseUp(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new LoginPage(this.rootFrame));
        }
    }
}