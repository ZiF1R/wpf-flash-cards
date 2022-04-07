using course_project1.storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void RemoveFolder(string name, DateTime created)
        {
            folders = folders.Where(x => x.Name != name && x.Created != created).ToArray();

            // **apply changes to database**
        }

        public void LoadFolders()
        {
            // **load user folders**
        }

        public void LoadCategories()
        {
            // **load user categories**
        }
    }
}
