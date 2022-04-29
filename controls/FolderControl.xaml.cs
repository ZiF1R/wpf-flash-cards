using course_project1.controls.ModalWindows;
using course_project1.storage;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
        Folder folder;
        DataStorage Storage;
        string ConnectionString;

        public enum Action
        {
            None,
            Export,
            Import
        };
        Action action;

        public FolderControl(
            Grid mainPageGrid, Folder folder, DataStorage storage,
            string connectionString, bool isForExportImport = false, Action action = Action.None)
        {
            this.action = action;
            Storage = storage;
            ConnectionString = connectionString;
            MainPageGrid = mainPageGrid;
            FolderName = folder.Name;
            FolderCategory = folder.Category;
            FolderCardsCount = folder.Cards.Length;
            FolderMemorizedCardsCount = folder.MemorizedCardsCount();
            FolderCreatedDate = folder.Created;
            this.folder = folder;
            InitializeComponent();

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.CacheOption = BitmapCacheOption.OnLoad;

            if (folder.Cards.Length > 0)
                image.UriSource = new Uri("pack://application:,,,/icons/Play.png");
            else
                image.UriSource = new Uri("pack://application:,,,/icons/Play_disable.png");

            image.EndInit();
            ReviewButton.Source = image;

            if (this.action != Action.None)
                BlinkRectangle.Visibility = Visibility.Visible;
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

        // folder memorized cards count
        public static readonly DependencyProperty FolderCreatedDateProperty =
            DependencyProperty.Register(
               name: "FolderCreatedDate",
               propertyType: typeof(DateTime),
               ownerType: typeof(FolderControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: DateTime.Now,
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public DateTime FolderCreatedDate
        {
            get => (DateTime)GetValue(FolderCreatedDateProperty);
            set => SetValue(FolderCreatedDateProperty, value);
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

        public static readonly RoutedEvent EditFolderEvent
            = EventManager.RegisterRoutedEvent("EditFolder", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FolderControl));

        public event RoutedEventHandler EditFolder
        {
            add { AddHandler(EditFolderEvent, value); }
            remove { RemoveHandler(EditFolderEvent, value); }
        }

        private void EditFolderButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            myPopup.IsOpen = false;
            RaiseEvent(new RoutedEventArgs(EditFolderEvent));
        }

        public static readonly RoutedEvent GoToCardsEvent
            = EventManager.RegisterRoutedEvent("GoToCards", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FolderControl));

        public event RoutedEventHandler GoToCards
        {
            add { AddHandler(GoToCardsEvent, value); }
            remove { RemoveHandler(GoToCardsEvent, value); }
        }

        private void FolderNameField_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(GoToCardsEvent));
        }

        private void ReviewButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (folder.Cards.Length == 0) return;

            ReviewModalWindow modal = new ReviewModalWindow(MainPageGrid, Storage, ConnectionString, folder.FolderId, folder.Cards);
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            MainPageGrid.Children.Add(modal);

            modal.CloseReview += (object s, RoutedEventArgs ev) =>
            {
                FolderMemorizedCardsCount = folder.MemorizedCardsCount();
                MainPageGrid.Children.Remove(modal);
            };
        }

        private void BlinkRectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (action == Action.Import)
                ImportCards();
            else if (action == Action.Export)
                ExportCards();
        }

        public static readonly RoutedEvent ReturnToSettingsEvent
            = EventManager.RegisterRoutedEvent("ReturnToSettings", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FolderControl));

        public event RoutedEventHandler ReturnToSettings
        {
            add { AddHandler(ReturnToSettingsEvent, value); }
            remove { RemoveHandler(ReturnToSettingsEvent, value); }
        }

        private void ExportCards()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON format|*.json";
            dialog.FileName = folder.Name;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (FileStream fs = File.Create(dialog.FileName))
                    {
                        string extension = System.IO.Path.GetExtension(dialog.FileName).ToLower();
                        if (extension != ".json") throw new Exception((string)Application.Current.FindResource("InvalidFileExtension"));

                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Card[]));
                        serializer.WriteObject(fs, folder.Cards);
                    }
                }
                catch (Exception ex)
                {
                    CustomMessage.Show(ex.Message);
                }
            }

            RaiseEvent(new RoutedEventArgs(ReturnToSettingsEvent));
        }

        private void ImportCards()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "JSON format|*.json";

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (FileStream fs = File.OpenRead(dialog.FileName))
                    {
                        string extension = System.IO.Path.GetExtension(dialog.FileName).ToLower();
                        if (extension != ".json") throw new Exception((string)Application.Current.FindResource("InvalidFileExtension"));

                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Card[]));
                        Card[] cards = (Card[])serializer.ReadObject(fs);

                        if (cards != null && cards?.Length > 0)
                            MergeCards(cards);
                    }
                }
                catch (Exception ex)
                {
                    CustomMessage.Show(ex.Message);
                }
            }

            RaiseEvent(new RoutedEventArgs(ReturnToSettingsEvent));
        }

        private void MergeCards(Card[] cards)
        {
            foreach (Card card in cards)
            {
                // check for unique card termin
                if (folder.Cards.Where(c => c.Term == card.Term).ToArray().Length == 0)
                {
                    folder.Cards = folder.Cards.Append(card).ToArray();
                    card.InsertCard(ConnectionString, folder.FolderId);
                }
            }
        }
    }
}
