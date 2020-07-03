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
    /// 主题样式
    /// </summary>
    public class ThemeMonitor : DependencyObject
    {
        #region 主题样式监听
        /// <summary>
        /// 主题样式监听
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(ThemeMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        /// <summary>
        /// 启用监听
        /// </summary>
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (obj is Control ctrl)
                {
                    ctrl.Loaded += delegate
                    {
                        if (ctrl.FontSize == new Control().FontSize) ctrl.SetValue(Control.FontSizeProperty, ctrl.GetValue(FontSizeProperty));
                    };
                }
            }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// 字体大小
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached(nameof(FontSize), typeof(double), typeof(ThemeMonitor), new PropertyMetadata(Config.FontSize));

        #endregion

        #region 扩展
        /// <summary>
        /// 字体大小
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        #endregion
    }
}
