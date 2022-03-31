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
        Frame rootFrame;
        Grid MainPageGrid;

        public FoldersPage(Frame frame, Grid mainPageGrid)
        {
            rootFrame = frame;
            MainPageGrid = mainPageGrid;
            InitializeComponent();

            (string name, string category, int cards, int memorized)[] folders =
            {
                ("TestFolder", null, 10, 4),
                ("TestFolder1", null, 14, 2),
                ("TestFolder2", null, 5, 3),
                ("TestFolder3", null, 23, 14)
            };


            /// show loader while render folders code

            foreach(var folder in folders)
            {
                FolderControl folderControl = new FolderControl(
                    mainPageGrid,
                    folder.name,
                    folder.category,
                    folder.cards,
                    folder.memorized);

                folderControl.Margin = new Thickness(0, 0, 40, 40);
                folderControl.RemoveFolder += (object s, RoutedEventArgs ev) => FoldersWrap.Children.Remove(folderControl);
                FoldersWrap.Children.Add(folderControl);
            }

            /// remove loader, show rendered folders code
        }

        private void AddFolderButton_AddFolder(object sender, RoutedEventArgs e)
        {
            SimpleModalWindow modal = new SimpleModalWindow();
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(SimpleModalWindow.ModalContentProperty, "RemoveFolder");

            MainPageGrid.Children.Add(modal);
            modal.CloseModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
        }
    }
}
