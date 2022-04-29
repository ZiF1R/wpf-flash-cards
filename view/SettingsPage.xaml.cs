using course_project1.controls.ModalWindows;
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

namespace course_project1.view
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        DataStorage Storage;
        string ConnectionString;
        Frame SecondFrame;
        Grid MainPageGrid;

        public SettingsPage(Grid mainPageGrid, string connectionString, DataStorage storage, Frame secondFrame)
        {
            this.MainPageGrid = mainPageGrid;
            SecondFrame = secondFrame;
            ConnectionString = connectionString;
            Storage = storage;
            InitializeComponent();
            ApplySettings();
        }

        private void ApplySettings()
        {
            (int min, int max, int step) cardsNum = (5, 100, 5);
            for (int i = cardsNum.min; i < cardsNum.max + 1; i += cardsNum.step)
            {
                CardsNumber.Items.Add(i);
                if (i == Storage.settings.ReviewCardsLimit)
                    CardsNumber.SelectedValue = i;
            }

            (int min, int max, int step) timeLimit = (0, 120, 5);
            for (int i = timeLimit.min; i < timeLimit.max + 1; i += timeLimit.step)
            {
                TimeLimit.Items.Add(i);
                if (i == Storage.settings.ReviewTimeLimit)
                    TimeLimit.SelectedValue = i;
            }

            ReviewSwitch.Switched = Storage.settings.isReviewSwitched;
            ThemeSwitch.Switched = Storage.settings.currentThemeId != 1;
        }

        private void ThemeSwitch_SwitchChanged(object sender, RoutedEventArgs e)
        {
            if (ThemeSwitch.Switched)
                Storage.settings.ChangeAppTheme(2, ConnectionString, Storage.user.Uid);
            else
                Storage.settings.ChangeAppTheme(1, ConnectionString, Storage.user.Uid);
        }

        private void ReviewSwitch_SwitchChanged(object sender, RoutedEventArgs e)
        {
            Storage.settings.ChangeReviewSwitched(ReviewSwitch.Switched, ConnectionString, Storage.user.Uid);
        }

        private void CardsNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (CardsNumber.Text != "")
                    Storage.settings.ChangeCardsLimit(Convert.ToInt32(CardsNumber.SelectedItem), ConnectionString, Storage.user.Uid);
            }
            catch
            {
                CustomMessage.Show((string)Application.Current.FindResource("SetCardsLimitError"));
            }
        }

        private void TimeLimit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (TimeLimit.Text != "")
                    Storage.settings.ChangeTimeLimit(Convert.ToInt32(TimeLimit.SelectedItem), ConnectionString, Storage.user.Uid);
            }
            catch
            {
                CustomMessage.Show((string)Application.Current.FindResource("SetTimeLimitError"));
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new FoldersPage(
                MainPageGrid, SecondFrame, ConnectionString, Storage, true, controls.FolderControl.Action.Export);
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            SecondFrame.Content = new FoldersPage(
                MainPageGrid, SecondFrame, ConnectionString, Storage, true, controls.FolderControl.Action.Import);
        }
    }
}
