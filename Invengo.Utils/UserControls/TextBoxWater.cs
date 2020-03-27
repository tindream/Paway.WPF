using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Invengo.Utils
{
    public partial class TextBoxWater : TextBox
    {
        public static readonly DependencyProperty WaterProperty =
            DependencyProperty.RegisterAttached("Water", typeof(string), typeof(TextBoxWater), new PropertyMetadata("请输入.."));
        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached("WaterSize", typeof(double), typeof(TextBoxWater), new PropertyMetadata(16d));
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(TextBoxWater), new PropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty FocusedBrushProperty =
            DependencyProperty.RegisterAttached("FocusedBrush", typeof(Brush), typeof(TextBoxWater), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(35, 175, 255))));

        [Category("扩展")]
        public string Water
        {
            get { return (string)GetValue(WaterProperty); }
            set { SetValue(WaterProperty, value); }
        }
        [Category("扩展")]
        public double WaterSize
        {
            get { return (double)GetValue(WaterSizeProperty); }
            set { SetValue(WaterSizeProperty, value); }
        }
        [Category("扩展")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        [Category("扩展")]
        public Brush FocusedBrush
        {
            get { return (Brush)GetValue(FocusedBrushProperty); }
            set { SetValue(FocusedBrushProperty, value); }
        }

        public TextBoxWater() { }
    }
}
