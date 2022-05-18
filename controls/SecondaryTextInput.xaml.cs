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
    /// Логика взаимодействия для SecondaryTextInput.xaml
    /// </summary>
    public partial class SecondaryTextInput : UserControl
    {
        public SecondaryTextInput()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
               name: "Value",
               propertyType: typeof(string),
               ownerType: typeof(SecondaryTextInput));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
               name: "IsReadOnly",
               propertyType: typeof(bool),
               ownerType: typeof(SecondaryTextInput));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(
               name: "MaxLength",
               propertyType: typeof(uint),
               ownerType: typeof(SecondaryTextInput),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: (uint)50,
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public uint MaxLength
        {
            get => (uint)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }
    }
}
