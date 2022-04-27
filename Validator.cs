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
                throw new ArgumentNullException("Received textbox is not exist!");

            textBox.Value = textBox.Value.Trim().Replace(" ", "");

            if (IsRequiredField)
            {
                if (textBox.Value.Length == 0)
                    throw new Exception((string)Application.Current.FindResource("EmptyString"));
            }

            if ((IsRequiredField == false && textBox.Value.Length > 0) || IsRequiredField)
            {
                if (IsSpecialFormat)
                {
                    if (Regex.IsMatch(textBox.Value, @"[^\w\d-_]"))
                        throw new Exception((string)Application.Current.FindResource("ErrorSpecialPattern"));
                }
                else
                {
                    if (Regex.IsMatch(textBox.Value, @"[\d\W]"))
                        throw new Exception((string)Application.Current.FindResource("ErrorPattern"));
                }
            }
        }

        public static void ValidateInput(SecondaryTextInput textBox, bool IsSpecialFormat = false)
        {
            if (textBox == null)
                throw new ArgumentNullException("Received textbox is not exist!");

            textBox.Placeholder = textBox.Placeholder.Trim().Replace(" ", "");

            if (textBox.Placeholder.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (IsSpecialFormat)
            {
                if (Regex.IsMatch(textBox.Placeholder, @"[^\w\d-_]"))
                    throw new Exception((string)Application.Current.FindResource("ErrorSpecialPattern"));
            }
            else
            {
                if (Regex.IsMatch(textBox.Placeholder, @"[\d\W]"))
                    throw new Exception((string)Application.Current.FindResource("ErrorPattern"));
            }
        }

        public static void ValidateEmail(CustomTextBox textBox)
        {
            if (textBox == null)
                throw new ArgumentNullException("Received textbox is not exist!");

            textBox.Value = textBox.Value.Trim().Replace(" ", "");

            if (textBox.Value.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (!Regex.IsMatch(textBox.Value, @"([\w\d-_]+)\@([\w\d]+)\.(\w){2,}"))
                throw new Exception((string)Application.Current.FindResource("EmailFormatError"));
        }

        public static void ValidatePassword(CustomPasswordBox passwordBox)
        {
            if (passwordBox == null)
                throw new ArgumentNullException("Received textbox is not exist!");

            //passwordBox.Value = passwordBox.Value.Trim().Replace(" ", "");
            if (passwordBox.Value.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (passwordBox.Value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("PasswordError"));

            if (!Regex.IsMatch(passwordBox.Value, @"([\w\d-_]){6,}"))
                throw new Exception((string)Application.Current.FindResource("ErrorPasswordPattern"));
        }

        public static void ValidatePassword(SecondaryTextInput passwordBox)
        {
            if (passwordBox == null)
                throw new ArgumentNullException("Received textbox is not exist!");

            if (passwordBox.Placeholder.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (passwordBox.Placeholder.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("PasswordError"));

            if (!Regex.IsMatch(passwordBox.Placeholder, @"([\w\d-_]){6,}"))
                throw new Exception((string)Application.Current.FindResource("ErrorPasswordPattern"));
        }
    }
}
