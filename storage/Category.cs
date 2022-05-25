using course_project1.controls.ModalWindows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace course_project1.storage
{
    public class Category
    {
        private string name;

        public string Name
        {
            get { return name; }
        }

        public Category(string name)
        {
            this.name = name;
        }

        public bool AddCategory(string connectionString, string category, int uid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var addCategoryCommand = string.Format("INSERT INTO CATEGORIES VALUES(@uid, @category)");
                    using (SqlCommand command = new SqlCommand(addCategoryCommand, connection))
                    {
                        command.Parameters.AddWithValue("@uid", uid);
                        command.Parameters.AddWithValue("@category", category);
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    connection.Close();
                    CustomMessage.Show((string)Application.Current.FindResource("CategoryInsertError"));
                    return false;
                }
                connection.Close();
                return true;
            }
        }

        public bool RemoveCategory(string connectionString, string category, int uid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        "DELETE CATEGORIES WHERE " +
                        $"CATEGORIES.USER_UID = {uid} AND CATEGORIES.CATEGORY = '{category}'";
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
                return true;
            }
        }

        public static bool CheckForUniqueCategory(string connectionString, string category, int uid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        $"SELECT * " +
                        $"FROM CATEGORIES " +
                        $"WHERE CATEGORIES.USER_UID = {uid} AND CATEGORIES.CATEGORY = '{category}'";
                    SqlDataReader commandReader = command.ExecuteReader();

                    bool isUnique = !commandReader.HasRows;
                    commandReader.Close();

                    connection.Close();
                    return isUnique;
                }
                catch (Exception ex)
                {
                    CustomMessage.Show(ex.Message);
                    return false;
                }
            }
        }
    }
}
