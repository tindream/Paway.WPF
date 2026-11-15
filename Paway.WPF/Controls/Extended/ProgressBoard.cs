using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// Progress仪表盘
    /// </summary>
    public partial class ProgressBoard : RangeBase
    {
        #region 动画进度
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AnimationValueProperty =
            DependencyProperty.RegisterAttached(nameof(AnimationValue), typeof(double), typeof(ProgressBoard), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAnimationValueChanged));
        /// <summary>
        /// 动画进度值
        /// <para>默认值：0</para>
        /// </summary>
        [Category("扩展")]
        [Description("动画进度值")]
        public double AnimationValue
        {
            get { return (double)GetValue(AnimationValueProperty); }
            set { SetValue(AnimationValueProperty, value); }
        }
        private static void OnAnimationValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressBoard board)
            {
                var animTime = PMethod.AnimTime((double)e.NewValue - board.Value) * 1.5;
                var animValue = new DoubleAnimation((double)e.NewValue, new Duration(TimeSpan.FromMilliseconds(animTime)))
                {
                    //AccelerationRatio = 0,
                    //DecelerationRatio = 1,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
                };
                board.BeginAnimation(ProgressBoard.ValueProperty, animValue);
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressBoard()
        {
            DefaultStyleKey = typeof(ProgressBoard);
            this.ValueChanged += ProgressBoard_ValueChanged;
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
            ProgressDragHelper.Down(this, true, e, ref startPoint);
            base.OnPreviewMouseLeftButtonDown(e);
        }
        /// <summary>
        /// 移动位置
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            ProgressDragHelper.Move(this, e, ref startPoint);
            base.OnPreviewMouseMove(e);
        }
        /// <summary>
        /// 抬起停止拖动
        /// </summary>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ProgressDragHelper.Up(this, e, ref startPoint);
            base.OnPreviewMouseLeftButtonUp(e);
        }

        #endregion

        #region 指针角度
        /// <summary>
        /// 指针Path
        /// </summary>
        private Path path_Pointer;
        private const double angleMin = -27.0;
        private const double angleMax = 207.0;
        private void ProgressBoard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (path_Pointer != null && path_Pointer.RenderTransform is TransformGroup transform)
            {
                if (transform.Children.Count > 0 && transform.Children[0] is RotateTransform rotate)
                {
                    var percent = (this.Value - this.Minimum) / (this.Maximum - this.Minimum);
                    rotate.Angle = angleMin + (angleMax - angleMin) * percent;
                }
            }
        }
        /// <summary>
        /// 重置指针大小与位置
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            //锁定长宽比
            this.Height = this.ActualWidth;
            base.OnRenderSizeChanged(sizeInfo);
            if (this.Child(out this.path_Pointer, "Path_Pointer"))
            {
                path_Pointer.Width = this.ActualWidth / 2 - 7 + 1;
                path_Pointer.Margin = new Thickness(0, 0, path_Pointer.Width, 0);
                ProgressBoard_ValueChanged(this, null);
            }
        }

        #endregion

        #region 鼠标滚轮滚动设置
        /// <summary>
        /// 滚动设置
        /// </summary>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            var interval = e.Delta / 120 * (this.Maximum - this.Minimum) / 30;
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) interval /= 3;
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) interval *= 3;
            var temp = this.Value + interval;
            var count = (temp / this.SmallChange).ToInt();
            temp = (count * this.SmallChange).Clear();
            if (temp > this.Maximum) temp = this.Maximum;
            else if (temp < this.Minimum) temp = this.Minimum;
            if (this.Value != temp) this.Value = temp;
            e.Handled = true;
            base.OnMouseWheel(e);
        }

        #endregion
    }
}
