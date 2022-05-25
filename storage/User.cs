using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using course_project1.controls.ModalWindows;

namespace course_project1.storage
{
    public class User
    {
        private int uid;
        public string nickname;
        public string surname;
        public string name;
        private string email;
        private string password;

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

        public string Password
        {
            get { return password; }
        }

        public int Uid
        {
            get => uid;
        }

        public User()
        {
            nickname = "";
            surname = "";
            name = "";
            email = "";
            password = "";
        }

        public User(string nickname, string surname, string name, string email, string password, string connectionString)
        {
            bool isUnique = CheckForUniqueEmail(connectionString, email);
            if (!isUnique) throw new ArgumentException((string)Application.Current.FindResource("EmailAlreadyUsed"));

            Nickname = nickname;
            Surname = surname;
            Name = name;
            this.email = email;

            try
            {
                this.password = DataEncriptor.Encrypt(password);
            }
            catch
            {
                CustomMessage.Show((string)Application.Current.FindResource("PasswordEncryptionError"));
            }

            InsertUser(connectionString);
            LoadUser(this.email, DataEncriptor.Decrypt(Password), connectionString);
        }

        public bool LoadUser(string email, string password, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string encryptedPass = DataEncriptor.Encrypt(password);
                    SqlCommand loginCommand = connection.CreateCommand();
                    loginCommand.CommandText =
                        $"SELECT * " +
                        $"FROM USERS " +
                        $"WHERE USERS.EMAIL = '{email}' AND USERS.PASS = '{encryptedPass}'";

                    SqlDataReader loginCommandReader = loginCommand.ExecuteReader();
                    if (!loginCommandReader.HasRows)
                    {
                        loginCommandReader.Close();
                        connection.Close();
                        return false;
                    };

                    loginCommandReader.Read();
                    int id = loginCommandReader.GetInt32(0);
                    string nickname = loginCommandReader.GetString(1);
                    string surname = loginCommandReader.GetString(2);
                    string name = loginCommandReader.GetString(3);

                    this.uid = id;
                    Nickname = nickname;
                    Surname = surname;
                    Name = name;
                    this.email = email;
                    this.password = encryptedPass;
                    connection.Close();
                    loginCommandReader.Close();
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("UserLoadingError"));
                    connection.Close();
                    return false;
                }

                return true;
            }
        }

        private bool CheckForUniqueEmail(string connectionString, string email)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        $"SELECT * " +
                        $"FROM USERS " +
                        $"WHERE USERS.EMAIL = '{email}'";
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

        private void InsertUser(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        "INSERT INTO USERS VALUES" +
                        $"('{nickname}', '{surname}', '{name}', '{email}', '{Password}')";

                    SqlDataReader commandReader = command.ExecuteReader();
                    commandReader.Close();
                }
                catch (Exception ex)
                {
                    CustomMessage.Show((string)Application.Current.FindResource("UserInsertError"));
                    CustomMessage.Show(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void ChangeUserData(string nickname, string surname, string name, string password, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string encryptedPass = DataEncriptor.Encrypt(password);

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText =
                        $"UPDATE USERS " +
                        $"SET NICKNAME = '{nickname}', SURNAME = '{surname}', NAME = '{name}', PASS = '{encryptedPass}' FROM USERS " +
                        $"WHERE EMAIL = '{this.Email}'";
                    SqlDataReader commandReader = command.ExecuteReader();
                    commandReader.Close();

                    Nickname = nickname;
                    Surname = surname;
                    Name = name;
                    this.password = encryptedPass;
                    commandReader.Close();
                }
                catch
                {
                    CustomMessage.Show((string)Application.Current.FindResource("UserUpdateError"));
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
