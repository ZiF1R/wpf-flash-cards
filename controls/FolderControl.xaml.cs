using course_project1.controls.ModalWindows;
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
    /// Логика взаимодействия для FolderControl.xaml
    /// </summary>
    public partial class FolderControl : UserControl
    {
        Grid MainPageGrid;

        public FolderControl(
            Grid mainPageGrid,
            string folderName,
            string folderCategory = "none",
            int folderCardsCount = 0,
            int folderMemorizedCardsCount = 0
            )
        {
            MainPageGrid = mainPageGrid;
            FolderName = folderName;
            FolderCategory = folderCategory;
            FolderCardsCount = folderCardsCount;
            FolderMemorizedCardsCount = folderMemorizedCardsCount;
            InitializeComponent();
        }

        // Folder name
        public static readonly DependencyProperty FolderNameProperty =
            DependencyProperty.Register(
               name: "FolderName",
               propertyType: typeof(string),
               ownerType: typeof(FolderControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "Default name",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
               validateValueCallback: new ValidateValueCallback(IsValidString));

        public string FolderName
        {
            get => (string)GetValue(FolderNameProperty);
            set => SetValue(FolderNameProperty, value);
        }

        public static bool IsValidString(object value)
        {
            string val = (string)value;
            return val != "";
        }

        // folder category
        public static readonly DependencyProperty FolderCategoryProperty =
            DependencyProperty.Register(
               name: "FolderCategory",
               propertyType: typeof(string),
               ownerType: typeof(FolderControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "none",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
               validateValueCallback: new ValidateValueCallback(IsValidString));

        public string FolderCategory
        {
            get => (string)GetValue(FolderCategoryProperty);
            set => SetValue(FolderCategoryProperty, value);
        }

        // folder cards count
        public static readonly DependencyProperty FolderCardsCountProperty =
            DependencyProperty.Register(
               name: "FolderCardsCount",
               propertyType: typeof(int),
               ownerType: typeof(FolderControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: 0,
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
               validateValueCallback: new ValidateValueCallback(IsValidCardsCount));

        public int FolderCardsCount
        {
            get => (int)GetValue(FolderCardsCountProperty);
            set => SetValue(FolderCardsCountProperty, value);
        }

        // folder memorized cards count
        public static readonly DependencyProperty FolderMemorizedCardsCountProperty =
            DependencyProperty.Register(
               name: "FolderMemorizedCardsCount",
               propertyType: typeof(int),
               ownerType: typeof(FolderControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: 0,
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
               validateValueCallback: new ValidateValueCallback(IsValidCardsCount));

        public int FolderMemorizedCardsCount
        {
            get => (int)GetValue(FolderMemorizedCardsCountProperty);
            set => SetValue(FolderMemorizedCardsCountProperty, value);
        }

        public static bool IsValidCardsCount(object value)
        {
            int val = (int)value;
            return val >= 0;
        }

        public static readonly RoutedEvent RemoveFolderEvent
            = EventManager.RegisterRoutedEvent("RemoveFolder", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FolderControl));

        public event RoutedEventHandler RemoveFolder
        {
            add { AddHandler(RemoveFolderEvent, value); }
            remove { RemoveHandler(RemoveFolderEvent, value); }
        }

        private void RemoveFolderButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            myPopup.IsOpen = false;

            SimpleModalWindow modal = new SimpleModalWindow();
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(SimpleModalWindow.ModalContentProperty, "RemoveFolder");

            MainPageGrid.Children.Add(modal);
            modal.CloseModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
            modal.NegativeButtonClick += (object s, RoutedEventArgs ev) => RaiseEvent(new RoutedEventArgs(RemoveFolderEvent));
        }

        private void EditFolderButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            myPopup.IsOpen = false;

            FolderModalWindow modal = new FolderModalWindow(MainPageGrid, this.FolderNameField.Text, this.FolderCategoryField.Text);
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(FolderModalWindow.ModalHeaderProperty, "EditFolder");
            modal.SetResourceReference(FolderModalWindow.ActionButtonContentProperty, "Edit");

            MainPageGrid.Children.Add(modal);
            modal.FolderAction += (object s, RoutedEventArgs ev) =>
            {
                this.FolderNameField.Text = modal.FolderName;
                this.FolderCategoryField.Text = modal.FolderCategory;
            };
            modal.CloseFolderModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
        }
    }
}
