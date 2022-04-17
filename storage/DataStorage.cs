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

        public bool AddCategory(SqlConnection connection, string category)
        {
            this.categories = categories.Append(category).ToArray();

            var addCategoryCommand = string.Format("INSERT INTO CATEGORIES VALUES(@uid, @category)");
            using (SqlCommand command = new SqlCommand(addCategoryCommand, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@uid", this.user.Uid);
                    command.Parameters.AddWithValue("@category", category);
                    command.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Category insert error!");
                    return false;
                }
            }
            return true;
        }

        public void LoadFolders(SqlConnection connection)
        {
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
                    folder1.LoadFolderCards(connection);
            }
            catch
            {
                MessageBox.Show("Folders loading error!");
                comandReader.Close();
            }
        }

        public void LoadCategories(SqlConnection connection)
        {
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
                MessageBox.Show("Categories loading error!");
            }
            finally
            {
                comandReader.Close();
            }
        }

        public bool CheckForUniqueCategory(SqlConnection connection, string category)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * " +
                $"FROM CATEGORIES " +
                $"WHERE CATEGORIES.USER_UID = {this.user.Uid} AND CATEGORIES.CATEGORY = '{category}'";
            SqlDataReader commandReader = command.ExecuteReader();

            bool isUnique = !commandReader.HasRows;
            commandReader.Close();

            return isUnique;
        }
    }
}
