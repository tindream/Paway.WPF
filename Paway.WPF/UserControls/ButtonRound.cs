using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class ButtonRound : Button
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(ButtonRound), new PropertyMetadata(new CornerRadius(7)));
        public static readonly DependencyProperty MouseBackgroundProperty =
            DependencyProperty.RegisterAttached("MouseBackground", typeof(Brush), typeof(ButtonRound),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(170, 35, 175, 255))));
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.RegisterAttached("PressedBackground", typeof(Brush), typeof(ButtonRound),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(230, 35, 175, 255))));

        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标划过时的背景颜色")]
        public Brush MouseBackground
        {
            get { return (Brush)GetValue(MouseBackgroundProperty); }
            set { SetValue(MouseBackgroundProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标点击时的背景颜色")]
        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public ButtonRound() { }
    }
}
