using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using course_project1.storage;

namespace course_project1.view
{
    /// <summary>
    /// Логика взаимодействия для FoldersPage.xaml
    /// </summary>
    public partial class FoldersPage : Page
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        SqlConnection CurrentConnection = mainWindow.CurrentConnection;
        DataStorage Storage = mainWindow.Storage;
        Frame rootFrame = mainWindow.MainFrame;
        Frame SecondFrame;
        Grid MainPageGrid;

        public FoldersPage(Grid mainPageGrid, Frame secondFrame)
        {
            MainPageGrid = mainPageGrid;
            SecondFrame = secondFrame;
            InitializeComponent();

            foreach (Folder folder in Storage.folders)
                CreateFolderElement(folder);
        }

        private FolderControl CreateFolderElement(Folder folder)
        {
            FolderControl folderControl = new FolderControl(
                MainPageGrid, folder.Name, folder.Category, folder.Cards.Length, folder.MemorizedCardsCount());

            folderControl.Margin = new Thickness(0, 0, 40, 40);
            folderControl.EditFolder += (object s, RoutedEventArgs ev) =>
            {
                FolderModalWindow modal = new FolderModalWindow(
                    MainPageGrid, CurrentConnection, Storage.user.Uid, folderControl.FolderNameField.Text,folderControl.FolderCategoryField.Text);
                modal.SetValue(Grid.RowSpanProperty, 2);
                modal.SetValue(Grid.ColumnSpanProperty, 3);
                modal.SetResourceReference(FolderModalWindow.ModalHeaderProperty, "EditFolder");
                modal.SetResourceReference(FolderModalWindow.ActionButtonContentProperty, "Edit");
                MainPageGrid.Children.Add(modal);

                modal.FolderAction += (object se, RoutedEventArgs evn) =>
                {
                    folder.ChangeFolderData(CurrentConnection, Storage.user.Uid, modal.FolderName, modal.FolderCategory);

                    folderControl.FolderNameField.Text = modal.FolderName;
                    folderControl.FolderCategoryField.Text = modal.FolderCategory;
                };
                modal.CloseFolderModal += (object se, RoutedEventArgs evn) => MainPageGrid.Children.Remove(modal);
            };
            folderControl.RemoveFolder += (object s, RoutedEventArgs ev) =>
            {
                folder.RemoveFolder(CurrentConnection, Storage.user.Uid);
                Storage.folders = Storage.folders.Where(x => x.Name != folderControl.FolderName).ToArray();
                FoldersWrap.Children.Remove(folderControl);
            };
            folderControl.GoToCards += (object s, RoutedEventArgs ev) => SecondFrame.Content = new CardsView(MainPageGrid, SecondFrame, folderControl);

            return folderControl;
        }

        private void AddFolderButton_AddFolder(object sender, RoutedEventArgs e)
        {
            FolderModalWindow modal = new FolderModalWindow(MainPageGrid, CurrentConnection, Storage.user.Uid, "", "");
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(FolderModalWindow.ModalHeaderProperty, "CreateFolder");
            modal.SetResourceReference(FolderModalWindow.ActionButtonContentProperty, "Create");

            MainPageGrid.Children.Add(modal);

            modal.FolderAction += (object s, RoutedEventArgs ev) =>
            {
                string folderName = modal.FolderName;
                string folderCategory = modal.FolderCategory;

                Folder folder = new Folder(CurrentConnection, Storage.user.Uid, folderName, folderCategory);
                Storage.folders = Storage.folders.Append(folder).ToArray();
                FoldersWrap.Children.Insert(1, this.CreateFolderElement(folder));
            };
            modal.CloseFolderModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
        }
    }
}
