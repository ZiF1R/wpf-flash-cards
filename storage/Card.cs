using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project1.storage
{
    public class Card
    {
        public string term;
        public string translation;
        public string Examples { get; set; }
        public DateTime Created { get; }
        public bool isMemorized;
        public int rightAnswers;
        public int wrongAnswers;

        public string Term
        {
            get => term;
            set
            {
                if (value != "")
                    term = value;
                else
                    throw new ArgumentException();
            }
        }

        public string Translation
        {
            get => translation;
            set
            {
                if (value != "")
                    translation = value;
                else
                    throw new ArgumentException();
            }
        }

        public bool IsMemorized { get => isMemorized; }
        public int RightAnswers { get => rightAnswers; }
        public int WrongAnswers { get => wrongAnswers; }

        public Card(string term, string translation, string examples, DateTime created, bool isMemorized, int rightAnswers, int wrongAnswers)
        {
            Term = term;
            Translation = translation;
            Examples = examples;
            Created = created;
            this.rightAnswers = rightAnswers;
            this.wrongAnswers = wrongAnswers;
            this.isMemorized = isMemorized;
        }
    }
}
