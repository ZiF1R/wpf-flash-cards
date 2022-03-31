using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для FolderModalWindow.xaml
    /// </summary>
    public partial class FolderModalWindow : UserControl
    {
        public string FolderName = "";
        public string FolderCategory = "";
        Grid MainPageGrid;

        public FolderModalWindow(Grid mainPageGrid, string folderName, string folderCategory)
        {
            this.FolderName = folderName;
            this.FolderCategory = folderCategory;
            this.MainPageGrid = mainPageGrid;
            InitializeComponent();

            FolderNameTextBox.Value = this.FolderName;

            ComboBoxItem defaultItem = new ComboBoxItem();
            defaultItem.Content = "none";
            FolderCategorySelect.Items.Add(defaultItem);

            if (folderCategory != "")
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = this.FolderCategory;
                item.IsSelected = true;
                FolderCategorySelect.Items.Add(item);
            }
            else
            {
                defaultItem.IsSelected = true;
            }
        }

        public static readonly DependencyProperty ModalHeaderProperty =
            DependencyProperty.Register(
               name: "ModalHeader",
               propertyType: typeof(string),
               ownerType: typeof(FolderModalWindow),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "ModalWindow Header",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string ModalHeader
        {
            get => (string)GetValue(ModalHeaderProperty);
            set => SetValue(ModalHeaderProperty, value);
        }

        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register(
               name: "ActionButtonContent",
               propertyType: typeof(string),
               ownerType: typeof(FolderModalWindow),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "ModalWindow Header",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string ActionButtonContent
        {
            get => (string)GetValue(ActionButtonContentProperty);
            set => SetValue(ActionButtonContentProperty, value);
        }

        public static readonly RoutedEvent CloseFolderModalEvent
            = EventManager.RegisterRoutedEvent("CloseFolderModal", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SimpleModalWindow));

        public event RoutedEventHandler CloseFolderModal
        {
            add { AddHandler(CloseFolderModalEvent, value); }
            remove { RemoveHandler(CloseFolderModalEvent, value); }
        }

        public static readonly RoutedEvent FolderActionEvent
            = EventManager.RegisterRoutedEvent("FolderAction", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SimpleModalWindow));

        public event RoutedEventHandler FolderAction
        {
            add { AddHandler(FolderActionEvent, value); }
            remove { RemoveHandler(FolderActionEvent, value); }
        }

        private void Overlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseFolderModalEvent));
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (FolderNameTextBox.Value.ToString() != "")
            {
                this.FolderName = FolderNameTextBox.Value.ToString();
                this.FolderCategory = FolderCategorySelect.Text.ToString();
                RaiseEvent(new RoutedEventArgs(FolderActionEvent));
                RaiseEvent(new RoutedEventArgs(CloseFolderModalEvent));
            }
            else
            {
                MessageBox.Show("Please fill all fields!");
            }
        }

        private void Modal_Close(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseFolderModalEvent));
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryModalWindow modal = new CategoryModalWindow();
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(FolderModalWindow.ModalHeaderProperty, "CreateCategory");
            modal.SetResourceReference(FolderModalWindow.ActionButtonContentProperty, "Create");

            MainPageGrid.Children.Add(modal);
            modal.AddCategory += (object s, RoutedEventArgs ev) => MessageBox.Show(modal.CategoryValue);
            modal.CloseCategoryModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
        }
    }
}
