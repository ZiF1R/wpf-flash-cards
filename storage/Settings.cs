using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace course_project1.storage
{
    public class Settings
    {
        public ResourceDictionary currentLang = new ResourceDictionary();
        public ResourceDictionary currentTheme = new ResourceDictionary();
        public int currentThemeId;
        public int currentLangId;

        private int reviewCardsLimit;
        private int reviewTimeLimit;
        public bool isReviewSwitched;

        private enum Setting
        {
            Lang,
            Theme,
            CardsLimit,
            TimeLimit,
            SwitchedReview
        }

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

        public int ReviewCardsLimit
        {
            get => reviewCardsLimit;
            set
            {
                if (value >= 5 && value <= 100)
                    reviewCardsLimit = value;
                else
                    throw new ArgumentOutOfRangeException("Cards count must be in range of 5 and 100!");
            }
        }

        public Settings()
        {
            currentLangId = 1;
            currentThemeId = 1;

            isReviewSwitched = false;
            reviewCardsLimit = 5;
            reviewTimeLimit = 0;

            this.currentLang.Source = new Uri("pack://application:,,,/lang/en.xaml");
            this.currentTheme.Source = new Uri($"pack://application:,,,/theme/Light.xaml");
        }

        public void CreateUserSettings(int uid, SqlConnection connection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO SETTINGS VALUES" +
                $"({uid}, {currentThemeId}, {currentLangId}, {reviewCardsLimit}, '{isReviewSwitched}', {reviewTimeLimit})";
            try
            {
                SqlDataReader commandReader = command.ExecuteReader();
                commandReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Settings insert error!");
            }
        }

        public void LoadSettings(int uid, SqlConnection connection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * " +
                $"FROM SETTINGS " +
                $"WHERE SETTINGS.USER_UID = {uid}";

            SqlDataReader commandReader = command.ExecuteReader();
            if (!commandReader.HasRows)
            {
                commandReader.Close();
                MessageBox.Show("Cannot find user settings!");
                return;
            };

            commandReader.Read();
            try
            {
                int themeId = commandReader.GetInt32(1);
                int langId = commandReader.GetInt32(2);
                int cardsLimit = commandReader.GetInt32(3);
                bool isSwitched = commandReader.GetString(4) == "True";
                int timeLimit = commandReader.GetInt32(5);

                currentLangId = langId;
                currentThemeId = themeId;
                reviewCardsLimit = cardsLimit;
                isReviewSwitched = isSwitched;
                reviewTimeLimit = timeLimit;
                commandReader.Close();

                this.SetAppTheme(this.GetThemeName(currentThemeId, connection));
                this.SetAppLang(this.GetLangName(currentLangId, connection));
            }
            catch (Exception ex)
            {
                MessageBox.Show("User settings loading error!");
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }

        ///

        public void ChangeAppTheme(int id, SqlConnection connection)
        {
            this.currentThemeId = id;
            this.UploadSettingChanges(Setting.Theme, connection);
            this.SetAppTheme(this.GetThemeName(currentThemeId, connection));
        }

        public string GetThemeName(int id, SqlConnection connection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                "SELECT THEME " +
                "FROM THEMES " +
                $"WHERE THEMES.THEME_ID = {id}";

            SqlDataReader commandReader = command.ExecuteReader();
            if (!commandReader.HasRows)
            {
                commandReader.Close();
                MessageBox.Show("Cannot get theme!");
                return null;
            };

            commandReader.Read();
            string theme = commandReader.GetString(0);
            theme = $"{theme[0]}{theme.ToLower().Remove(0, 1)}";
            commandReader.Close();

            return theme;
        }

        public void SetAppTheme(string theme)
        {
            Uri newSource = new Uri($"pack://application:,,,/theme/{theme}.xaml");
            ResourceDictionary newResource = new ResourceDictionary();
            newResource.Source = newSource;
            Application.Current.Resources.MergedDictionaries.Remove(currentTheme);
            Application.Current.Resources.MergedDictionaries.Add(newResource);
            currentTheme.Source = newSource;
        }

        ///

        public void ChangeAppLang(int id, SqlConnection connection)
        {
            this.currentLangId = id;
            this.UploadSettingChanges(Setting.Lang, connection);
            this.SetAppLang(this.GetLangName(currentLangId, connection));
        }

        public string GetLangName(int id, SqlConnection connection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                "SELECT LANG " +
                "FROM LANGS " +
                $"WHERE LANGS.LANG_ID = {id}";

            SqlDataReader commandReader = command.ExecuteReader();
            if (!commandReader.HasRows)
            {
                commandReader.Close();
                MessageBox.Show("Cannot get theme!");
                return null;
            };

            commandReader.Read();
            string lang = commandReader.GetString(0).ToLower();
            commandReader.Close();

            return lang;
        }

        public void SetAppLang(string lang)
        {
            Uri newSource = new Uri($"pack://application:,,,/lang/{lang}.xaml");
            ResourceDictionary newResource = new ResourceDictionary();
            newResource.Source = newSource;
            Application.Current.Resources.MergedDictionaries.Remove(currentLang);
            Application.Current.Resources.MergedDictionaries.Add(newResource);
            currentLang.Source = newSource;
        }

        ///

        public void ChangeReviewSwitched(bool value, SqlConnection connection)
        {
            this.isReviewSwitched = value;
            this.UploadSettingChanges(Setting.SwitchedReview, connection);
        }

        public void ChangeTimeLimit(int limit, SqlConnection connection)
        {
            this.ReviewTimeLimit = limit;
            this.UploadSettingChanges(Setting.TimeLimit, connection);
        }

        public void ChangeCardsLimit(int limit, SqlConnection connection)
        {
            this.ReviewCardsLimit = limit;
            this.UploadSettingChanges(Setting.CardsLimit, connection);
        }

        ///

        private void UploadSettingChanges(Setting setting, SqlConnection connection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                "UPDATE SETTINGS SET ";

            switch (setting)
            {
                case Setting.Lang:
                    command.CommandText += $"ACTIVE_LANG = {currentLangId}";
                    break;
                case Setting.Theme:
                    command.CommandText += $"ACTIVE_THEME = {currentThemeId}";
                    break;
                case Setting.CardsLimit:
                    command.CommandText += $"CARDS_LIMIT = {ReviewCardsLimit}";
                    break;
                case Setting.TimeLimit:
                    command.CommandText += $"TIME_LIMIT = {ReviewTimeLimit}";
                    break;
                case Setting.SwitchedReview:
                    command.CommandText += $"SWITCHED_REVIEW = '{isReviewSwitched}'";
                    break;
            }

            int uid = ((MainWindow)System.Windows.Application.Current.MainWindow).Storage.user.Uid;
            command.CommandText += $" WHERE SETTINGS.USER_UID = {uid}";
            
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Upload settings error!");
            }
        }
    }
}
