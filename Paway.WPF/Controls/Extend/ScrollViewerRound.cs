using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class ScrollViewerRound : ScrollViewer
    {
        public static readonly DependencyProperty ScrollBarRadiusProperty = 
            DependencyProperty.RegisterAttached("ScrollBarRadius", typeof(CornerRadius), typeof(ScrollViewerRound), new PropertyMetadata(new CornerRadius(4)));
        public static readonly DependencyProperty ScrollBarColorProperty = 
            DependencyProperty.RegisterAttached("ScrollBarColor", typeof(Brush), typeof(ScrollViewerRound), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(200, 125, 125, 125))));
        public static readonly DependencyProperty ScrollBarWidthProperty = 
            DependencyProperty.RegisterAttached("ScrollBarWidth", typeof(double), typeof(ScrollViewerRound), new PropertyMetadata(8d));

        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius ScrollBarRadius
        {
            get { return (CornerRadius)GetValue(ScrollBarRadiusProperty); }
            set { SetValue(ScrollBarRadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("滚动条颜色")]
        public Brush ScrollBarColor
        {
            get { return (Brush)GetValue(ScrollBarColorProperty); }
            set { SetValue(ScrollBarColorProperty, value); }
        }
        [Category("扩展")]
        [Description("滚动条高/宽度")]
        public double ScrollBarWidth
        {
            get { return (double)GetValue(ScrollBarWidthProperty); }
            set { SetValue(ScrollBarWidthProperty, value); }
        }

        public ScrollViewerRound() { }
    }
}
