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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace course_project1.controls.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для MultipleAnswersReviewModalWindow.xaml
    /// </summary>
    public partial class MultipleAnswersReviewModalWindow : UserControl
    {
        Grid MainPageGrid;
        DataStorage Storage;

        private Review Review;
        private int RootFolderId;
        private bool isSubmitted = false;
        string ConnectionString;

        public (int totalCards, int rightAnswers, int wrongAnswers, string timePassed)? reviewResults = null;

        public MultipleAnswersReviewModalWindow(Grid mainPageGrid, DataStorage storage, string connectionString, int rootFolderId, Card[] cards)
        {
            this.MainPageGrid = mainPageGrid;
            this.Storage = storage;
            this.ConnectionString = connectionString;
            this.RootFolderId = rootFolderId;

            InitializeComponent();

            Review = new Review(cards);
            Card[] deck = Review.Init(Storage.settings.ReviewCardsLimit);
            AllCardsNumber.Text = deck.Length.ToString();
            NextCard();
        }

        private void NextCard()
        {
            if (Review.CurrentCardIndex >= Review.Deck.Length - 1)
            {
                int totalCards = Review.Deck.Length;
                int rightAnswers = Review.RightAnswers;
                int wrongAnswers = Review.WrongAnswers;
                string timePassed = Review.GetFormattedTime();

                this.reviewResults = (totalCards, rightAnswers, wrongAnswers, timePassed);
                RaiseEvent(new RoutedEventArgs(CloseReviewEvent));

                return;
            }

            (int currentCardIndex, Card currentCard) = Review.NextCard();
            CurrentCardNumber.Text = (currentCardIndex + 1).ToString();
            isSubmitted = false;

            if (Storage.settings.isReviewSwitched)
            {
                CurrentCardTerm.Text = currentCard.Translation;
            }
            else
            {
                CurrentCardTerm.Text = currentCard.Term;
            }

            AnswerCompareResult.Visibility = Visibility.Hidden;
            AnswersListBox.Items.Clear();

            string[] answers = Review.GenerateCardAnswers(4, Storage.settings.isReviewSwitched);
            foreach (string answer in answers)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = answer;
                AnswersListBox.Items.Add(item);
            }

            AnswersListBox.SelectedIndex = 0;
            AnswersListBox.IsEnabled = true;
        }

        public static readonly RoutedEvent CloseReviewEvent
            = EventManager.RegisterRoutedEvent("CloseReview", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MultipleAnswersReviewModalWindow));

        public event RoutedEventHandler CloseReview
        {
            add { AddHandler(CloseReviewEvent, value); }
            remove { RemoveHandler(CloseReviewEvent, value); }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSubmitted)
                NextCard();
            else
                SubmitAction();
        }

        private void SubmitAction()
        {
            AnswersListBox.IsManipulationEnabled = false;
            AnswerCompareResult.Visibility = Visibility.Visible;

            string answer = Storage.settings.isReviewSwitched ? Review.CurrentCard.Term : Review.CurrentCard.Translation;
            string selectedItem = (string)((ListBoxItem)AnswersListBox.SelectedItem).Content;
            if (selectedItem == answer)
            {
                Review.RightAnswers++;
                AnswerCompareResult.SetResourceReference(Label.ContentProperty, "RightAnswer");
                AnswerCompareResult.Foreground = new SolidColorBrush(Colors.Green);
                SubmitButton.SetResourceReference(Button.ContentProperty, "Next");
                ((ListBoxItem)AnswersListBox.SelectedItem).SetResourceReference(BackgroundProperty, "RightAnswerBg");

                ((ListBoxItem)AnswersListBox.SelectedItem).IsSelected = false;
                Review.CurrentCard.SendAnswer(ConnectionString, RootFolderId, true);
            }
            else
            {
                Review.WrongAnswers++;
                AnswerCompareResult.SetResourceReference(Label.ContentProperty, "WrongAnswer");
                AnswerCompareResult.Foreground = new SolidColorBrush(Colors.Tomato);
                SubmitButton.SetResourceReference(Button.ContentProperty, "Next");
                SubmitButton.Style = (Style)SubmitButton.FindResource("DangerButton");
                ((ListBoxItem)AnswersListBox.SelectedItem).SetResourceReference(BackgroundProperty, "WrongAnswerBg");

                ((ListBoxItem)AnswersListBox.SelectedItem).IsSelected = false;
                for (int i = 0; i < AnswersListBox.Items.Count; i++)
                {
                    string item = (string)((ListBoxItem)AnswersListBox.Items[i]).Content;
                    if (answer == item)
                    {
                        ((ListBoxItem)AnswersListBox.Items[i]).SetResourceReference(BackgroundProperty, "RightAnswerBg");
                        break;
                    }
                }

                Review.CurrentCard.SendAnswer(ConnectionString, RootFolderId, false);
            }

            isSubmitted = true;
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
