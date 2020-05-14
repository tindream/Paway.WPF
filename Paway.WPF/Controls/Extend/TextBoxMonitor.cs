using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paway.WPF
{
    public class TextBoxMonitor : DependencyObject
    {
        #region 启用监听，设置水印大小
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(TextBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBox txt)
            {
                txt.LayoutUpdated += delegate
                {
                    txt.SetValue(WaterSizeProperty, txt.FontSize * 0.85);
                };
            }
        }
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached("WaterSize", typeof(double), typeof(TextBoxMonitor), new PropertyMetadata(15d));
    }
}
