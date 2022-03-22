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
    /// Логика взаимодействия для SettingDescription.xaml
    /// </summary>
    public partial class SettingDescription : UserControl
    {
        public SettingDescription()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty HeaderProperty =
          DependencyProperty.Register(
             name: "Header",
             propertyType: typeof(string),
             ownerType: typeof(SettingDescription),
             typeMetadata: new FrameworkPropertyMetadata(
                 defaultValue: "Header value",
                 flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
             validateValueCallback: new ValidateValueCallback(IsValidString));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static bool IsValidString(object value)
        {
            string val = (string)value;
            return val != "";
        }

        public static readonly DependencyProperty DescriptionProperty =
          DependencyProperty.Register(
             name: "Description",
             propertyType: typeof(string),
             ownerType: typeof(SettingDescription),
             typeMetadata: new FrameworkPropertyMetadata(
                 defaultValue: "Description value",
                 flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
             validateValueCallback: new ValidateValueCallback(IsValidString));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }
    }
}
