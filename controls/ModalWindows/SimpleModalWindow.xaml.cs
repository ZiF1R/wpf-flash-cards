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

namespace course_project1.controls.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для SimpleModalWindow.xaml
    /// </summary>
    public partial class SimpleModalWindow : UserControl
    {
        public SimpleModalWindow(bool isCancelWindow = false)
        {
            InitializeComponent();

            if (isCancelWindow)
            {
                NegativeButton.SetResourceReference(Button.ContentProperty, "Close");
            }
            else
            {
                NegativeButton.SetResourceReference(Button.ContentProperty, "Remove");
            }
        }

        private void Overlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseModalEvent));
        }

        public static readonly RoutedEvent CloseModalEvent
            = EventManager.RegisterRoutedEvent("CloseModal", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SimpleModalWindow));

        public event RoutedEventHandler CloseModal
        {
            add { AddHandler(CloseModalEvent, value); }
            remove { RemoveHandler(CloseModalEvent, value); }
        }

        public static readonly RoutedEvent NegativeButtonClickEvent
            = EventManager.RegisterRoutedEvent("NegativeButtonClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SimpleModalWindow));

        public event RoutedEventHandler NegativeButtonClick
        {
            add { AddHandler(NegativeButtonClickEvent, value); }
            remove { RemoveHandler(NegativeButtonClickEvent, value); }
        }

        public static readonly DependencyProperty ModalContentProperty =
            DependencyProperty.Register(
               name: "ModalContent",
               propertyType: typeof(string),
               ownerType: typeof(SimpleModalWindow),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "Modal content",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string ModalContent
        {
            get => (string)GetValue(ModalContentProperty);
            set => SetValue(ModalContentProperty, value);
        }

        private void Modal_Close(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseModalEvent));
        }

        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NegativeButtonClickEvent));
            RaiseEvent(new RoutedEventArgs(CloseModalEvent));
        }
    }
}
