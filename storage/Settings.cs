using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project1.storage
{
    public class Settings
    {
        public int currentThemeId;
        public int currentLangId;

        private int reviewCardsCount;
        private int reviewTimeLimit;
        public bool isReviewSwitched;

        public int ReviewTimeLimit
        {
            get => reviewTimeLimit;
            set
            {
                if (value >= 0 && value <= 120)
                    reviewTimeLimit = value;
                else
                    throw new ArgumentOutOfRangeException("Time limit must be in range of 0 and 120!");
            }
        }

        public int ReviewCardsCount
        {
            get => reviewCardsCount;
            set
            {
                if (value >= 5 && value <= 100)
                    reviewCardsCount = value;
                else
                    throw new ArgumentOutOfRangeException("Cards count must be in range of 5 and 100!");
            }
        }

        public Settings()
        {
            currentLangId = 1;
            currentThemeId = 1;

            isReviewSwitched = false;
            reviewCardsCount = 5;
            reviewTimeLimit = 0;
        }

        public void LoadSettings(User user)
        {
            // **get user settings**
        }
    }
}
