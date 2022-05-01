using course_project1.controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace course_project1
{
    public static class Validator
    {
        public static void ValidateInput(CustomTextBox textBox, bool IsSpecialFormat = false, bool IsRequiredField = true)
        {
            if (textBox == null)
                throw new ArgumentNullException((string)Application.Current.FindResource("TextBoxNotExist"));

            if (IsRequiredField)
            {
                if (textBox.Value.Length == 0)
                    throw new Exception((string)Application.Current.FindResource("EmptyString"));
            }

            if (textBox.Value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if ((IsRequiredField == false && textBox.Value.Length > 0) || IsRequiredField)
            {
                if (IsSpecialFormat)
                {
                    if (Regex.IsMatch(textBox.Value, @"[^\w\d-_]"))
                        throw new FormatException();
                }
                else
                {
                    if (Regex.IsMatch(textBox.Value, @"[_\d\W]"))
                        throw new FormatException();
                }
            }
        }

        public static void ValidateInput(SecondaryTextInput textBox, bool IsSpecialFormat = false)
        {
            if (textBox == null)
                throw new ArgumentNullException((string)Application.Current.FindResource("TextBoxNotExist"));

            if (textBox.Placeholder.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (textBox.Placeholder.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if (IsSpecialFormat)
            {
                if (Regex.IsMatch(textBox.Placeholder, @"[^\w\d-_]"))
                    throw new FormatException();
            }
            else
            {
                if (Regex.IsMatch(textBox.Placeholder, @"[_\d\W]"))
                    throw new FormatException();
            }
        }

        public static void ValidateEmail(CustomTextBox textBox)
        {
            if (textBox == null)
                throw new ArgumentNullException((string)Application.Current.FindResource("TextBoxNotExist"));

            if (textBox.Value.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (textBox.Value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if (!Regex.IsMatch(textBox.Value, @"^(([a-zA-Z\d-_]+)\@([a-zA-Z\d]+)\.([a-zA-Z]){2,})$") ||
                Regex.IsMatch(textBox.Value, @"^(([^a-zA-Z\d-_]+)\@)") ||
                Regex.IsMatch(textBox.Value, @"^((.+)\@([^a-zA-Z\d]+))\.") ||
                Regex.IsMatch(textBox.Value, @"^((.+)\@(.+)\.([^a-zA-Z]+))$"))
                throw new FormatException((string)Application.Current.FindResource("EmailFormatError"));
        }

        public static void ValidatePassword(CustomPasswordBox passwordBox)
        {
            if (passwordBox == null)
                throw new ArgumentNullException((string)Application.Current.FindResource("TextBoxNotExist"));

            if (passwordBox.Value.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (passwordBox.Value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if (!Regex.IsMatch(passwordBox.Value, @"([\w\d-_]){6,}"))
                throw new FormatException((string)Application.Current.FindResource("ErrorPasswordPattern"));
        }

        public static void ValidatePassword(SecondaryTextInput passwordBox)
        {
            if (passwordBox == null)
                throw new ArgumentNullException((string)Application.Current.FindResource("TextBoxNotExist"));

            if (passwordBox.Placeholder.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (passwordBox.Placeholder.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if (!Regex.IsMatch(passwordBox.Placeholder, @"([\w\d-_]){6,}"))
                throw new FormatException((string)Application.Current.FindResource("ErrorPasswordPattern"));
        }
    }
}
