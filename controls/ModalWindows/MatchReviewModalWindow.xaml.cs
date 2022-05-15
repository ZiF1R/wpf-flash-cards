using course_project1.storage;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace course_project1.controls.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для MatchReviewModalWindow.xaml
    /// </summary>
    public partial class MatchReviewModalWindow : UserControl
    {
        Grid MainPageGrid;
        DataStorage Storage;

        private Review Review;
        private int RootFolderId;
        string ConnectionString;

        public (int totalCards, int rightAnswers, int wrongAnswers, string timePassed)? reviewResults = null;

        public MatchReviewModalWindow(Grid mainPageGrid, DataStorage storage, string connectionString, int rootFolderId, Card[] cards)
        {
            this.MainPageGrid = mainPageGrid;
            this.Storage = storage;
            this.ConnectionString = connectionString;
            this.RootFolderId = rootFolderId;

            InitializeComponent();

            Review = new Review(cards);
            Card[] deck = Review.Init(Storage.settings.ReviewCardsLimit);

            foreach (Card card in deck)
            {
                ListBoxItem word = new ListBoxItem();
                word.Content = card.Term;
                WordsListBox.Items.Add(word);
            }

            deck = Review.ShuffleDeck(deck);
            foreach (Card card in deck)
            {
                ListBoxItem answer = new ListBoxItem();
                answer.Content = card.Translation;
                AnswersListBox.Items.Add(answer);
            }
        }

        public static readonly RoutedEvent CloseReviewEvent
            = EventManager.RegisterRoutedEvent("CloseReview", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MatchReviewModalWindow));

        public event RoutedEventHandler CloseReview
        {
            add { AddHandler(CloseReviewEvent, value); }
            remove { RemoveHandler(CloseReviewEvent, value); }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnswersListBox.SelectedItem != null && WordsListBox.SelectedItem != null)
            {
                ListBoxItem word = (ListBoxItem)WordsListBox.SelectedItem;
                ListBoxItem answer = (ListBoxItem)AnswersListBox.SelectedItem;
                SubmitButton.SetResourceReference(BackgroundProperty, "TextColorOpacity");
                SubmitButton.IsEnabled = false;

                Card selectedCard = null;
                foreach (Card card in Review.Deck)
                {
                    if (card.Term == (string)word.Content)
                        selectedCard = card;
                }

                if (selectedCard == null) return;

                int duration = 1000;

                DispatcherTimer myDispatcherTimer = new DispatcherTimer();
                myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, duration);
                myDispatcherTimer.Tick += new EventHandler(Tick);
                myDispatcherTimer.Start();

                void Tick(object o, EventArgs s)
                {
                    myDispatcherTimer.Stop();
                    myDispatcherTimer.Tick -= Tick;

                    if ((string)answer.Content == selectedCard.Translation)
                    {
                        WordsListBox.Items.Remove(word);
                        AnswersListBox.Items.Remove(answer);

                        if (WordsListBox.Items.Count == 0 && AnswersListBox.Items.Count == 0)
                        {
                            FinishReview();
                        }
                    }
                    else
                    {
                        word.Background = Brushes.Transparent;
                        answer.Background = Brushes.Transparent;
                    }

                    SubmitButton.IsEnabled = true;
                    SubmitButton.SetResourceReference(BackgroundProperty, "Primary");
                }

                if ((string)answer.Content == selectedCard.Translation)
                {
                    Review.RightAnswers++;
                    ((ListBoxItem)AnswersListBox.SelectedItem).SetResourceReference(BackgroundProperty, "RightAnswerBg");
                    ((ListBoxItem)WordsListBox.SelectedItem).SetResourceReference(BackgroundProperty, "RightAnswerBg");

                    DoubleAnimation ani = new DoubleAnimation(1, 0, new Duration(new TimeSpan(0, 0, 0, 0, duration)));
                    ((ListBoxItem)WordsListBox.SelectedItem).BeginAnimation(OpacityProperty, ani);
                    ((ListBoxItem)AnswersListBox.SelectedItem).BeginAnimation(OpacityProperty, ani);

                    selectedCard.SendAnswer(ConnectionString, RootFolderId, true);
                }
                else
                {
                    Review.WrongAnswers++;
                    ((ListBoxItem)AnswersListBox.SelectedItem).SetResourceReference(BackgroundProperty, "WrongAnswerBg");
                    ((ListBoxItem)WordsListBox.SelectedItem).SetResourceReference(BackgroundProperty, "WrongAnswerBg");

                    selectedCard.SendAnswer(ConnectionString, RootFolderId, false);
                }
                ((ListBoxItem)AnswersListBox.SelectedItem).IsSelected = false;
                ((ListBoxItem)WordsListBox.SelectedItem).IsSelected = false;
            }
        }

        private void FinishReview()
        {
            int totalCards = Review.Deck.Length;
            int rightAnswers = Review.RightAnswers;
            int wrongAnswers = Review.WrongAnswers;
            string timePassed = Review.GetFormattedTime();

            this.reviewResults = (totalCards, rightAnswers, wrongAnswers, timePassed);
            RaiseEvent(new RoutedEventArgs(CloseReviewEvent));
        }

        private void CloseReviewButton_MouseUp(object sender, MouseButtonEventArgs e)
        {

            SimpleModalWindow modal = new SimpleModalWindow(true);
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(SimpleModalWindow.ModalContentProperty, "CloseReview");

            MainPageGrid.Children.Add(modal);
            modal.CloseModal += (object se, RoutedEventArgs evn) => MainPageGrid.Children.Remove(modal);
            modal.NegativeButtonClick += (object se, RoutedEventArgs evn) =>
            {
                MainPageGrid.Children.Remove(modal);
                RaiseEvent(new RoutedEventArgs(CloseReviewEvent));
            };
        }
    }
}
