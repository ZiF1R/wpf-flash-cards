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
        public static void ValidateInput(string value, bool IsSpecialFormat = false, bool IsRequiredField = true)
        {
            if (IsRequiredField)
            {
                if (value.Length == 0)
                    throw new Exception((string)Application.Current.FindResource("EmptyString"));
            }

            if (value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if ((IsRequiredField == false && value.Length > 0) || IsRequiredField)
            {
                if (IsSpecialFormat)
                {
                    if (Regex.IsMatch(value, @"[^\w\d-_]"))
                        throw new FormatException();
                }
                else
                {
                    if (Regex.IsMatch(value, @"[_\d\W]"))
                        throw new FormatException();
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
                Regex.IsMatch(value, @"^((.+)\@([^a-zA-Z\d]+))\.") ||
                Regex.IsMatch(value, @"^((.+)\@(.+)\.([^a-zA-Z]+))$"))
                throw new FormatException((string)Application.Current.FindResource("EmailFormatError"));
        }

        public static void ValidatePassword(string value)
        {
            if (value.Length == 0)
                throw new Exception((string)Application.Current.FindResource("EmptyString"));

            if (value.Contains(" "))
                throw new Exception((string)Application.Current.FindResource("TextBoxError"));

            if (!Regex.IsMatch(value, @"([\w\d-_]){6,}"))
                throw new FormatException((string)Application.Current.FindResource("ErrorPasswordPattern"));
        }
    }
}
