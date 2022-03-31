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
    /// Логика взаимодействия для CategoryModalWindow.xaml
    /// </summary>
    public partial class CategoryModalWindow : UserControl
    {
        public CategoryModalWindow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CategoryValueProperty =
           DependencyProperty.Register(
              name: "CategoryValue",
              propertyType: typeof(string),
              ownerType: typeof(CustomTextBox),
              typeMetadata: new FrameworkPropertyMetadata(
                  defaultValue: "",
                  flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string CategoryValue
        {
            get => (string)GetValue(CategoryValueProperty);
            set => SetValue(CategoryValueProperty, value);
        }

        public static readonly RoutedEvent CloseCategoryModalEvent
            = EventManager.RegisterRoutedEvent("CloseCategoryModal", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CategoryModalWindow));

        public event RoutedEventHandler CloseCategoryModal
        {
            add { AddHandler(CloseCategoryModalEvent, value); }
            remove { RemoveHandler(CloseCategoryModalEvent, value); }
        }

        public static readonly RoutedEvent AddCategoryEvent
            = EventManager.RegisterRoutedEvent("AddCategory", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SimpleModalWindow));

        public event RoutedEventHandler AddCategory
        {
            add { AddHandler(AddCategoryEvent, value); }
            remove { RemoveHandler(AddCategoryEvent, value); }
        }

        private void Overlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseCategoryModalEvent));
        }

        private void Modal_Close(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseCategoryModalEvent));
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (FolderNameTextBox.Value != "")
            {
                this.CategoryValue = FolderNameTextBox.Value;
                RaiseEvent(new RoutedEventArgs(AddCategoryEvent));
                RaiseEvent(new RoutedEventArgs(CloseCategoryModalEvent));
            }
            else
            {
                MessageBox.Show("Please fill all fields!");
            }
        }
    }
}
