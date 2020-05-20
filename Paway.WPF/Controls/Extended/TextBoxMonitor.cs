using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paway.WPF
{
    /// <summary>
    /// TextBox扩展监听
    /// </summary>
    public class TextBoxMonitor : DependencyObject
    {
        #region 启用监听，设置水印大小
        /// <summary>
        /// 启用监听，设置水印大小
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(TextBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is TextBoxRound txt)
            {
                txt.Loaded += delegate
                {
                    if (txt.WaterSize == 0) txt.SetValue(TextBoxRound.WaterSizeProperty, txt.FontSize * 0.85);
                };
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
    }
}
