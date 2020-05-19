using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ScrollViewer扩展
    /// </summary>
    public partial class ScrollViewerRound : ScrollViewer
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ScrollBarRadiusProperty =
            DependencyProperty.RegisterAttached("ScrollBarRadius", typeof(CornerRadius), typeof(ScrollViewerRound), new PropertyMetadata(new CornerRadius(4)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ScrollBarColorProperty =
            DependencyProperty.RegisterAttached("ScrollBarColor", typeof(Brush), typeof(ScrollViewerRound), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(200, 125, 125, 125))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ScrollBarWidthProperty =
            DependencyProperty.RegisterAttached("ScrollBarWidth", typeof(double), typeof(ScrollViewerRound), new PropertyMetadata(8d));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius ScrollBarRadius
        {
            get { return (CornerRadius)GetValue(ScrollBarRadiusProperty); }
            set { SetValue(ScrollBarRadiusProperty, value); }
        }
        /// <summary>
        /// 滚动条颜色
        /// </summary>
        [Category("扩展")]
        [Description("滚动条颜色")]
        public Brush ScrollBarColor
        {
            get { return (Brush)GetValue(ScrollBarColorProperty); }
            set { SetValue(ScrollBarColorProperty, value); }
        }
        /// <summary>
        /// 滚动条高度(宽度)
        /// </summary>
        [Category("扩展")]
        [Description("滚动条高度(宽度)")]
        public double ScrollBarWidth
        {
            get { return (double)GetValue(ScrollBarWidthProperty); }
            set { SetValue(ScrollBarWidthProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ScrollViewerRound() { }
    }
}
