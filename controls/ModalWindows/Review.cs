using course_project1.storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace course_project1.controls.ModalWindows
{
    public class Review
    {
        private Card[] deck;
        private Card currentCard;
        private int currentCardIndex;
        public int RightAnswers;
        public int WrongAnswers;

        public Card[] Deck { get => deck; } 
        public Card CurrentCard { get => currentCard; }
        public int CurrentCardIndex { get => currentCardIndex; }

        private DispatcherTimer timer;
        private int seconds;

        public int Seconds
        {
            get { return seconds; }
        }

        public Review(Card[] cards)
        {
            deck = cards;
            currentCardIndex = -1;
            RightAnswers = 0;
            WrongAnswers = 0;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(Timer_tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void Timer_tick(object sender, EventArgs e)
        {
            seconds++;
        }

        public Card[] Init(int ReviewCardsLimit)
        {
            (Card[] newCards, Card[] oldCards) = this.SplitCards();

            double CalcPercentageAnswers(int i)
            {
                int wrongAnswers = oldCards[i].WrongAnswers,
                  rightAnswers = oldCards[i].RightAnswers,
                  totalAnswers = rightAnswers + wrongAnswers;

                return Math.Floor((double)(wrongAnswers / totalAnswers) * 100);
            }

            for (int i = 0; i < oldCards.Length - 1; i++)
                for (int j = 0; j < oldCards.Length - i - 1; j++)
                {
                    if (CalcPercentageAnswers(j) < CalcPercentageAnswers(j + 1))
                    {
                        (oldCards[j], oldCards[j + 1]) = (oldCards[j + 1], oldCards[j]);
                    }
                }

            this.deck = newCards.Concat(oldCards).ToArray();

            int cardsLimit = ReviewCardsLimit;
            if (cardsLimit < Deck.Length)
                deck = Deck.Where((el, index) => index < cardsLimit).ToArray();

            deck = ShuffleDeck(Deck);
            return Deck;
        }

        private Card[] ShuffleDeck(Card[] deck)
        {
            for (int i = deck.Length - 1; i > 0; i--)
            {
                int j = (int)Math.Floor(new Random().NextDouble() * (i + 1));
                (deck[i], deck[j]) = (deck[j], deck[i]);
            }

            return deck;
        }

        private (Card[], Card[]) SplitCards()
        {
            Card[] newCards = new Card[] { },
                oldCards = new Card[] { };

            for (int i = 0; i < Deck.Length; i++)
            {
                if (Deck[i].RightAnswers == 0 && Deck[i].WrongAnswers == 0)
                    newCards = newCards.Append(Deck[i]).ToArray();
                else
                    oldCards = oldCards.Append(Deck[i]).ToArray();
            }
            return (newCards, oldCards);
        }

        public (int, Card) NextCard()
        {
            if (CurrentCardIndex >= Deck.Length - 1)
            {
                timer.Stop();
                return (CurrentCardIndex, CurrentCard);
            }

            currentCardIndex++;
            currentCard = Deck[CurrentCardIndex];

            return (CurrentCardIndex, CurrentCard);
        }

        public string GetFormattedTime()
        {
            int minutes = (int)Math.Floor((double)seconds / 60);
            string minFormat = minutes < 10 ? "0" + minutes : minutes.ToString();

            int sec = seconds % 60;
            string secFormat = sec < 10 ? "0" + sec : sec.ToString();

            return $"{minFormat}:{secFormat}";
        }
    }
}
