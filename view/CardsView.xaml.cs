using course_project1.controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace course_project1.view
{
    /// <summary>
    /// Логика взаимодействия для CardsView.xaml
    /// </summary>
    public partial class CardsView : Page
    {
        static MainWindow mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
        Frame SecondFrame;
        FolderControl rootFolder;
        Grid MainPageGrid;

        public CardsView(Grid mainPageGrid, Frame secondFrame, FolderControl folder)
        {
            SecondFrame = secondFrame;
            rootFolder = folder;
            MainPageGrid = mainPageGrid;
            InitializeComponent();
            FolderName.Text = rootFolder.FolderName;
        }

        private void AddCardButton_AddCard(object sender, RoutedEventArgs e)
        {

        }

        private void ReturnToFolders_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SecondFrame.Content = new FoldersPage(MainPageGrid, SecondFrame);
        }
    }
}
