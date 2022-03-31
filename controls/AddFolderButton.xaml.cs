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
    /// Логика взаимодействия для AddFolderButton.xaml
    /// </summary>
    public partial class AddFolderButton : UserControl
    {
        public AddFolderButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddFolderEvent));
        }

        public static readonly RoutedEvent AddFolderEvent
            = EventManager.RegisterRoutedEvent("AddFolderEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AddFolderButton));

        public event RoutedEventHandler AddFolder
        {
            add { AddHandler(AddFolderEvent, value); }
            remove { RemoveHandler(AddFolderEvent, value); }
        }
    }
}
