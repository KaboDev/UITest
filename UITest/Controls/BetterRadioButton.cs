using FontAwesome.Sharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UITest.Controls
{
    class BetterRadioButton : RadioButton
    {
        public static readonly DependencyProperty ForegroundHoverProperty =
            DependencyProperty.Register("ForegroundHover", typeof(SolidColorBrush), typeof(BetterRadioButton), new PropertyMetadata());
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconChar), typeof(BetterRadioButton), new PropertyMetadata());

        public SolidColorBrush ForegroundHover
        {
            get
            {
                return (SolidColorBrush)GetValue(ForegroundHoverProperty);
            }
            set
            {
                SetValue(ForegroundHoverProperty, value);
            }
        }

        public IconChar Icon
        {
            get
            {
                return (IconChar)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }
    }
}
