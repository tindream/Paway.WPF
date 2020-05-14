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
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(TextBoxRound), new PropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty BorderFocusedBrushProperty =
            DependencyProperty.RegisterAttached("BorderFocusedBrush", typeof(Brush), typeof(TextBoxRound), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 35, 175, 255))));

        [Category("扩展")]
        [Description("水印内容")]
        public string Water
        {
            get { return (string)GetValue(WaterProperty); }
            set { SetValue(WaterProperty, value); }
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
        public Brush BorderFocusedBrush
        {
            get { return (Brush)GetValue(BorderFocusedBrushProperty); }
            set { SetValue(BorderFocusedBrushProperty, value); }
        }

        public TextBoxRound() { }
    }
}
