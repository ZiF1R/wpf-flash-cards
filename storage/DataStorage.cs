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

        public void LoadFolders(SqlConnection connection, int uid)
        {
            SqlCommand comand = connection.CreateCommand();
            comand.CommandText =
                $"SELECT * " +
                $"FROM FOLDERS " +
                $"WHERE FOLDERS.USER_UID = {uid}";

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

                    Folder folder = new Folder(connection, folderId, folderName, folderCategory, created);
                    folders = folders.Append(folder).ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Folders loading error!");
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                return;
            }
            finally
            {
                comandReader.Close();
            }
        }

        public void LoadCategories()
        {
            // **load user categories**
        }
    }
}
