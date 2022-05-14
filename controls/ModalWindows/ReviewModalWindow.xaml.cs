using course_project1.storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для ReviewModalWindow.xaml
    /// </summary>
    public partial class ReviewModalWindow : UserControl
    {
        Grid MainPageGrid;
        DataStorage Storage;

        private Review Review;
        private int RootFolderId;
        private bool isSubmitted = false;
        private DispatcherTimer TimerToAnswer;
        private int CurrentTime = 0;
        string ConnectionString;

        public ReviewModalWindow(Grid mainPageGrid, DataStorage storage, string connectionString, int rootFolderId, Card[] cards)
        {
            Storage = storage;
            ConnectionString = connectionString;
            RootFolderId = rootFolderId;
            MainPageGrid = mainPageGrid;
            RootFolderId = rootFolderId;
            InitializeComponent();

            TimerToAnswer = new DispatcherTimer();
            TimerToAnswer.Interval = new TimeSpan(0, 0, 1);

            if (Storage.settings.ReviewTimeLimit != 0)
                TimerToAnswer.Tick += new EventHandler(Timer_tick);

            AnswerCompareResult.Visibility = Visibility.Hidden;

            Review = new Review(cards);
            Card[] deck = Review.Init(Storage.settings.ReviewCardsLimit);
            AllCardsNumber.Text = deck.Length.ToString();
            NextCard();
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            CurrentTime++;

            if (CurrentTime == Storage.settings.ReviewTimeLimit)
            {
                TimerToAnswer.Stop();
                SubmitAction();
            }
        }

        private void NextCard()
        {
            if (Review.CurrentCardIndex >= Review.Deck.Length - 1)
            {
                ShowReviewResults();
                return;
            }

            (int currentCardIndex, Card currentCard) = Review.NextCard();
            CurrentCardNumber.Text = (currentCardIndex + 1).ToString();
            isSubmitted = false;

            AnswerCompareResult.Visibility = Visibility.Hidden;
            CurrentCardAnswer.Visibility = Visibility.Hidden;
            ShowAnswer.Visibility = Visibility.Visible;
            ShowAnswer.SetResourceReference(Button.ContentProperty, "ShowAnswer");
            SubmitButton.SetResourceReference(Button.ContentProperty, "Submit");
            SubmitButton.Style = (Style)SubmitButton.FindResource("PrimaryButton");

            AnswerInput.Value = "";

            if (Storage.settings.isReviewSwitched)
            {
                CurrentCardTerm.Text = currentCard.Translation;
                CurrentCardAnswer.Text = currentCard.Term;
            }
            else
            {
                CurrentCardTerm.Text = currentCard.Term;
                CurrentCardAnswer.Text = currentCard.Translation;
            }
            CurrentTime = 0;
            TimerToAnswer.Start();
        }

        private void ShowReviewResults()
        {
            ReviewResultsForm.Visibility = Visibility.Visible;
            ReviewForm.Visibility = Visibility.Hidden;

            TotalCards.Text = Review.Deck.Length.ToString();
            RightAnswers.Text = Review.RightAnswers.ToString();
            WrongAnswers.Text = Review.WrongAnswers.ToString();
            TimePassed.Text = Review.GetFormattedTime();
        }

        private void ShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCardAnswer.Visibility == Visibility.Visible)
            {
                CurrentCardAnswer.Visibility = Visibility.Hidden;
                ShowAnswer.SetResourceReference(Button.ContentProperty, "ShowAnswer");
            }
            else
            {
                CurrentCardAnswer.Visibility = Visibility.Visible;
                ShowAnswer.SetResourceReference(Button.ContentProperty, "HideAnswer");
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            TimerToAnswer.Stop();
            if (isSubmitted)
                NextCard();
            else
                SubmitAction();
        }

        private void SubmitAction() 
        {
            AnswerInput.Value = AnswerInput.Value.Trim();
            AnswerCompareResult.Visibility = Visibility.Visible;
            CurrentCardAnswer.Visibility = Visibility.Visible;

            string answer = Storage.settings.isReviewSwitched ? Review.CurrentCard.Term : Review.CurrentCard.Translation;
            if (AnswerInput.Value == answer)
            {
                Review.RightAnswers++;
                AnswerCompareResult.SetResourceReference(Label.ContentProperty, "RightAnswer");
                AnswerCompareResult.Foreground = new SolidColorBrush(Colors.Green);
                SubmitButton.SetResourceReference(Button.ContentProperty, "Next");

                Review.CurrentCard.SendAnswer(ConnectionString, RootFolderId, true);
            }
            else
            {
                Review.WrongAnswers++;
                AnswerCompareResult.SetResourceReference(Label.ContentProperty, "WrongAnswer");
                AnswerCompareResult.Foreground = new SolidColorBrush(Colors.Tomato);
                SubmitButton.SetResourceReference(Button.ContentProperty, "Next");
                SubmitButton.Style = (Style)SubmitButton.FindResource("DangerButton");
                Review.CurrentCard.SendAnswer(ConnectionString, RootFolderId, false);
            }
            ShowAnswer.Visibility = Visibility.Hidden;

            isSubmitted = true;
        }

        public static readonly RoutedEvent CloseReviewEvent
            = EventManager.RegisterRoutedEvent("CloseReview", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ReviewModalWindow));

        public event RoutedEventHandler CloseReview
        {
            add { AddHandler(CloseReviewEvent, value); }
            remove { RemoveHandler(CloseReviewEvent, value); }
        }

        private void CloseReviewButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            TimerToAnswer.Stop();

            SimpleModalWindow modal = new SimpleModalWindow(true);
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(SimpleModalWindow.ModalContentProperty, "CloseReview");

            MainPageGrid.Children.Add(modal);
            modal.CloseModal += (object se, RoutedEventArgs evn) =>
            {
                MainPageGrid.Children.Remove(modal);
                TimerToAnswer.Start();
            };
            modal.NegativeButtonClick += (object se, RoutedEventArgs evn) =>
            {
                TimerToAnswer.Stop();
                MainPageGrid.Children.Remove(modal);
                RaiseEvent(new RoutedEventArgs(CloseReviewEvent));
            };
        }

        private void FinishReviewButton_Click(object sender, RoutedEventArgs e)
        {
            TimerToAnswer.Stop();
            RaiseEvent(new RoutedEventArgs(CloseReviewEvent));
        }
    }
}
