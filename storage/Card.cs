using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace course_project1.storage
{
    public class Card
    {
        private string term;
        private string translation;
        public string Examples { get; set; }
        public DateTime Created { get; }
        private bool isMemorized;
        private int rightAnswers;
        private int wrongAnswers;

        public string Term
        {
            get => term;
            set
            {
                if (value != "")
                    term = value;
                else
                    throw new ArgumentException();
            }
        }

        public string Translation
        {
            get => translation;
            set
            {
                if (value != "")
                    translation = value;
                else
                    throw new ArgumentException();
            }
        }

        public bool IsMemorized { get => isMemorized; }
        public int RightAnswers { get => rightAnswers; }
        public int WrongAnswers { get => wrongAnswers; }

        public Card(SqlConnection connection, int rootFolderId, string term, string translation, string examples)
        {
            Term = term;
            Translation = translation;
            Examples = examples;
            Created = DateTime.Now;
            this.rightAnswers = 0;
            this.wrongAnswers = 0;
            this.isMemorized = false;

            if (!InsertCard(connection, rootFolderId))
                throw new Exception();
        }

        public Card(string term, string translation, string examples, DateTime created, bool isMemorized, int rightAnswers, int wrongAnswers)
        {
            Term = term;
            Translation = translation;
            Examples = examples;
            Created = created;
            this.rightAnswers = rightAnswers;
            this.wrongAnswers = wrongAnswers;
            this.isMemorized = isMemorized;
        }

        private bool InsertCard(SqlConnection connection, int rootFolderId)
        {
            var addFolderCommand = string.Format("INSERT INTO CARDS VALUES(" +
                "@folderId, @created, @term, @translation, @examples, @memorized, @right, @wrong)");
            using (SqlCommand command = new SqlCommand(addFolderCommand, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@folderId", rootFolderId);
                    command.Parameters.AddWithValue("@created", Created);
                    command.Parameters.AddWithValue("@term", Term);
                    command.Parameters.AddWithValue("@translation", Translation);
                    command.Parameters.AddWithValue("@examples", Examples);
                    command.Parameters.AddWithValue("@memorized", IsMemorized.ToString());
                    command.Parameters.AddWithValue("@right", RightAnswers);
                    command.Parameters.AddWithValue("@wrong", WrongAnswers);
                    command.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Card insert error!");
                    return false;
                }
            }
            return true;
        }

        internal static bool IsUniqueCardTerm(SqlConnection connection, int folderId, string term)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * " +
                $"FROM CARDS " +
                $"WHERE CARDS.FOLDER_ID = {folderId} AND CARDS.TERM = '{term}'";
            SqlDataReader commandReader = command.ExecuteReader();

            bool isUnique = !commandReader.HasRows;
            commandReader.Close();

            return isUnique;
        }

        public bool RemoveCard(SqlConnection connection, int rootFolderId)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                "DELETE CARDS WHERE " +
                $"CARDS.FOLDER_ID = {rootFolderId} AND CARDS.TERM = '{Term}'";
            try
            {
                SqlDataReader commandReader = command.ExecuteReader();
                commandReader.Close();
            }
            catch
            {
                MessageBox.Show("Card remove error!");
                return false;
            }
            return true;
        }

        public void ChangeCardData(SqlConnection connection, int rootFolderId, string term, string translation, string examples)
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    $"UPDATE CARDS " +
                    $"SET TERM = '{term}', TRANSLATION = '{translation}', EXAMPLES = '{examples}' FROM CARDS " +
                    $"WHERE FOLDER_ID = {rootFolderId} AND TERM = '{Term}'";
                SqlDataReader commandReader = command.ExecuteReader();
                commandReader.Close();

                Term = term;
                Translation = translation;
                Examples = examples;
            }
            catch
            {
                MessageBox.Show("Card update error!");
            }
        }
    }
}
