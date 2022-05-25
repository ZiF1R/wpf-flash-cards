using course_project1.storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        DataStorage Storage;
        string ConnectionString;
        public string FolderName = "";
        public string FolderCategory = "";
        Grid MainPageGrid;
        bool isModalOpened = false;

        public FolderModalWindow(Grid mainPageGrid, string connectionString, DataStorage storage, string folderName, string folderCategory)
        {
            ConnectionString = connectionString;
            Storage = storage;
            this.FolderName = folderName;
            this.FolderCategory = folderCategory;
            this.MainPageGrid = mainPageGrid;
            InitializeComponent();

            FolderNameTextBox.Value = this.FolderName;

            foreach (Category category in Storage.categories)
                AddCategoryItem(category);
        }

        private void AddCategoryItem(Category category)
        {
            try
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = category.Name;

                if (FolderCategory != "" && category.Name == FolderCategory)
                    item.IsSelected = true;
                else if (FolderCategory == "" && category.Name == "none")
                    item.IsSelected = true;

                FolderCategorySelect.Items.Add(item);
                item.MouseRightButtonUp += (sender, e) =>
                {
                    if (isModalOpened) return;
                    if (item.Content.ToString() == "none") return;

                    SimpleModalWindow modal = new SimpleModalWindow();
                    modal.SetValue(Grid.RowSpanProperty, 2);
                    modal.SetValue(Grid.ColumnSpanProperty, 3);
                    modal.SetResourceReference(SimpleModalWindow.ModalContentProperty, "RemoveCategory");

                    FolderCategorySelect.IsDropDownOpen = false;
                    MainPageGrid.Children.Add(modal);
                    isModalOpened = true;

                    modal.CloseModal += (object s, RoutedEventArgs ev) =>
                    {
                        MainPageGrid.Children.Remove(modal);
                        isModalOpened = false;
                    };
                    modal.NegativeButtonClick += (object s, RoutedEventArgs ev) =>
                    {
                        if (!Storage.IsUnusedCategory(category.Name))
                        {
                            CustomMessage.Show((string)Application.Current.FindResource("UsedCategoryError"));
                            return;
                        }

                        category.RemoveCategory(ConnectionString, item.Content.ToString(), Storage.user.Uid);
                        Storage.categories = Storage.categories.Where(c => c.Name != category.Name).ToArray();
                        FolderCategorySelect.SelectedIndex = 0;
                        FolderCategory = ((ComboBoxItem)FolderCategorySelect.Items.GetItemAt(0)).Content.ToString();
                        FolderCategorySelect.Items.Remove(item);
                    };
                };
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
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
                   defaultValue: "Action",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string ActionButtonContent
        {
            get => (string)GetValue(ActionButtonContentProperty);
            set => SetValue(ActionButtonContentProperty, value);
        }

        public static readonly RoutedEvent CloseFolderModalEvent
            = EventManager.RegisterRoutedEvent("CloseFolderModal", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FolderModalWindow));

        public event RoutedEventHandler CloseFolderModal
        {
            add { AddHandler(CloseFolderModalEvent, value); }
            remove { RemoveHandler(CloseFolderModalEvent, value); }
        }

        public static readonly RoutedEvent FolderActionEvent
            = EventManager.RegisterRoutedEvent("FolderAction", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FolderModalWindow));

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
            try
            {
                FolderNameTextBox.Value = FolderNameTextBox.Value.Trim();
                Validator.ValidateInput(FolderNameTextBox.Value, "FolderNameFormatError", true);

                if (FolderName != FolderNameTextBox.Value)
                {
                    bool isUnique = Folder.IsUniqueFolderName(ConnectionString, FolderNameTextBox.Value, Storage.user.Uid);
                    if (!isUnique)
                    {
                        FolderNameUsed.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        FolderNameUsed.Visibility = Visibility.Hidden;
                    }
                }

                this.FolderName = FolderNameTextBox.Value;
                this.FolderCategory = FolderCategorySelect.Text;

                RaiseEvent(new RoutedEventArgs(FolderActionEvent));
                RaiseEvent(new RoutedEventArgs(CloseFolderModalEvent));
            }
            catch (FormatException ex)
            {
                CustomMessage.Show(ex.Message);
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
            }
        }

        private void Modal_Close(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseFolderModalEvent));
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (isModalOpened) return;

            CategoryModalWindow modal = new CategoryModalWindow(ConnectionString, Storage);
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(ModalHeaderProperty, "CreateCategory");
            modal.SetResourceReference(ActionButtonContentProperty, "Create");

            MainPageGrid.Children.Add(modal);
            isModalOpened = true;

            modal.AddCategory += (object s, RoutedEventArgs ev) =>
            {
                Category category = new Category(modal.CategoryValue);
                category.AddCategory(ConnectionString, category.Name, Storage.user.Uid);
                AddCategoryItem(category);
                Storage.categories = Storage.categories.Append(category).ToArray();
            };
            modal.CloseCategoryModal += (object s, RoutedEventArgs ev) =>
            {
                MainPageGrid.Children.Remove(modal);
                isModalOpened = false;
            };
        }
    }
}
