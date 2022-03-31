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
    /// Логика взаимодействия для SwitchBox.xaml
    /// </summary>
    public partial class SwitchBox : UserControl
    {
        public SwitchBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SwitchedProperty =
        DependencyProperty.Register(
           name: "Switched",
           propertyType: typeof(bool),
           ownerType: typeof(SwitchBox),
           typeMetadata: new FrameworkPropertyMetadata(
               defaultValue: false,
               flags: FrameworkPropertyMetadataOptions.AffectsMeasure
           ));

        public bool Switched
        {
            get => (bool)GetValue(SwitchedProperty);
            set => SetValue(SwitchedProperty, value);
        }

        private void SwitchControl_Click(object sender, RoutedEventArgs e)
        {
            this.SetValue(SwitchedProperty, SwitchControl.IsChecked);
            RaiseEvent(new RoutedEventArgs(SwitchChangedEvent));
        }

        public static readonly RoutedEvent SwitchChangedEvent
            = EventManager.RegisterRoutedEvent("SwitchChangedEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SwitchBox));

        public event RoutedEventHandler SwitchChanged
        {
            add { AddHandler(SwitchChangedEvent, value); }
            remove { RemoveHandler(SwitchChangedEvent, value); }
        }
    }
}
