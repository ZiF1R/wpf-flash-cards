using course_project1.controls.ModalWindows;
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
                    throw new ArgumentOutOfRangeException();
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
                    throw new ArgumentOutOfRangeException();
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

        public void CreateUserSettings(string connectionString, int uid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        "INSERT INTO SETTINGS VALUES" +
                        $"({uid}, {currentThemeId}, {currentLangId}, {reviewCardsLimit}, '{isReviewSwitched}', {reviewTimeLimit})";

                    SqlDataReader commandReader = command.ExecuteReader();
                    commandReader.Close();
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("SettingsInsertError"));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void LoadSettings(string connectionString, int uid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        $"SELECT * " +
                        $"FROM SETTINGS " +
                        $"WHERE SETTINGS.USER_UID = {uid}";

                    SqlDataReader commandReader = command.ExecuteReader();
                    if (!commandReader.HasRows)
                    {
                        commandReader.Close();
                        CustomMessage.Show((string)Application.Current.FindResource("FindSettingsError"));
                        connection.Close();
                        return;
                    };

                    commandReader.Read();
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

                    this.SetAppTheme(this.GetThemeName(currentThemeId, connectionString));
                    this.SetAppLang(this.GetLangName(currentLangId, connectionString));
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("SettingsLoadingError"));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        ///

        public void ChangeAppTheme(int id, string connectionString, int uid)
        {
            this.currentThemeId = id;
            this.UploadSettingChanges(Setting.Theme, connectionString, uid);
            this.SetAppTheme(this.GetThemeName(currentThemeId, connectionString));
        }

        public string GetThemeName(int id, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        "SELECT THEME " +
                        "FROM THEMES " +
                        $"WHERE THEMES.THEME_ID = {id}";

                    SqlDataReader commandReader = command.ExecuteReader();
                    if (!commandReader.HasRows)
                    {
                        commandReader.Close();
                        CustomMessage.Show((string)Application.Current.FindResource("GetThemeError"));
                        connection.Close();
                        return null;
                    };

                    commandReader.Read();
                    string theme = commandReader.GetString(0);
                    theme = $"{theme[0]}{theme.ToLower().Remove(0, 1)}";
                    commandReader.Close();
                    connection.Close();

                    return theme;
                }
                catch (Exception ex)
                {
                    CustomMessage.Show(ex.Message);
                    return "Light";
                }
            }
        }

        public void SetAppTheme(string theme)
        {
            try
            {
                Uri newSource = new Uri($"pack://application:,,,/theme/{theme}.xaml");
                ResourceDictionary newResource = new ResourceDictionary();
                newResource.Source = newSource;
                Application.Current.Resources.MergedDictionaries.Remove(currentTheme);
                Application.Current.Resources.MergedDictionaries.Add(newResource);
                currentTheme.Source = newSource;
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
            }
        }

        ///

        public void ChangeAppLang(int id, string connectionString, int uid)
        {
            this.currentLangId = id;
            this.UploadSettingChanges(Setting.Lang, connectionString, uid);
            this.SetAppLang(this.GetLangName(currentLangId, connectionString));
        }

        public string GetLangName(int id, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        "SELECT LANG " +
                        "FROM LANGS " +
                        $"WHERE LANGS.LANG_ID = {id}";

                    SqlDataReader commandReader = command.ExecuteReader();
                    if (!commandReader.HasRows)
                    {
                        commandReader.Close();
                        CustomMessage.Show((string)Application.Current.FindResource("GetLangError"));
                        connection.Close();
                        return null;
                    };

                    commandReader.Read();
                    string lang = commandReader.GetString(0).ToLower();
                    commandReader.Close();
                    connection.Close();

                    return lang;
                }
                catch (Exception ex)
                {
                    CustomMessage.Show(ex.Message);
                    return "EN";
                }
            }
        }

        public void SetAppLang(string lang)
        {
            try
            {
                Uri newSource = new Uri($"pack://application:,,,/lang/{lang}.xaml");
                ResourceDictionary newResource = new ResourceDictionary();
                newResource.Source = newSource;
                Application.Current.Resources.MergedDictionaries.Remove(currentLang);
                Application.Current.Resources.MergedDictionaries.Add(newResource);
                currentLang.Source = newSource;
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
            }
        }

        ///

        public void ChangeReviewSwitched(bool value, string connectionString, int uid)
        {
            this.isReviewSwitched = value;
            this.UploadSettingChanges(Setting.SwitchedReview, connectionString, uid);
        }

        public void ChangeTimeLimit(int limit, string connectionString, int uid)
        {
            this.ReviewTimeLimit = limit;
            this.UploadSettingChanges(Setting.TimeLimit, connectionString, uid);
        }

        public void ChangeCardsLimit(int limit, string connectionString, int uid)
        {
            this.ReviewCardsLimit = limit;
            this.UploadSettingChanges(Setting.CardsLimit, connectionString, uid);
        }

        ///

        private void UploadSettingChanges(Setting setting, string connectionString, int uid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
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

                    command.CommandText += $" WHERE SETTINGS.USER_UID = {uid}";
                    command.ExecuteNonQuery();
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("SettingsUploadError"));
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
