using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ListBox扩展
    /// </summary>
    public partial class ListBoxEXT : ListBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ListBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ListBoxEXT),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(ListBoxEXT),
            new PropertyMetadata(new BrushEXT(PConfig.TextColor, PConfig.TextColor, Colors.White)));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 项颜色
        /// <para>默认值：默认</para>
        /// </summary>
        [Category("扩展")]
        [Description("项颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 项字体颜色
        /// <para>默认值：TextColor, TextColor, Cols.White</para>
        /// </summary>
        [Category("扩展")]
        [Description("项字体颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ListBoxEXT()
        {
            DefaultStyleKey = typeof(ListBoxEXT);
        }
        /// <summary>
        /// 滚动到指定比例位置
        /// </summary>
        public void ScrollViewer(double percent)
        {
            if (Template.FindName("Part_ScrollViewer", this) is ScrollViewerEXT scrollViewer)
            {
                var height = scrollViewer.ScrollableHeight * percent;
                var iHeight = height.ToInt();
                if (iHeight < height) iHeight += 1;
                scrollViewer.ScrollToVerticalOffset(iHeight);
            }
        }
    }
}
