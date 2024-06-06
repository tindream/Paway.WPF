using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Paway.WPF
{
    /// <summary>
    /// ProgressBar扩展-圆形
    /// </summary>
    public partial class ProgressRound : ProgressBarBase
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundColorLinear), typeof(ColorLinear), typeof(ProgressRound),
                new PropertyMetadata(new ColorLinear(Colors.LightGray, PMethod.ThemeColor())));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IForwardProperty =
            DependencyProperty.RegisterAttached(nameof(IForward), typeof(bool), typeof(ProgressRound), new PropertyMetadata(true));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IDragProperty =
            DependencyProperty.RegisterAttached(nameof(IDrag), typeof(bool), typeof(ProgressRound));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.RegisterAttached(nameof(Angle), typeof(double), typeof(ProgressRound), new PropertyMetadata(360d, OnBackgroundAngleChanged));
        /// <summary>
        /// </summary>
        internal static readonly DependencyProperty AngleTransformProperty =
            DependencyProperty.RegisterAttached(nameof(AngleTransform), typeof(double), typeof(ProgressRound));
        /// <summary>
        /// </summary>
        internal static readonly DependencyProperty AngleRateProperty =
            DependencyProperty.RegisterAttached(nameof(AngleRate), typeof(double), typeof(ProgressRound), new PropertyMetadata(1d));
        private static void OnBackgroundAngleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressRound view)
            {
                var iForward = view.IForward ? 1 : -1;
                view.AngleTransform = iForward * -view.Angle / 2;
                view.AngleRate = view.Angle / 360;
            }
        }

        #endregion

        #region 扩展
        /// <summary>
        /// 进度条线性颜色
        /// <para>默认值：LightGray, 主题色</para>
        /// </summary>
        [Category("扩展")]
        [Description("进度条线性颜色")]
        public ColorLinear ForegroundColorLinear
        {
            get { return (ColorLinear)GetValue(ForegroundColorLinearProperty); }
            set { SetValue(ForegroundColorLinearProperty, value); }
        }
        /// <summary>
        /// 圆环的方向
        /// <para>默认值：True（正向，顺时针）</para>
        /// </summary>
        [Category("扩展")]
        [Description("圆环的方向")]
        public bool IForward
        {
            get { return (bool)GetValue(IForwardProperty); }
            set { SetValue(IForwardProperty, value); }
        }
        /// <summary>
        /// 允许鼠标拖动设置值
        /// <para>默认值：False</para>
        /// </summary>
        [Category("扩展")]
        [Description("允许鼠标拖动设置值")]
        public bool IDrag
        {
            get { return (bool)GetValue(IDragProperty); }
            set { SetValue(IDragProperty, value); }
        }
        /// <summary>
        /// 圆环度
        /// <para>默认值：360</para>
        /// </summary>
        [Category("扩展")]
        [Description("圆环度")]
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }
        /// <summary>
        /// 圆环旋转角度(内部自动)
        /// <para>默认值：0</para>
        /// </summary>
        internal double AngleTransform
        {
            get { return (double)GetValue(AngleTransformProperty); }
            set { SetValue(AngleTransformProperty, value); }
        }
        /// <summary>
        /// 圆环刻度值系数(内部自动)
        /// <para>默认值：1</para>
        /// </summary>
        internal double AngleRate
        {
            get { return (double)GetValue(AngleRateProperty); }
            set { SetValue(AngleRateProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressRound()
        {
            DefaultStyleKey = typeof(ProgressRound);
        }

        #region 拖动设置
        /// <summary>
        /// 拖动起始点
        /// </summary>
        private Point? startPoint;
        /// <summary>
        /// 按下开始拖动
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IDrag && e.ButtonState == MouseButtonState.Pressed && e.ClickCount == 1)
            {
                if (PMethod.Parent(this, out Window window)) window.Cursor = Cursors.Hand;
                this.startPoint = e.GetPosition(this);
                //尝试将鼠标强制捕获到控件
                CaptureMouse();
                e.Handled = true;
            }
            base.OnPreviewMouseLeftButtonDown(e);
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
            var x = Math.Pow(endPoint.X - startPoint.Value.X, 2.0);
            if (endPoint.X < startPoint.Value.X) x *= -1;
            var y = Math.Pow(endPoint.Y - startPoint.Value.Y, 2.0);
            if (endPoint.Y > startPoint.Value.Y) y *= -1;
            var interval = Math.Sqrt(Math.Abs(x + y));
            if (x + y < 0) interval *= -1;
            var percent = interval * 0.35 / this.ActualWidth;
            var temp = this.Value + (this.Maximum - this.Minimum) * percent;
            var count = (temp / this.SmallChange).ToInt();
            temp = count * this.SmallChange;
            if (temp > this.Maximum) temp = this.Maximum;
            else if (temp < this.Minimum) temp = this.Minimum;
            if (this.Value != temp) this.Value = temp;
        }

        #endregion

        #region 鼠标滚轮滚动设置
        /// <summary>
        /// 滚动设置
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (IDrag)
            {
                var interval = e.Delta / 120 * (this.Maximum - this.Minimum) / 30;
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) interval /= 3;
                else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) interval *= 3;
                var temp = this.Value + interval;
                var count = (temp / this.SmallChange).ToInt();
                temp = count * this.SmallChange;
                if (temp > this.Maximum) temp = this.Maximum;
                else if (temp < this.Minimum) temp = this.Minimum;
                if (this.Value != temp) this.Value = temp;
                e.Handled = true;
            }
            base.OnMouseWheel(e);
        }

        #endregion

        /// <summary>
        /// 锁定长宽比
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            this.Height = this.ActualWidth;
            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}
