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

namespace course_project1.controls
{
    /// <summary>
    /// Логика взаимодействия для CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl
    {
        public CustomTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
               name: "Placeholder",
               propertyType: typeof(string),
               ownerType: typeof(CustomTextBox),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "Input",
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
               ownerType: typeof(CustomTextBox),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.Value = Input.Text;
        }
    }
}
