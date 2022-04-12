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
    /// Логика взаимодействия для AddCardButton.xaml
    /// </summary>
    public partial class AddCardButton : UserControl
    {
        public AddCardButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ButtonNameProperty =
            DependencyProperty.Register(
               name: "ButtonName",
               propertyType: typeof(string),
               ownerType: typeof(AddCardButton),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string ButtonName
        {
            get => (string)GetValue(ButtonNameProperty);
            set => SetValue(ButtonNameProperty, value);
        }

        public static readonly RoutedEvent AddCardEvent
            = EventManager.RegisterRoutedEvent("AddCard", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AddCardButton));

        public event RoutedEventHandler AddCard
        {
            add { AddHandler(AddCardEvent, value); }
            remove { RemoveHandler(AddCardEvent, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddCardEvent));
        }
    }
}
