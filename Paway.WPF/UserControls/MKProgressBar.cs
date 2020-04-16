using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class MKProgressBar : ProgressBar
    {
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached("Radius", typeof(CornerRadius), typeof(MKProgressBar), new PropertyMetadata(new CornerRadius(3)));
        public static readonly DependencyProperty ForegroundStartColorProperty =
            DependencyProperty.RegisterAttached("ForegroundStartColor", typeof(Color), typeof(MKProgressBar),
            new PropertyMetadata(Color.FromArgb(255, 57, 143, 180)));
        public static readonly DependencyProperty ForegroundEndColorProperty =
            DependencyProperty.RegisterAttached("ForegroundEndColor", typeof(Color), typeof(MKProgressBar),
            new PropertyMetadata(Color.FromArgb(255, 106, 210, 216)));
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.RegisterAttached("ProgressValue", typeof(string), typeof(MKProgressBar));

        #region 启用监听，获取进度
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(MKProgressBar), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ProgressBar bar)
            {
                bar.LayoutUpdated += delegate
                {
                    bar.SetValue(ProgressValueProperty, bar.Value + "/" + bar.Maximum);
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
                bar.SetValue(ProgressValueProperty, bar.Value + "/" + bar.Maximum);
            }
        }
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        [Category("扩展")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        [Category("扩展")]
        public Color ForegroundStartColor
        {
            get { return (Color)GetValue(ForegroundStartColorProperty); }
            set { SetValue(ForegroundStartColorProperty, value); }
        }
        [Category("扩展")]
        public Color ForegroundEndColor
        {
            get { return (Color)GetValue(ForegroundEndColorProperty); }
            set { SetValue(ForegroundEndColorProperty, value); }
        }

        public MKProgressBar() { }
    }
}
