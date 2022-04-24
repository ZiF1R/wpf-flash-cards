using course_project1.controls;
using course_project1.controls.ModalWindows;
using course_project1.storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace course_project1.view
{
    /// <summary>
    /// Логика взаимодействия для CardsView.xaml
    /// </summary>
    public partial class CardsView : Page
    {
        static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        DataStorage Storage;
        string ConnectionString;
        Frame SecondFrame;
        Folder RootFolder;
        Grid MainPageGrid;

        enum CardFilter
        {
            All,
            Memorized,
            NotMemorized
        }

        CardFilter activeFilter = CardFilter.All;

        public CardsView(Grid mainPageGrid, Frame secondFrame, Folder folder, string connectionString, DataStorage storage)
        {
            ConnectionString = connectionString;
            Storage = storage;
            SecondFrame = secondFrame;
            RootFolder = folder;
            MainPageGrid = mainPageGrid;
            InitializeComponent();
            FolderName.Text = RootFolder.Name;

            foreach (Card card in RootFolder.Cards)
                CardsWrap.Children.Insert(1, this.CreateCardElement(card));
        }

        private void AddCardButton_AddCard(object sender, RoutedEventArgs e)
        {
            CardModalWindow modal = new CardModalWindow(MainPageGrid, ConnectionString, Storage, RootFolder.FolderId, "", "", "");
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(CardModalWindow.ModalHeaderProperty, "CreateCard");
            modal.SetResourceReference(CardModalWindow.ActionButtonContentProperty, "Create");

            MainPageGrid.Children.Add(modal);
            modal.CardAction += (object s, RoutedEventArgs ev) =>
            {
                string Term = modal.Term;
                string Translation = modal.Translation;
                string Examples = modal.Examples;

                try
                {
                    Card card = new Card(ConnectionString, RootFolder.FolderId, Term, Translation, Examples);
                    RootFolder.Cards = RootFolder.Cards.Append(card).ToArray();
                    if (activeFilter != CardFilter.Memorized)
                        CardsWrap.Children.Insert(1, this.CreateCardElement(card));
                }
                catch { }
            };
            modal.CloseCardModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
        }

        private CardControl CreateCardElement(Card card)
        {
            CardControl cardControl = null;
            try
            {
                cardControl = new CardControl(MainPageGrid, card);

                cardControl.Margin = new Thickness(0, 0, 40, 40);
                cardControl.EditCard += (object s, RoutedEventArgs ev) =>
                {
                    CardModalWindow modal = new CardModalWindow(
                        MainPageGrid, ConnectionString, Storage, RootFolder.FolderId, card.Term, card.Translation, card.Examples);
                    modal.SetValue(Grid.RowSpanProperty, 2);
                    modal.SetValue(Grid.ColumnSpanProperty, 3);
                    modal.SetResourceReference(CardModalWindow.ModalHeaderProperty, "EditCard");
                    modal.SetResourceReference(CardModalWindow.ActionButtonContentProperty, "Edit");
                    MainPageGrid.Children.Add(modal);

                    modal.CardAction += (object se, RoutedEventArgs evn) =>
                    {
                        card.ChangeCardData(ConnectionString, RootFolder.FolderId, modal.Term, modal.Translation, modal.Examples);

                        cardControl.Term = modal.Term;
                        cardControl.Translation = modal.Translation;
                        cardControl.Examples = modal.Examples;
                    };
                    modal.CloseCardModal += (object se, RoutedEventArgs evn) => MainPageGrid.Children.Remove(modal);
                };
                cardControl.RemoveCard += (object s, RoutedEventArgs ev) =>
                {
                    if (RootFolder.RemoveCard(ConnectionString, card))
                        CardsWrap.Children.Remove(cardControl);
                };
            }
            catch
            {
                MessageBox.Show("Card control creation error!");
            }
            return cardControl;
        }

        private void ReturnToFolders_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SecondFrame.Content = new FoldersPage(MainPageGrid, SecondFrame, ConnectionString, Storage);
        }

        private void ShowAllCards_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = CardFilter.All;
            CardsWrap.Children.RemoveRange(1, RootFolder.Cards.Length);
            foreach (Card card in RootFolder.Cards)
                CardsWrap.Children.Insert(1, this.CreateCardElement(card));
        }

        private void ShowMemorizedCards_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = CardFilter.Memorized;
            CardsWrap.Children.RemoveRange(1, RootFolder.Cards.Length);
            Card[] memorizedCards = RootFolder.Cards.Where(card => card.IsMemorized).ToArray();
            foreach (Card card in memorizedCards)
                CardsWrap.Children.Insert(1, this.CreateCardElement(card));
        }

        private void ShowNotMemorizedCards_Click(object sender, RoutedEventArgs e)
        {
            activeFilter = CardFilter.NotMemorized;
            CardsWrap.Children.RemoveRange(1, RootFolder.Cards.Length);
            Card[] notMemorizedCards = RootFolder.Cards.Where(card => !card.IsMemorized).ToArray();
            foreach (Card card in notMemorizedCards)
                CardsWrap.Children.Insert(1, this.CreateCardElement(card));
        }
    }
}
