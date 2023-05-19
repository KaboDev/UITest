using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace UITest.Controls
{
    class BetterButton : Button
    {
        public static readonly DependencyProperty DefaultForegroundProperty =
            DependencyProperty.Register("DefaultForeground", typeof(SolidColorBrush), typeof(BetterButton), new PropertyMetadata(new BrushConverter().ConvertFrom("#717277")));
        public static readonly DependencyProperty DefaultBorderProperty =
            DependencyProperty.Register("DefaultBorder", typeof(SolidColorBrush), typeof(BetterButton), new PropertyMetadata());
        public static readonly DependencyProperty ForegroundHoverProperty =
            DependencyProperty.Register("ForegroundHover", typeof(SolidColorBrush), typeof(BetterButton), new PropertyMetadata());
        public static readonly DependencyProperty ForegroundPressProperty =
            DependencyProperty.Register("ForegroundPress", typeof(SolidColorBrush), typeof(BetterButton), new PropertyMetadata());
        public static readonly DependencyProperty BorderHoverProperty =
            DependencyProperty.Register("BorderHover", typeof(SolidColorBrush), typeof(BetterButton), new PropertyMetadata());
        public static readonly DependencyProperty BorderPressProperty =
            DependencyProperty.Register("BorderPress", typeof(SolidColorBrush), typeof(BetterButton), new PropertyMetadata());
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(BetterButton), new PropertyMetadata());
        public static readonly DependencyProperty BorderEffectProperty =
            DependencyProperty.Register("BorderEffect", typeof(Effect), typeof(BetterButton), new PropertyMetadata());

        public SolidColorBrush DefaultForeground
        {
            get
            {
                return (SolidColorBrush)GetValue(DefaultForegroundProperty);
            }
            set
            {
                SetValue(DefaultForegroundProperty, value);
            }
        }

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

        public SolidColorBrush ForegroundPress
        {
            get
            {
                return (SolidColorBrush)GetValue(ForegroundPressProperty);
            }
            set
            {
                SetValue(ForegroundPressProperty, value);
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

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
        public Effect BorderEffect
        {
            get
            {
                return (Effect)GetValue(BorderEffectProperty);
            }
            set
            {
                SetValue(BorderEffectProperty, value);
            }
        }
    }
}
