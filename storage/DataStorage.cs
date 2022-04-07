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
        public User user = new User();
        public Settings settings = new Settings();
        public Folder[] folders = new Folder[] { };

        public void RemoveFolder(string name, DateTime created)
        {
            folders = folders.Where(x => x.Name != name && x.Created != created).ToArray();

            // **apply changes to database**
        }
    }
}
