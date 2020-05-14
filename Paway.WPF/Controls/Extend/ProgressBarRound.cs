using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class ProgressBarRound : ProgressBar
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(ProgressBarRound), new PropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty ForegroundStartColorProperty =
            DependencyProperty.RegisterAttached("ForegroundStartColor", typeof(Color), typeof(ProgressBarRound),
            new PropertyMetadata(Color.FromArgb(85, 35, 175, 255)));
        public static readonly DependencyProperty ForegroundEndColorProperty =
            DependencyProperty.RegisterAttached("ForegroundEndColor", typeof(Color), typeof(ProgressBarRound),
            new PropertyMetadata(Color.FromArgb(250, 35, 175, 255)));
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.RegisterAttached("ProgressValue", typeof(string), typeof(ProgressBarRound));

        #region 启用监听，获取进度
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(ProgressBarRound), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressBar bar)
            {
                bar.Loaded += delegate
                {
                    bar.SetValue(ProgressValueProperty, $"{bar.Value * 100 / bar.Maximum:F0}%");
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
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        [Category("扩展")]
        [Description("进度条起始颜色")]
        public Color ForegroundStartColor
        {
            get { return (Color)GetValue(ForegroundStartColorProperty); }
            set { SetValue(ForegroundStartColorProperty, value); }
        }
        [Category("扩展")]
        [Description("进度条终点颜色")]
        public Color ForegroundEndColor
        {
            get { return (Color)GetValue(ForegroundEndColorProperty); }
            set { SetValue(ForegroundEndColorProperty, value); }
        }

        public ProgressBarRound() { }
    }
}
