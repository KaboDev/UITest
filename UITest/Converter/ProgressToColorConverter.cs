using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace UITest.Converter
{
    class ProgressToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            double progress = (double)value * 0.01f;

            Color startColor = (Color)ColorConverter.ConvertFromString("#7fff7f");
            Color endColor = (Color)ColorConverter.ConvertFromString("#b13e3e");

            Color color = LerpColor(startColor, endColor, progress);

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Color LerpColor(Color startColor, Color endColor, double t)
        {
            t = Math.Max(0, Math.Min(1, t)); // Clamp t between 0 and 1

            byte r = (byte)((1 - t) * startColor.R + t * endColor.R);
            byte g = (byte)((1 - t) * startColor.G + t * endColor.G);
            byte b = (byte)((1 - t) * startColor.B + t * endColor.B);
            byte a = (byte)((1 - t) * startColor.A + t * endColor.A);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
