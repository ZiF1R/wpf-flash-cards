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
    /// Логика взаимодействия для ReviewResults.xaml
    /// </summary>
    public partial class ReviewResults : UserControl
    {
        public ReviewResults((int totalCards, int rightAnswers, int wrongAnswers, string timePassed)? reviewResults)
        {
            InitializeComponent();

            TotalCards.Text = reviewResults?.totalCards.ToString();
            RightAnswers.Text = reviewResults?.rightAnswers.ToString();
            WrongAnswers.Text = reviewResults?.wrongAnswers.ToString();
            TimePassed.Text = reviewResults?.timePassed;
        }

        public static readonly RoutedEvent CloseResultsEvent
            = EventManager.RegisterRoutedEvent("CloseResults", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ReviewResults));

        public event RoutedEventHandler CloseResults
        {
            add { AddHandler(CloseResultsEvent, value); }
            remove { RemoveHandler(CloseResultsEvent, value); }
        }

        private void FinishReviewButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseResultsEvent));
        }
    }
}
