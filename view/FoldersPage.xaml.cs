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
        DataStorage Storage;
        string ConnectionString;
        Frame SecondFrame;
        Grid MainPageGrid;
        bool IsForExportImport;

        FolderControl.Action action;

        public FoldersPage(
            Grid mainPageGrid, Frame secondFrame,
            string connectionString, DataStorage storage, bool isForExportImport = false,
            FolderControl.Action action = FolderControl.Action.None)
        {
            this.action = action;
            IsForExportImport = isForExportImport;
            ConnectionString = connectionString;
            Storage = storage;
            MainPageGrid = mainPageGrid;
            SecondFrame = secondFrame;
            InitializeComponent();

            foreach (Folder folder in Storage.folders)
                FoldersWrap.Children.Insert(1, this.CreateFolderElement(folder));
        }

        private FolderControl CreateFolderElement(Folder folder)
        {
            FolderControl folderControl = null;
            try
            {
                folderControl = new FolderControl(MainPageGrid, folder, Storage, ConnectionString, IsForExportImport, action);
                folderControl.Margin = new Thickness(0, 0, 40, 40);

                folderControl.EditFolder += (object s, RoutedEventArgs ev) =>
                {
                    FolderModalWindow modal = new FolderModalWindow(
                        MainPageGrid, ConnectionString, Storage, folderControl.FolderNameField.Text, folderControl.FolderCategoryField.Text);
                    modal.SetValue(Grid.RowSpanProperty, 2);
                    modal.SetValue(Grid.ColumnSpanProperty, 3);
                    modal.SetResourceReference(FolderModalWindow.ModalHeaderProperty, "EditFolder");
                    modal.SetResourceReference(FolderModalWindow.ActionButtonContentProperty, "Edit");
                    MainPageGrid.Children.Add(modal);

                    modal.FolderAction += (object se, RoutedEventArgs evn) =>
                    {
                        folder.ChangeFolderData(ConnectionString, Storage.user.Uid, modal.FolderName, modal.FolderCategory);

                        folderControl.FolderNameField.Text = modal.FolderName;
                        folderControl.FolderCategoryField.Text = modal.FolderCategory;
                    };
                    modal.CloseFolderModal += (object se, RoutedEventArgs evn) => MainPageGrid.Children.Remove(modal);
                };
                folderControl.RemoveFolder += (object s, RoutedEventArgs ev) =>
                {
                    bool isSuccessfully = folder.RemoveFolder(ConnectionString, Storage.user.Uid);
                    if (!isSuccessfully) return;

                    Storage.folders = Storage.folders.Where(x => x.Name != folderControl.FolderName).ToArray();
                    FoldersWrap.Children.Remove(folderControl);
                };
                folderControl.GoToCards += (object s, RoutedEventArgs ev) =>
                    SecondFrame.Content = new CardsView(MainPageGrid, SecondFrame, folder, ConnectionString, Storage);
                folderControl.ReturnToSettings += (object s, RoutedEventArgs ev) =>
                    SecondFrame.Content = new SettingsPage(MainPageGrid, ConnectionString, Storage, SecondFrame);
            }
            catch
            {
                CustomMessage.Show((string)Application.Current.FindResource("FolderCreateError"));
            }
            return folderControl;
        }

        private void AddFolderButton_AddFolder(object sender, RoutedEventArgs e)
        {
            FolderModalWindow modal = new FolderModalWindow(MainPageGrid, ConnectionString, Storage, "", "");
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(FolderModalWindow.ModalHeaderProperty, "CreateFolder");
            modal.SetResourceReference(FolderModalWindow.ActionButtonContentProperty, "Create");

            MainPageGrid.Children.Add(modal);
            modal.FolderAction += (object s, RoutedEventArgs ev) =>
            {
                string folderName = modal.FolderName;
                string folderCategory = modal.FolderCategory;

                try
                {
                    Folder folder = new Folder(ConnectionString, Storage.user.Uid, folderName, folderCategory);
                    Storage.folders = Storage.folders.Append(folder).ToArray();

                    if (folder.Name.Contains(SearchInput.Value))
                        FoldersWrap.Children.Insert(1, this.CreateFolderElement(folder));
                }
                catch { }
            };
            modal.CloseFolderModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
        }

        private void SearchInput_Input(object sender, RoutedEventArgs e)
        {
            SearchInput.Value = SearchInput.Value.Trim();
            FoldersWrap.Children.RemoveRange(1, FoldersWrap.Children.Count - 1);
            Folder[] folders = Storage.folders.Where(f => f.Name.Contains(SearchInput.Value)).ToArray();

            foreach (Folder folder in folders)
                FoldersWrap.Children.Insert(1, this.CreateFolderElement(folder));
        }
    }
}
