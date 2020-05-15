﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paway.WPF
{
    public class ThemeMonitor : DependencyObject
    {
        #region 主题样式监听
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(ThemeMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (obj is Button btn)
                {
                    btn.Loaded += delegate
                    {
                        if (btn.FontSize == new Control().FontSize) btn.SetValue(Control.FontSizeProperty, btn.GetValue(ButtonFontSizeProperty));
                    };
                }
                else if (obj is Control ctrl)
                {
                    ctrl.Loaded += delegate
                    {
                        if (ctrl.FontSize == new Control().FontSize) ctrl.SetValue(Control.FontSizeProperty, ctrl.GetValue(FontSizeProperty));
                    };
                }
            }
        }

        #endregion

        /// <summary>
        /// 字体大小
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached("FontSize", typeof(double), typeof(ThemeMonitor), new PropertyMetadata(15d));
        /// <summary>
        /// 按钮字体大小
        /// </summary>
        public static readonly DependencyProperty ButtonFontSizeProperty =
            DependencyProperty.RegisterAttached("ButtonFontSize", typeof(double), typeof(ThemeMonitor), new PropertyMetadata(18d));
    }
}