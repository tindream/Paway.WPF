using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class TextBoxRound : TextBox
    {
        public static readonly DependencyProperty WaterProperty =
            DependencyProperty.RegisterAttached("Water", typeof(string), typeof(TextBoxRound), new PropertyMetadata("请输入.."));
        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached("WaterSize", typeof(double), typeof(TextBoxRound), new PropertyMetadata(16d));
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(TextBoxRound), new PropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(Brush), typeof(TextBoxRound), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(35, 175, 255))));

        [Category("扩展")]
        [Description("水印内容")]
        public string Water
        {
            get { return (string)GetValue(WaterProperty); }
            set { SetValue(WaterProperty, value); }
        }
        [Category("扩展")]
        [Description("水印内容字体大小")]
        public double WaterSize
        {
            get { return (double)GetValue(WaterSizeProperty); }
            set { SetValue(WaterSizeProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("文本框激活时的边框颜色")]
        public Brush FocusedBrush
        {
            get { return (Brush)GetValue(FocusedBrushProperty); }
            set { SetValue(FocusedBrushProperty, value); }
        }

        public TextBoxRound() { }
    }
}
