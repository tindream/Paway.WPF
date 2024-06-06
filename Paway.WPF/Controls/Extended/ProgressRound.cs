using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        public static readonly DependencyProperty BackgroundAngleProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundAngle), typeof(int), typeof(ProgressRound), new PropertyMetadata(360, OnBackgroundAngleChanged));
        /// <summary>
        /// </summary>
        internal static readonly DependencyProperty BackgroundValueProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundValue), typeof(double), typeof(ProgressRound));
        private static void OnBackgroundAngleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressRound view)
            {
                view.BackgroundValue = -view.BackgroundAngle / 2.0;
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
        /// 背景圆环度
        /// <para>默认值：360</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景圆环度")]
        public int BackgroundAngle
        {
            get { return (int)GetValue(BackgroundAngleProperty); }
            set { SetValue(BackgroundAngleProperty, value); }
        }
        /// <summary>
        /// 背景圆环旋转角度(自动)
        /// <para>默认值：0</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景角度")]
        internal double BackgroundValue
        {
            get { return (double)GetValue(BackgroundValueProperty); }
            set { SetValue(BackgroundValueProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressRound()
        {
            DefaultStyleKey = typeof(ProgressRound);
        }
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
