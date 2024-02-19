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
        public static readonly DependencyProperty ScrollBarColorProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarColor), typeof(Brush), typeof(ScrollViewerEXT), new PropertyMetadata(Colors.Black.ToBrush()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ScrollIntervalProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollInterval), typeof(double), typeof(ScrollViewerEXT));
        /// <summary>
        /// 未应用：动画不可设置参数
        /// </summary>
        public static readonly DependencyProperty ScrollBarWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarWidth), typeof(double), typeof(ScrollViewerEXT), new PropertyMetadata(8d));
        /// <summary>
        /// 未应用：动画不可设置参数
        /// </summary>
        public static readonly DependencyProperty ScrollBarRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarRadius), typeof(CornerRadius), typeof(ScrollViewerEXT), new PropertyMetadata(new CornerRadius(4)));
        #endregion

        #region 扩展
        /// <summary>
        /// 滚动条颜色
        /// <para>默认值：Black</para>
        /// </summary>
        [Category("扩展")]
        [Description("滚动条颜色")]
        public Brush ScrollBarColor
        {
            get { return (Brush)GetValue(ScrollBarColorProperty); }
            set { SetValue(ScrollBarColorProperty, value); }
        }
        /// <summary>
        /// 滚动间隔
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("滚动间隔")]
        public double ScrollInterval
        {
            get { return (double)GetValue(ScrollIntervalProperty); }
            set { SetValue(ScrollIntervalProperty, value); }
        }
        /// <summary>
        /// 滚动条高度(宽度)
        /// <para>未应用：动画不可设置参数</para>
        /// <para>默认值：8</para>
        /// </summary>
        [Category("扩展")]
        [Description("滚动条高度(宽度)(未应用：动画不可设置参数)")]
        public double ScrollBarWidth
        {
            get { return (double)GetValue(ScrollBarWidthProperty); }
            set { SetValue(ScrollBarWidthProperty, value); }
        }
        /// <summary>
        /// 自定义边框圆角
        /// <para>未应用：动画不可设置参数</para>
        /// <para>默认值：4</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角(未应用：动画不可设置参数)")]
        public CornerRadius ScrollBarRadius
        {
            get { return (CornerRadius)GetValue(ScrollBarRadiusProperty); }
            set { SetValue(ScrollBarRadiusProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ScrollViewerEXT() { }

        #region 拖动滚动
        private Point? startPoint;
        /// <summary>
        /// 按下开始拖动
        /// </summary>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 1)
            {
                if (this.ScrollableHeight > 0 || this.ScrollableWidth > 0)
                {
                    if (PMethod.Parent(this, out Window window)) window.Cursor = Cursors.Hand;
                    this.startPoint = e.GetPosition(this);
                    //尝试将鼠标强制捕获到控件
                    CaptureMouse();
                    e.Handled = true;
                }
            }
            base.OnMouseLeftButtonDown(e);
        }
        /// <summary>
        /// 移动位置
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && startPoint != null)
            {
                var movePoint = e.GetPosition(this);
                Calc(movePoint);
                this.startPoint = movePoint;
            }
            base.OnPreviewMouseMove(e);
        }
        /// <summary>
        /// 抬起停止拖动
        /// </summary>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (startPoint != null)
            {
                Calc(e.GetPosition(this));
                this.startPoint = null;
                //当控件具有鼠标捕获的话，则释放该捕获。
                ReleaseMouseCapture();
                if (PMethod.Parent(this, out Window window)) window.Cursor = null;
            }
            base.OnPreviewMouseLeftButtonUp(e);
        }
        /// <summary>
        /// 计算移动距离
        /// </summary>
        private void Calc(Point endPoint)
        {
            var verticalOffset = endPoint.Y - startPoint.Value.Y;
            var horizontalOffset = endPoint.X - startPoint.Value.X;
            if (verticalOffset != 0 && this.ScrollableHeight > 0 && this.VerticalScrollBarVisibility != ScrollBarVisibility.Hidden)
            {
                this.ScrollToVerticalOffset(this.VerticalOffset - verticalOffset);
            }
            if (horizontalOffset != 0 && this.ScrollableWidth > 0 && this.HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden)
            {
                this.ScrollToHorizontalOffset(this.HorizontalOffset - horizontalOffset);
            }
        }

        #endregion
        /// <summary>
        /// 滚动响应
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (this.ScrollableHeight > 0 && this.VerticalScrollBarVisibility != ScrollBarVisibility.Hidden)
            {
                if (ScrollInterval != 0) this.ScrollToVerticalOffset(this.VerticalOffset - e.Delta / 120 * ScrollInterval);
            }
            else if (this.ScrollableWidth > 0 && this.HorizontalScrollBarVisibility != ScrollBarVisibility.Hidden)
            {
                if (ScrollInterval != 0) this.ScrollToHorizontalOffset(this.HorizontalOffset - e.Delta / 120 * 50);
                else if (e.Delta > 0) this.LineLeft();
                else this.LineRight();
            }
            base.OnMouseWheel(e);
        }
        /// <summary>
        /// 自动垂直滚动
        /// </summary>
        /// <param name="time">滚动耗时(s)</param>
        /// <param name="beginTime">开始延时(默认2s)</param>
        /// <param name="forever">无限重复(默认true)</param>
        public void AutoScroll(double time, double beginTime = 2, bool forever = true)
        {
            if (time < 0) time = 0;
            ScrollViewerBehavior.AutoScroll(this, time, beginTime, forever);
        }
    }
}
