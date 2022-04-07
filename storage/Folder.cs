using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Folder(string name, string category)
        {
            Name = name;
            Category = category;
            Created = DateTime.Now;
            Cards = new Card[] { };

            // **add folder to database**
        }

        public void ChangeFolderData(string name, string category)
        {
            Name = name;
            Category = category;

            // **apply changes to database**
        }
    }
}
