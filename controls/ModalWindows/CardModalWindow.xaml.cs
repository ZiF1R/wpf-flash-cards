using course_project1.storage;
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

namespace course_project1.controls.ModalWindows
{
    /// <summary>
    /// Логика взаимодействия для CardModalWindow.xaml
    /// </summary>
    public partial class CardModalWindow : UserControl
    {
        string ConnectionString;
        public int RootFolderId;
        public string Term = "";
        public string Translation = "";
        public string Examples = "";

        public CardModalWindow(string connectionString, int folderId, string term, string translation, string examples)
        {
            RootFolderId = folderId;
            ConnectionString = connectionString;
            Term = term;
            Translation = translation;
            Examples = examples;
            InitializeComponent();

            CardTermTextBox.Value = Term;
            CardTranslationTextBox.Value = Translation;
            CardExamplesTextBox.Value = Examples;
        }

        public static readonly DependencyProperty ModalHeaderProperty =
           DependencyProperty.Register(
              name: "ModalHeader",
              propertyType: typeof(string),
              ownerType: typeof(CardModalWindow),
              typeMetadata: new FrameworkPropertyMetadata(
                  defaultValue: "ModalWindow Header",
                  flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string ModalHeader
        {
            get => (string)GetValue(ModalHeaderProperty);
            set => SetValue(ModalHeaderProperty, value);
        }

        public static readonly DependencyProperty ActionButtonContentProperty =
            DependencyProperty.Register(
               name: "ActionButtonContent",
               propertyType: typeof(string),
               ownerType: typeof(CardModalWindow),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "Action",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string ActionButtonContent
        {
            get => (string)GetValue(ActionButtonContentProperty);
            set => SetValue(ActionButtonContentProperty, value);
        }

        public static readonly RoutedEvent CloseCardModalEvent
           = EventManager.RegisterRoutedEvent("CloseCardModal", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardModalWindow));

        public event RoutedEventHandler CloseCardModal
        {
            add { AddHandler(CloseCardModalEvent, value); }
            remove { RemoveHandler(CloseCardModalEvent, value); }
        }

        public static readonly RoutedEvent CardActionEvent
            = EventManager.RegisterRoutedEvent("CardAction", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardModalWindow));

        public event RoutedEventHandler CardAction
        {
            add { AddHandler(CardActionEvent, value); }
            remove { RemoveHandler(CardActionEvent, value); }
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateInput(CardTermTextBox, "CardTermFormatError", true);
                ValidateInput(CardTranslationTextBox, "CardTranslationFormatError", true);
                ValidateInput(CardExamplesTextBox, "CardExamplesFormatError", true, false);

                if (CardTermTextBox.Value != Term)
                {
                    bool isUnique = Card.IsUniqueCardTerm(ConnectionString, RootFolderId, CardTermTextBox.Value);
                    if (!isUnique)
                    {
                        CardTermUsed.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        CardTermUsed.Visibility = Visibility.Hidden;
                    }
                }
                this.Term = CardTermTextBox.Value;
                this.Translation = CardTranslationTextBox.Value;
                this.Examples = CardExamplesTextBox.Value;

                RaiseEvent(new RoutedEventArgs(CardActionEvent));
                RaiseEvent(new RoutedEventArgs(CloseCardModalEvent));
            }
            catch (Exception ex)
            {
                CustomMessage.Show(ex.Message);
                return;
            }
        }

        private void ValidateInput(CustomTextBox textBox, string errorFormatMessageResourceName, bool specialFormat = false, bool requiredField = true)
        {
            try
            {
                Validator.ValidateInput(textBox, specialFormat, requiredField);
            }
            catch (FormatException ex)
            {
                throw new FormatException((string)Application.Current.FindResource(errorFormatMessageResourceName));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Modal_Close(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseCardModalEvent));
        }

        private void Overlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseCardModalEvent));
        }
    }
}
