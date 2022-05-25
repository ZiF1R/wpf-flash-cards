using course_project1.controls.ModalWindows;
using course_project1.storage;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace course_project1
{
    public class DataStorage
    {
        public User user;
        public Settings settings;
        public Folder[] folders;
        public Category[] categories;

        public DataStorage()
        {
            user = new User();
            settings = new Settings();
            folders = new Folder[] { };
            categories = new Category[] { new Category("none") };
        }

        public void LoadFolders(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand comand = connection.CreateCommand();
                    comand.CommandText =
                        $"SELECT * " +
                        $"FROM FOLDERS " +
                        $"WHERE FOLDERS.USER_UID = {this.user.Uid}";

                    SqlDataReader comandReader = comand.ExecuteReader();
                    if (!comandReader.HasRows)
                    {
                        comandReader.Close();
                        return;
                    }

                    while (comandReader.Read())
                    {
                        int folderId = comandReader.GetInt32(0);
                        string folderName = comandReader.GetString(2);
                        DateTime created = comandReader.GetDateTime(3);
                        string folderCategory = comandReader.GetString(4);

                        Folder folder = new Folder(folderId, folderName, folderCategory, created);
                        folders = folders.Append(folder).ToArray();
                    }
                    comandReader.Close();

                    foreach (Folder folder1 in folders)
                        folder1.LoadFolderCards(connectionString);
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("FoldersLoadingError"));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void LoadCategories(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand comand = connection.CreateCommand();
                    comand.CommandText =
                        $"SELECT CATEGORY " +
                        $"FROM CATEGORIES " +
                        $"WHERE CATEGORIES.USER_UID = {this.user.Uid}";

                    SqlDataReader comandReader = comand.ExecuteReader();
                    if (!comandReader.HasRows)
                    {
                        comandReader.Close();
                        return;
                    }

                    while (comandReader.Read())
                    {
                        string category = comandReader.GetString(0);
                        categories = categories.Append(new Category(category)).ToArray();
                    }
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("CategoriesLoadingError"));
                }
                connection.Close();
            }
        }

        public bool IsUnusedCategory(string category)
        {
            return !this.folders.Where(folder => folder.Category == category).Any();
        }

        public void Clear()
        {
            user = new User();
            settings = new Settings();
            folders = new Folder[] { };
            categories = new Category[] { new Category("none") };
        }
    }
}
