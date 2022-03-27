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

namespace course_project1.view
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        Frame rootFrame;

        public SettingsPage(Frame frame)
        {
            InitializeComponent();
            rootFrame = frame;

            (int min, int max, int step) cardsNum = (5, 100, 5);
            for (int i = cardsNum.min; i < cardsNum.max + 1; i += cardsNum.step)
            {
                CardsNumber.Items.Add(i);
                if (i == cardsNum.min)
                    CardsNumber.SelectedValue = i;
            }

            (int min, int max, int step) timeLimit = (0, 120, 5);
            for (int i = timeLimit.min; i < timeLimit.max + 1; i += timeLimit.step)
            {
                TimeLimit.Items.Add(i);
                if (i == timeLimit.min)
                    TimeLimit.SelectedValue = i;
            }
        }
    }
}
