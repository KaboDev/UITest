using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UITest.Model;

namespace UITest.Converter
{
    class MultiVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility desiredVisibility = Visibility.Collapsed;
            if (values[0] is int count)
            {
                desiredVisibility = count > 0 ? Visibility.Visible : Visibility.Collapsed;
                if (values.Length > 1 && values[1] != null && desiredVisibility == Visibility.Visible)
                {
                    Type valueType = values[1].GetType();
                    desiredVisibility = valueType == typeof(Storage) || valueType == typeof(StoragePlace) ? Visibility.Visible : desiredVisibility;
                }
            }
            return desiredVisibility;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
