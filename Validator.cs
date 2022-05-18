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
        public static void ValidateInput(string value, string formatErrorMessageKey, bool IsSpecialFormat = false, bool IsRequiredField = true)
        {
            if (IsRequiredField)
            {
                if (value.Length == 0)
                    throw new Exception((string)Application.Current.FindResource("EmptyString"));
            }

            if (Regex.IsMatch(value, @".+\s{2,}.+"))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if ((IsRequiredField == false && value.Length > 0) || IsRequiredField)
            {
                if (IsSpecialFormat)
                {
                    if (Regex.IsMatch(value, @"[^a-zA-Zа-яА-Я\d-_$=+*^%#,;<>:\.!?\s]"))
                        throw new FormatException((string)Application.Current.FindResource(formatErrorMessageKey));
                }
                else
                {
                    if (Regex.IsMatch(value, @"[^a-zA-Zа-яА-Я]"))
                        throw new FormatException((string)Application.Current.FindResource(formatErrorMessageKey));
                }
            }
        }

        public static void ValidateEmail(string value)
        {
            if (value.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if (!Regex.IsMatch(value, @"^(([a-zA-Z\d-_]+)\@([a-zA-Z\d]+)\.([a-zA-Z]){2,})$") ||
                Regex.IsMatch(value, @"^(([^a-zA-Z\d-_]+)\@)") ||
                Regex.IsMatch(value, @"^((.+)\@([^a-zA-Z]+))\.") ||
                Regex.IsMatch(value, @"^((.+)\@(.+)\.([^a-zA-Z]+))$"))
                throw new FormatException((string)Application.Current.FindResource("EmailFormatError"));
        }

        public static void ValidatePassword(string value)
        {
            if (value.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("PasswordError"));

            if (!Regex.IsMatch(value, @"([a-zA-Zа-яА-Я\d-_$=+*^%#,;:\.!?]){6,}"))
                throw new FormatException((string)Application.Current.FindResource("ErrorPasswordPattern"));
        }
    }
}
