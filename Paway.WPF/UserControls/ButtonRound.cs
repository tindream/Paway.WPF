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
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(Brush), typeof(ButtonRound),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(170, 35, 175, 255))));
        public static readonly DependencyProperty FocusedBrushDownProperty =
            DependencyProperty.RegisterAttached("FocusedBrushDown", typeof(Brush), typeof(ButtonRound),
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
        public Brush FocusedBrush
        {
            get { return (Brush)GetValue(FocusedBrushProperty); }
            set { SetValue(FocusedBrushProperty, value); }
        }
        [Category("扩展")]
        [Description("鼠标点击时的背景颜色")]
        public Brush FocusedBrushDown
        {
            get { return (Brush)GetValue(FocusedBrushDownProperty); }
            set { SetValue(FocusedBrushDownProperty, value); }
        }

        public ButtonRound() { }
    }
}
