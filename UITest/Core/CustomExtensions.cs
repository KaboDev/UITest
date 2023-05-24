using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Core
{
    public static class CustomExtensions
    {
        public static bool IsParsePossible(string value, Type type)
        {
            try
            {
                Convert.ChangeType(value, type);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T TryParseValue<T>(string input)
        {
            T value;

            try
            {
                value = (T)Convert.ChangeType(input, typeof(T));
            }
            catch
            {
                value = default(T);
            }

            return value;
        }

        internal static T SearchElement<T>(List<Element> list, string name, Elements elementType = Elements.Input, T defaultValue = default(T))
        {
            object element = list.FirstOrDefault(x => x.Label == name).CurrentElement;

            T elementReturn = default(T);
            switch (elementType)
            {
                case Elements.Input:
                    string input = (element as InputElement).Input;
                    elementReturn = GetParsedValue<T>(input, defaultValue);
                    break;
                case Elements.Selection:
                    elementReturn = (T)(object)(element as SelectionElement).SelectedCategory;
                    break;
                case Elements.Image:
                    elementReturn = (T)(object)(element as ImageElement).ImagePath;
                    break;
            }

            return elementReturn;
        }

        //Returns parsed value => when parsed value is default value and custom defaultValue is passed as param => use given defaultValue
        public static T GetParsedValue<T>(string input, T defaultValue = default(T))
        {
            T value = CustomExtensions.TryParseValue<T>(input);
            //string needs extra condition, because default value is null
            return value.Equals(default(T)) ? defaultValue.Equals(default(T)) ? value : defaultValue : (value.GetType() == typeof(string) && value.Equals("") ? defaultValue : value);
        }
    }
}
