using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paway.WPF
{
    public class PasswordBoxMonitor : DependencyObject
    {
        #region 启用监听，设置水印大小，获取密码长度
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PasswordBox pad)
            {
                pad.LayoutUpdated += delegate
                {
                    pad.SetValue(WaterSizeProperty, pad.FontSize * 0.85);
                };
                if ((bool)e.NewValue)
                {
                    pad.PasswordChanged += PasswordChanged;
                }
                else
                {
                    pad.PasswordChanged -= PasswordChanged;
                }
            }
        }
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox pad)
            {
                pad.SetValue(PasswordLengthProperty, pad.Password.Length);
            }
        }
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        /// <summary>
        /// 自动获取当前密码框文本长度
        /// </summary>
        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor));
        /// <summary>
        /// 水印字体大小
        /// </summary>
        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached("WaterSize", typeof(double), typeof(PasswordBoxMonitor), new PropertyMetadata(15d));
    }
}
