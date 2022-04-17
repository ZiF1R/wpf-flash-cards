using course_project1.controls.ModalWindows;
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

namespace course_project1.controls
{
    /// <summary>
    /// Логика взаимодействия для CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {
        Grid MainPageGrid;

        public CardControl(
            Grid mainPageGrid,
            DateTime created,
            string term,
            string translation,
            string examples)
        {
            this.MainPageGrid = mainPageGrid;
            this.Created = created;
            this.Term = term;
            this.Translation = translation;
            this.Examples = examples;
            InitializeComponent();
        }

        ///
        
        public static readonly DependencyProperty TermProperty =
            DependencyProperty.Register(
               name: "Term",
               propertyType: typeof(string),
               ownerType: typeof(CardControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "default",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
               validateValueCallback: new ValidateValueCallback(IsValidString));

        public string Term
        {
            get => (string)GetValue(TermProperty);
            set => SetValue(TermProperty, value);
        }

        public static bool IsValidString(object value)
        {
            string val = (string)value;
            return val != "";
        }

        ///
        
        public static readonly DependencyProperty TranslationProperty =
            DependencyProperty.Register(
               name: "Translation",
               propertyType: typeof(string),
               ownerType: typeof(CardControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "default",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure),
               validateValueCallback: new ValidateValueCallback(IsValidString));

        public string Translation
        {
            get => (string)GetValue(TranslationProperty);
            set => SetValue(TranslationProperty, value);
        }

        ///
        
        public static readonly DependencyProperty ExamplesProperty =
            DependencyProperty.Register(
               name: "Examples",
               propertyType: typeof(string),
               ownerType: typeof(CardControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: "",
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public string Examples
        {
            get => (string)GetValue(ExamplesProperty);
            set => SetValue(ExamplesProperty, value);
        }

        ///

        public static readonly DependencyProperty CreatedProperty =
            DependencyProperty.Register(
               name: "Created",
               propertyType: typeof(DateTime),
               ownerType: typeof(CardControl),
               typeMetadata: new FrameworkPropertyMetadata(
                   defaultValue: DateTime.Now,
                   flags: FrameworkPropertyMetadataOptions.AffectsMeasure));

        public DateTime Created
        {
            get => (DateTime)GetValue(CreatedProperty);
            set => SetValue(CreatedProperty, value);
        }

        public static readonly RoutedEvent EditCardEvent
            = EventManager.RegisterRoutedEvent("EditCard", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardControl));

        public event RoutedEventHandler EditCard
        {
            add { AddHandler(EditCardEvent, value); }
            remove { RemoveHandler(EditCardEvent, value); }
        }

        private void EditCardButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            myPopup.IsOpen = false;
            RaiseEvent(new RoutedEventArgs(EditCardEvent));
        }

        public static readonly RoutedEvent RemoveCardEvent
            = EventManager.RegisterRoutedEvent("RemoveCard", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CardControl));

        public event RoutedEventHandler RemoveCard
        {
            add { AddHandler(RemoveCardEvent, value); }
            remove { RemoveHandler(RemoveCardEvent, value); }
        }

        private void RemoveCardButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            myPopup.IsOpen = false;

            SimpleModalWindow modal = new SimpleModalWindow();
            modal.SetValue(Grid.RowSpanProperty, 2);
            modal.SetValue(Grid.ColumnSpanProperty, 3);
            modal.SetResourceReference(SimpleModalWindow.ModalContentProperty, "RemoveCard");

            MainPageGrid.Children.Add(modal);
            modal.CloseModal += (object s, RoutedEventArgs ev) => MainPageGrid.Children.Remove(modal);
            modal.NegativeButtonClick += (object s, RoutedEventArgs ev) => RaiseEvent(new RoutedEventArgs(RemoveCardEvent));
        }

        private void CopyAllButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string cardInfo = $"{Term} / {Translation}";
            if (Examples != "") cardInfo += $" / {Examples}";
            Clipboard.SetText(cardInfo);
            myPopup.IsOpen = false;
        }

        private void CopyTermButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Term);
            myPopup.IsOpen = false;
        }

        private void CopyTranslationButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Translation);
            myPopup.IsOpen = false;
        }

        private void CopyExamplesButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(Examples);
            myPopup.IsOpen = false;
        }
    }
}
