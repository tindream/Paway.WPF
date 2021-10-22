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
    /// ProgressBar扩展
    /// </summary>
    public partial class ProgressBarEXT : ProgressBar
    {
        #region 启用监听，获取进度
        /// <summary>
        /// 启用监听，获取进度
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(ProgressBarEXT), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressBarEXT bar)
            {
                bar.LayoutUpdated += delegate
                {
                    var value = $"{bar.Value * 100 / bar.Maximum:F0}%";
                    if (bar.ProgressValue != value) bar.SetValue(ProgressValueProperty, value);
                };
                if ((bool)e.NewValue)
                {
                    bar.ValueChanged += Bar_ValueChanged;
                }
                else
                {
                    bar.ValueChanged -= Bar_ValueChanged;
                }
            }
        }
        private static void Bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is ProgressBar bar)
            {
                bar.SetValue(ProgressValueProperty, $"{bar.Value * 100 / bar.Maximum:F0}%");
            }
        }
        /// <summary>
        /// 启用监听
        /// </summary>
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        #region 动画进度
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AnimationValueProperty =
            DependencyProperty.RegisterAttached(nameof(AnimationValue), typeof(double), typeof(ProgressBarEXT), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAnimationValueChanged));
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
            if (obj is ProgressBarEXT bar)
            {
                var animTime = PMethod.AnimTime((double)e.NewValue - bar.Value) * 1.5;
                var animValue = new DoubleAnimation((double)e.NewValue, new Duration(TimeSpan.FromMilliseconds(animTime)))
                {
                    //AccelerationRatio = 0,
                    //DecelerationRatio = 1,
                    EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut },
                };
                bar.BeginAnimation(ProgressBarEXT.ValueProperty, animValue);
            }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ProgressBarEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundColorLinear), typeof(ColorLinear), typeof(ProgressBarEXT),
                new PropertyMetadata(new ColorLinear(85, 240)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.RegisterAttached(nameof(ProgressValue), typeof(string), typeof(ProgressBarEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IRoundProperty =
            DependencyProperty.RegisterAttached(nameof(IRound), typeof(bool), typeof(ProgressBarEXT));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 进度条线性颜色
        /// </summary>
        [Category("扩展")]
        [Description("进度条线性颜色")]
        public ColorLinear ForegroundColorLinear
        {
            get { return (ColorLinear)GetValue(ForegroundColorLinearProperty); }
            set { SetValue(ForegroundColorLinearProperty, value); }
        }
        /// <summary>
        /// 进度条显示文本
        /// </summary>
        [Category("扩展")]
        [Description("进度条显示文本")]
        public string ProgressValue
        {
            get { return (string)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }
        /// <summary>
        /// 显示圆形
        /// </summary>
        [Category("扩展")]
        [Description("显示圆形")]
        public bool IRound
        {
            get { return (bool)GetValue(IRoundProperty); }
            set { SetValue(IRoundProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressBarEXT()
        {
            DefaultStyleKey = typeof(ProgressBarEXT);
        }
    }
}
