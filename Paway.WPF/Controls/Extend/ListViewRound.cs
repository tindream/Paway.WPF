using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class ListViewRound : ComboBox
    {
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(ListViewRound), new PropertyMetadata(150d));
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(ListViewRound), new PropertyMetadata(42d));
        public static readonly DependencyProperty ItemRadiusProperty =
            DependencyProperty.RegisterAttached("ItemRadius", typeof(CornerRadius), typeof(ListViewRound), new PropertyMetadata(new CornerRadius(5)));
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.RegisterAttached("ItemMargin", typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(1)));
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached("ItemBorder", typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(1)));
        public static readonly DependencyProperty ItemImageWidthProperty =
            DependencyProperty.RegisterAttached("ItemImageWidth", typeof(double), typeof(ListViewRound), new PropertyMetadata(24d));
        public static readonly DependencyProperty ItemImageHeightProperty =
            DependencyProperty.RegisterAttached("ItemImageHeight", typeof(double), typeof(ListViewRound), new PropertyMetadata(24d));

        [Category("扩展")]
        [Description("自定义项宽度")]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义项高度")]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义项圆角")]
        public CornerRadius ItemRadius
        {
            get { return (CornerRadius)GetValue(ItemRadiusProperty); }
            set { SetValue(ItemRadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义项外边距")]
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义项外边框")]
        public Thickness ItemBorder
        {
            get { return (Thickness)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义项图片宽度")]
        public double ItemImageWidth
        {
            get { return (double)GetValue(ItemImageWidthProperty); }
            set { SetValue(ItemImageWidthProperty, value); }
        }
        [Category("扩展")]
        [Description("自定义项图片高度")]
        public double ItemImageHeight
        {
            get { return (double)GetValue(ItemImageHeightProperty); }
            set { SetValue(ItemImageHeightProperty, value); }
        }

        public ListViewRound() { }
    }
}
