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

        public Card(string connectionString, int rootFolderId, string term, string translation, string examples)
        {
            Term = term;
            Translation = translation;
            Examples = examples;
            Created = DateTime.Now;
            this.rightAnswers = 0;
            this.wrongAnswers = 0;
            this.isMemorized = false;

            if (!InsertCard(connectionString, rootFolderId))
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

        private bool InsertCard(string connectionString, int rootFolderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                        connection.Close();
                    }
                    catch
                    {
                        connection.Close();
                        MessageBox.Show("Card insert error!");
                        return false;
                    }
                }
                return true;
            }
        }

        internal static bool IsUniqueCardTerm(string connectionString, int folderId, string term)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    $"SELECT * " +
                    $"FROM CARDS " +
                    $"WHERE CARDS.FOLDER_ID = {folderId} AND CARDS.TERM = '{term}'";
                SqlDataReader commandReader = command.ExecuteReader();

                bool isUnique = !commandReader.HasRows;
                commandReader.Close();

                connection.Close();
                return isUnique;
            }
        }

        public bool RemoveCard(string connectionString, int rootFolderId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                    connection.Close();
                    MessageBox.Show("Card remove error!");
                    return false;
                }
                connection.Close();
                return true;
            }
        }

        public void ChangeCardData(string connectionString, int rootFolderId, string term, string translation, string examples)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
                finally
                {
                    connection.Close();
                }
            }
        }

        public void SendAnswer(string ConnectionString, int folderId, bool isRight, bool isSetAsCorrect = false)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    if (!isSetAsCorrect)
                    {
                        if (isRight)
                        {
                            command.CommandText = "Update CARDS " +
                                "set RIGHT_ANSWERS = RIGHT_ANSWERS + 1, IS_MEMORIZED = 'True' " +
                                $"Where FOLDER_ID = {folderId} and TERM = '{Term}'";
                            rightAnswers++;
                        }
                        else
                        {
                            command.CommandText = "Update CARDS " +
                                "set WRONG_ANSWERS = WRONG_ANSWERS + 1, IS_MEMORIZED = 'False' " +
                                $"Where FOLDER_ID = {folderId} and TERM = '{Term}'";
                            wrongAnswers++;
                        }
                    }
                    else
                    {
                        command.CommandText = "Update CARDS " +
                            "set RIGHT_ANSWERS = RIGHT_ANSWERS + 1, WRONG_ANSWERS = WRONG_ANSWERS - 1, IS_MEMORIZED = 'True' " +
                            $"Where FOLDER_ID = {folderId} and TERM = '{Term}'";
                        rightAnswers++;
                        wrongAnswers--;
                    }
                    command.ExecuteNonQuery();
                    isMemorized = isRight;
                }
                catch
                {
                    MessageBox.Show("Cannot send answer to database!");
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
