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
        private static readonly List<Control> ControlList = new List<Control>();

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
                        if (ctrl.FontSize == new Control().FontSize)
                        {
                            if (!ControlList.Contains(ctrl)) ControlList.Add(ctrl);
                            ctrl.SetValue(Control.FontSizeProperty, Config.FontSize);
                        }
                    };
                }
            }
        }

        #endregion

        /// <summary>
        /// 监听主题字体更新
        /// </summary>
        static ThemeMonitor()
        {
            Config.FontSizeChanged += Config_FontSizeChanged;
        }
        /// <summary>
        /// 更新字体大小
        /// </summary>
        private static void Config_FontSizeChanged(double old)
        {
            for (int i = 0; i < ControlList.Count; i++)
            {
                ControlList[i].SetValue(Control.FontSizeProperty, Config.FontSize);
            }
        }
    }
}
