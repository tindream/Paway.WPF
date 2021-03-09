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
    /// ScrollViewer扩展
    /// </summary>
    public partial class ScrollViewerEXT : ScrollViewer
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ScrollBarRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarRadius), typeof(CornerRadius), typeof(ScrollViewerEXT), new PropertyMetadata(new CornerRadius(4)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ScrollBarColorProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarColor), typeof(Brush), typeof(ScrollViewerEXT), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(200, 125, 125, 125))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ScrollBarWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarWidth), typeof(double), typeof(ScrollViewerEXT), new PropertyMetadata(8d));

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
        public ScrollViewerEXT() { }
        /// <summary>
        /// 滚动
        /// </summary>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = this
            };
            this.RaiseEvent(eventArg);
            base.OnPreviewMouseWheel(e);
        }
        /// <summary>
        /// 自动垂直滚动
        /// </summary>
        /// <param name="time">滚动耗时(s)</param>
        /// <param name="beginTime">开始延时(默认2s)</param>
        /// <param name="forever">无限重复(默认true)</param>
        public void AutoScroll(double time, double beginTime = 2, bool forever = true)
        {
            ScrollViewerBehavior.AutoScroll(this, time, beginTime, forever);
        }
    }
}
