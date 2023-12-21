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
                UpdateText(bar);
                bar.ValueChanged -= Bar_ValueChanged;
                if ((bool)e.NewValue)
                {
                    bar.ValueChanged += Bar_ValueChanged;
                }
            }
        }
        private static void Bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is ProgressBarEXT bar) UpdateText(bar);
        }
        private static void UpdateText(ProgressBarEXT bar)
        {
            if (bar.Dot == -2)
            {
                var value = (bar.Maximum - bar.Minimum) / bar.SmallChange;
                while (value / 10 >= 0.5)
                {
                    bar.Dot++;
                    value /= 10;
                }
                if (bar.Dot < 0) bar.Dot = 0;
            }
            var text = $"{((bar.Value - bar.Minimum) * 100 / (bar.Maximum - bar.Minimum)).Rounds(bar.Dot)}%";
            if (bar.ProgressValue != text) bar.SetValue(ProgressValueProperty, text);
        }
        /// <summary>
        /// 启用监听
        /// </summary>
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }
        /// <summary>
        /// 小数位数
        /// </summary>
        internal int Dot { get; set; } = -2;

        #endregion

        #region 动画进度
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty AnimationValueProperty =
            DependencyProperty.RegisterAttached(nameof(AnimationValue), typeof(double), typeof(ProgressBarEXT), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAnimationValueChanged));
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
                new PropertyMetadata(new ColorLinear()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.RegisterAttached(nameof(ProgressValue), typeof(string), typeof(ProgressBarEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ITextProperty =
            DependencyProperty.RegisterAttached(nameof(IText), typeof(bool), typeof(ProgressBarEXT), new PropertyMetadata(true));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：3</para>
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
        /// <para>默认值：默认</para>
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
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("进度条显示文本")]
        public string ProgressValue
        {
            get { return (string)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }
        /// <summary>
        /// 显示进度值
        /// <para>默认值：true</para>
        /// </summary>
        [Category("扩展")]
        [Description("显示进度值")]
        public bool IText
        {
            get { return (bool)GetValue(ITextProperty); }
            set { SetValue(ITextProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressBarEXT()
        {
            DefaultStyleKey = typeof(ProgressBarEXT);
        }
        /// <summary>
        /// 呈现时更新进度文本
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateText(this);
        }
    }
}
