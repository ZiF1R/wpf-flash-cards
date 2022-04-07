using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace course_project1.storage
{
    public class User
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        SqlConnection CurrentConnection = mainWindow.CurrentConnection;

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
            Nickname = "";
            Surname = "";
            Name = "";
            email = "";
            Password = "";
        }

        public User(string nickname, string surname, string name, string email, string password)
        {
            // **check for unique email**
            //CurrentConnection

            Nickname = nickname;
            Surname = surname;
            Name = name;
            this.email = email;
            Password = password;
        }

        public User LoadUser(string email, string password)
        {
            // **load data**

            string nickname = "";
            string surname = "";
            string name = "";

            Nickname = nickname;
            Surname = surname;
            Name = name;
            this.email = email;
            Password = password;

            return this;
        }

        public void ChangeUserData(string nickname, string surname, string name)
        {
            Nickname = nickname;
            Surname = surname;
            Name = name;

            // **apply changes to database**
        }
    }
}
