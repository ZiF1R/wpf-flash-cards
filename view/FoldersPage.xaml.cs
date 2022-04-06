using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using course_project1.controls;
using course_project1.controls.ModalWindows;

namespace course_project1.view
{
    /// <summary>
    /// Логика взаимодействия для FoldersPage.xaml
    /// </summary>
    public partial class FoldersPage : Page
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        Frame rootFrame = mainWindow.MainFrame;
        Grid MainPageGrid;

        public FoldersPage(Grid mainPageGrid)
        {
            MainPageGrid = mainPageGrid;
            InitializeComponent();

            (string name, string category, int cards, int memorized)[] folders =
            {
                ("TestFolder", "none", 10, 4),
                ("TestFolder1", "none", 14, 2),
                ("TestFolder2", "test category", 5, 3),
                ("TestFolder3", "none", 23, 14)
            };


            /// show loader while render folders code

            foreach(var folder in folders)
            {
                FoldersWrap.Children.Add(this.CreateFolder(folder.name, folder.category, folder.cards, folder.memorized));
            }

            /// remove loader, show rendered folders code
        }

        private FolderControl CreateFolder(string name, string category, int cardsCount, int memorizedCards)
        {
            FolderControl folderControl = new FolderControl(MainPageGrid, name, category, cardsCount, memorizedCards);

            folderControl.Margin = new Thickness(0, 0, 40, 40);
            folderControl.RemoveFolder += (object s, RoutedEventArgs ev) => FoldersWrap.Children.Remove(folderControl);

            return folderControl;
        }

        private void AddFolderButton_AddFolder(object sender, RoutedEventArgs e)
        {
            FolderModalWindow modal = new FolderModalWindow(MainPageGrid, "", "");
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(FolderModalWindow.ModalHeaderProperty, "CreateFolder");
            modal.SetResourceReference(FolderModalWindow.ActionButtonContentProperty, "Create");

            MainPageGrid.Children.Add(modal);
            modal.FolderAction += (object s, RoutedEventArgs ev) =>
                FoldersWrap.Children.Insert(1, this.CreateFolder(modal.FolderName, modal.FolderCategory, 0, 0));
            modal.CloseFolderModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
        }
    }
}
