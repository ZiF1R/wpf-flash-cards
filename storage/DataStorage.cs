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
        public string[] categories;

        public DataStorage()
        {
            user = new User();
            settings = new Settings();
            folders = new Folder[] { };
            categories = new string[] { "none" };
        }

        public bool AddCategory(string connectionString, string category)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var addCategoryCommand = string.Format("INSERT INTO CATEGORIES VALUES(@uid, @category)");
                using (SqlCommand command = new SqlCommand(addCategoryCommand, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@uid", this.user.Uid);
                        command.Parameters.AddWithValue("@category", category);
                        command.ExecuteNonQuery();
                        this.categories = categories.Append(category).ToArray();
                    }
                    catch
                    {
                        connection.Close();
                        CustomMessage.Show((string)Application.Current.FindResource("CategoryInsertError"));
                        return false;
                    }
                }
                connection.Close();
                return true;
            }
        }

        public bool RemoveCategory(string connectionString, string category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    "DELETE CATEGORIES WHERE " +
                    $"CATEGORIES.USER_UID = {user.Uid} AND CATEGORIES.CATEGORY = '{category}'";
                try
                {
                    SqlDataReader commandReader = command.ExecuteReader();
                    commandReader.Close();
                }
                catch
                {
                    connection.Close();
                    CustomMessage.Show((string)Application.Current.FindResource("CategoryRemoveError"));
                    return false;
                }
                connection.Close();

                categories = categories.Where(c => c != category).ToArray();
                return true;
            }
        }

        public void LoadFolders(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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

                try
                {
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
                    comandReader.Close();
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

                try
                {
                    while (comandReader.Read())
                    {
                        string category = comandReader.GetString(0);
                        categories = categories.Append(category).ToArray();
                    }
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("CategoriesLoadingError"));
                }
                finally
                {
                    comandReader.Close();
                }
                connection.Close();
            }
        }

        public bool CheckForUniqueCategory(string connectionString, string category)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    $"SELECT * " +
                    $"FROM CATEGORIES " +
                    $"WHERE CATEGORIES.USER_UID = {this.user.Uid} AND CATEGORIES.CATEGORY = '{category}'";
                SqlDataReader commandReader = command.ExecuteReader();

                bool isUnique = !commandReader.HasRows;
                commandReader.Close();

                connection.Close();
                return isUnique;
            }
        }

        public void Clear()
        {
            user = new User();
            settings = new Settings();
            folders = new Folder[] { };
            categories = new string[] { };
        }
    }
}
