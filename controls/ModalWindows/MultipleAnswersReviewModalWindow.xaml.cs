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
        private DispatcherTimer TimerToAnswer;
        private int CurrentTime = 0;

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

            TimerToAnswer = new DispatcherTimer();
            TimerToAnswer.Interval = new TimeSpan(0, 0, 1);

            if (Storage.settings.ReviewTimeLimit != 0)
                TimerToAnswer.Tick += new EventHandler(Timer_tick);

            NextCard();
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            CurrentTime++;

            if (CurrentTime >= Storage.settings.ReviewTimeLimit)
            {
                TimerToAnswer.Stop();
                SubmitAction();
            }
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

            CurrentCardExamples.Text = currentCard.Examples;

            AnswerCompareResult.Visibility = Visibility.Hidden;
            SubmitButton.Style = (Style)SubmitButton.FindResource("PrimaryButton");
            SubmitButton.SetResourceReference(Button.ContentProperty, "Submit");

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

            CurrentTime = 0;
            TimerToAnswer.Start();

            if (currentCard.Examples == "")
                ShowExamplesButton.Visibility = Visibility.Hidden;
            else
                ShowExamplesButton.Visibility = Visibility.Visible;

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri("pack://application:,,,/icons/review/eye.png", UriKind.RelativeOrAbsolute);
            image.EndInit();

            CurrentCardExamples.Visibility = Visibility.Collapsed;
            ShowExamplesButton.Source = image;
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
            TimerToAnswer.Stop();
            if (isSubmitted)
                NextCard();
            else
                SubmitAction();
        }

        private void SubmitAction()
        {
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
            ((ListBoxItem)AnswersListBox.SelectedItem).IsSelected = false;
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

        private void ShowExamplesButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            image.CacheOption = BitmapCacheOption.OnLoad;
            if (CurrentCardExamples.Visibility == Visibility.Collapsed)
            {
                image.UriSource = new Uri("pack://application:,,,/icons/review/invisible.png", UriKind.RelativeOrAbsolute);
                CurrentCardExamples.Visibility = Visibility.Visible;
            }
            else
            {
                image.UriSource = new Uri("pack://application:,,,/icons/review/eye.png", UriKind.RelativeOrAbsolute);
                CurrentCardExamples.Visibility = Visibility.Collapsed;
            }

            image.EndInit();
            ShowExamplesButton.Source = image;
        }
    }
}
