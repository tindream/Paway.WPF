using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        /// 指针Path
        /// </summary>
        private Path path_Pointer;
        private const double angleMin = -27.0;
        private const double angleMax = 207.0;
        /// <summary>
        /// </summary>
        public ProgressBoard()
        {
            DefaultStyleKey = typeof(ProgressBoard);
            this.ValueChanged += ProgressBoard_ValueChanged;
        }
        private void ProgressBoard_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (path_Pointer != null && path_Pointer.RenderTransform is TransformGroup transform)
            {
                if (transform.Children.Count > 0 && transform.Children[0] is RotateTransform rotate)
                {
                    var percent = this.Minimum + (this.Maximum - this.Minimum) * Value / 100.0;
                    rotate.Angle = angleMin + (angleMax - angleMin) * percent / 100.0;
                }
            }
        }
        /// <summary>
        /// 重置指针大小与位置
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (PMethod.Child(this, out this.path_Pointer, "Path_Pointer"))
            {
                path_Pointer.Width = this.ActualWidth / 2 - 7 + 1;
                path_Pointer.Margin = new Thickness(0, 0, path_Pointer.Width, 0);
            }
        }
    }
}
