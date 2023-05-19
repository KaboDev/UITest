using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest.Core
{
    public static class TypeParseExtension
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
    }
}
