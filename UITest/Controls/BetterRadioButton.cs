using FontAwesome.Sharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UITest.Controls
{
    class BetterRadioButton : RadioButton
    {
        public static readonly DependencyProperty DefaultBorderProperty =
            DependencyProperty.Register("DefaultBorder", typeof(SolidColorBrush), typeof(BetterRadioButton), new PropertyMetadata());
        public static readonly DependencyProperty BorderHoverProperty =
            DependencyProperty.Register("BorderHover", typeof(SolidColorBrush), typeof(BetterRadioButton), new PropertyMetadata());
        public static readonly DependencyProperty BorderPressProperty =
            DependencyProperty.Register("BorderPress", typeof(SolidColorBrush), typeof(BetterRadioButton), new PropertyMetadata());
        public static readonly DependencyProperty ForegroundHoverProperty =
            DependencyProperty.Register("ForegroundHover", typeof(SolidColorBrush), typeof(BetterRadioButton), new PropertyMetadata());
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(IconChar), typeof(BetterRadioButton), new PropertyMetadata());

        public SolidColorBrush DefaultBorder
        {
            get
            {
                return (SolidColorBrush)GetValue(DefaultBorderProperty);
            }
            set
            {
                SetValue(DefaultBorderProperty, value);
            }
        }

        public SolidColorBrush BorderHover
        {
            get
            {
                return (SolidColorBrush)GetValue(BorderHoverProperty);
            }
            set
            {
                SetValue(BorderHoverProperty, value);
            }
        }

        public SolidColorBrush BorderPress
        {
            get
            {
                return (SolidColorBrush)GetValue(BorderPressProperty);
            }
            set
            {
                SetValue(BorderPressProperty, value);
            }
        }

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
