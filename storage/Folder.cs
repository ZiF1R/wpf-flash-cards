using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace course_project1.storage
{
    public class Folder
    {
        private string name;
        private string category;
        public DateTime Created { get; }
        public Card[] Cards { get; set; }

        public string Name
        {
            get => name;
            set
            {
                if (value != "")
                    name = value;
                else
                    throw new ArgumentException();
            }
        }

        public string Category
        {
            get => category;
            set
            {
                if (value != "")
                    category = value;
                else
                    throw new ArgumentException();
            }
        }

        public Folder(SqlConnection connection, int uid, string name, string category)
        {
            Name = name;
            Category = category;
            Created = DateTime.Now;
            Cards = new Card[] { };

            InsertFolder(connection, uid);
        }

        public Folder(SqlConnection connection, int folderId, string name, string category, DateTime created)
        {
            Name = name;
            Category = category;
            Created = created;

            LoadFolderCards(connection, folderId);
        }

        private void LoadFolderCards(SqlConnection connection, int folderId)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * " +
                $"FROM CARDS " +
                $"WHERE CARDS.FOLDER_ID = {folderId}";

            SqlDataReader comandReader = command.ExecuteReader();
            if (comandReader.HasRows)
            {
                try
                {
                    while (comandReader.Read())
                    {
                        DateTime created = comandReader.GetDateTime(1);
                        string term = comandReader.GetString(2);
                        string translation = comandReader.GetString(3);
                        string examples = comandReader.GetString(4);
                        bool isMemorized = comandReader.GetString(5) == "True";
                        int rightAnswers = comandReader.GetInt32(6);
                        int wrongAnswers = comandReader.GetInt32(7);

                        Card card = new Card(term, translation, examples, created, isMemorized, rightAnswers, wrongAnswers);
                        Cards = Cards.Append(card).ToArray();
                    }
                }
                catch
                {
                    MessageBox.Show("Cards loading error!");
                    return;
                }
            }
            comandReader.Close();
        }

        public int MemorizedCardsCount()
        {
            return Cards.Where(card => card.isMemorized).ToArray().Length;
        }

        private bool InsertFolder(SqlConnection connection, int uid)
        {
            var addFolderCommand = string.Format("INSERT INTO FOLDERS VALUES(@id, @name, @created, @category)");
            using (SqlCommand command = new SqlCommand(addFolderCommand, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@id", uid);
                    command.Parameters.AddWithValue("@name", Name);
                    command.Parameters.AddWithValue("@created", Created);
                    command.Parameters.AddWithValue("@category", Category);
                    command.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Folder insert error!");
                    return false;
                }
            }
            return true;
        }

        public void RemoveFolder(SqlConnection connection, int uid)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                "DELETE FOLDERS WHERE " +
                $"FOLDERS.USER_UID = {uid} AND FOLDERS.FOLDER_NAME = '{Name}'";
            try
            {
                SqlDataReader commandReader = command.ExecuteReader();
                commandReader.Close();
            }
            catch
            {
                MessageBox.Show("Folder remove error!");
            }
        }

        public void ChangeFolderData(SqlConnection connection, int uid, string newName, string newCategory)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    $"UPDATE FOLDERS " +
                    $"SET FOLDER_NAME = '{newName}', CATEGORY = '{newCategory}' FROM FOLDERS " +
                    $"WHERE USER_UID = {uid} AND FOLDER_NAME = '{Name}'";
                SqlDataReader commandReader = command.ExecuteReader();
                commandReader.Close();
            }
            catch
            {
                MessageBox.Show("Folder update error!");
            }

            Name = newName;
            Category = newCategory;
        }

        public static bool IsUniqueFolderName(SqlConnection connection, int uid, string folderName)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * " +
                $"FROM FOLDERS " +
                $"WHERE FOLDERS.USER_UID = {uid} AND FOLDERS.FOLDER_NAME = '{folderName}'";
            SqlDataReader commandReader = command.ExecuteReader();

            bool isUnique = !commandReader.HasRows;
            commandReader.Close();

            return isUnique;
        }
    }
}
