using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace course_project1.storage
{
    public class User
    {
        public string nickname;
        public string surname;
        public string name;
        private string email;
        private string Password;

        public string Nickname
        {
            get => nickname;
            set
            {
                if (value != "") nickname = value;
                else throw new ArgumentException();
            }
        }

        public string Surname
        {
            get => surname;
            set
            {
                if (value != "") surname = value;
                else throw new ArgumentException();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (value != "") name = value;
                else throw new ArgumentException();
            }
        }

        public string Email
        {
            get => email;
        }

        public User()
        {
            nickname = "";
            surname = "";
            name = "";
            email = "";
            Password = "";
        }

        public User(string nickname, string surname, string name, string email, string password, SqlConnection connection)
        {
            bool isUnique = CheckForUniqueEmail(connection, email);
            if (!isUnique) throw new ArgumentException("Such email already used!");

            Nickname = nickname;
            Surname = surname;
            Name = name;
            this.email = email;
            Password = password;

            InsertUser(connection);
        }

        public bool LoadUser(string email, string password, SqlConnection connection)
        {
            SqlCommand loginCommand = connection.CreateCommand();
            loginCommand.CommandText =
                $"SELECT * " +
                $"FROM USERS " +
                $"WHERE USERS.EMAIL = '{email}' AND USERS.PASS = '{password}'";

            SqlDataReader loginCommandReader = loginCommand.ExecuteReader();
            if (!loginCommandReader.HasRows)
            {
                loginCommandReader.Close();
                return false;
            };

            loginCommandReader.Read();
            string nickname = loginCommandReader.GetString(1);
            string surname = loginCommandReader.GetString(2);
            string name = loginCommandReader.GetString(3);
            loginCommandReader.Close();

            Nickname = nickname;
            Surname = surname;
            Name = name;
            this.email = email;
            Password = password;

            return true;
        }

        private bool CheckForUniqueEmail(SqlConnection connection, string email)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * " +
                $"FROM USERS " +
                $"WHERE USERS.EMAIL = '{email}'";
            SqlDataReader commandReader = command.ExecuteReader();

            bool isUnique = !commandReader.HasRows;
            commandReader.Close();

            return isUnique;
        }

        private void InsertUser(SqlConnection connection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO USERS VALUES" +
                $"('{nickname}', '{surname}', '{name}', '{email}', '{Password}')";
            SqlDataReader commandReader = command.ExecuteReader();
            try
            {
                commandReader.Close();
            }
            catch
            {
                MessageBox.Show("User insert error!");
                commandReader.Close();
            }
        }

        public void ChangeUserData(string nickname, string surname, string name, SqlConnection connection)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText =
                $"UPDATE USERS " +
                $"SET NICKNAME = '{nickname}', SURNAME = '{surname}', NAME = '{name}' FROM USERS " +
                $"WHERE EMAIL = '{this.Email}'";
            SqlDataReader commandReader = command.ExecuteReader();
            try
            {
                commandReader.Close();
            }
            catch
            {
                MessageBox.Show("User update error!");
                commandReader.Close();
            }

            Nickname = nickname;
            Surname = surname;
            Name = name;
        }
    }
}
