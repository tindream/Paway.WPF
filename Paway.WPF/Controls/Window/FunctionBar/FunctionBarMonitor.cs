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
    public class FunctionBarMonitor : DependencyObject
    {
        #region 启用监听，设置按钮宽度
        /// <summary>
        /// 启用监听，设置按钮宽度
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(FunctionBarMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is MenuItem item)
            {
                item.Loaded += delegate
                {
                    item.SetValue(BarWidthProperty, item.ActualWidth - 2);
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

        /// <summary>
        /// 按钮宽度
        /// </summary>
        public static readonly DependencyProperty BarWidthProperty =
            DependencyProperty.RegisterAttached(nameof(BarWidth), typeof(double), typeof(FunctionBarMonitor), new PropertyMetadata());
        /// <summary>
        /// 按钮宽度
        /// </summary>
        public double BarWidth
        {
            get { return (double)GetValue(BarWidthProperty); }
            set { SetValue(BarWidthProperty, value); }
        }
    }
}
