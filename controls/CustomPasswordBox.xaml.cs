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

namespace course_project1.controls
{
    /// <summary>
    /// Логика взаимодействия для CustomPasswordBox.xaml
    /// </summary>
    public partial class CustomPasswordBox : UserControl
    {
        public CustomPasswordBox()
        {
            InitializeComponent();
        }

        private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PasswordInput.Password.Length == 0)
            {
                PasswordInputLabel.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordInputLabel.Visibility = Visibility.Hidden;
            }
            this.Value = PasswordInput.Password;
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
               name: "Placeholder",
               propertyType: typeof(string),
               ownerType: typeof(CustomPasswordBox),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "Enter password",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
               validateValueCallback: new ValidateValueCallback(IsValidReading));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static bool IsValidReading(object value)
        {
            string val = (string)value;
            return val != "";
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
               name: "Value",
               propertyType: typeof(string),
               ownerType: typeof(CustomPasswordBox),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}
