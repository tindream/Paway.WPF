using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class ComboBoxMonitor : DependencyObject
    {
        #region 启用监听，设置项属性
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(ComboBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ComboBoxItem item)
            {
                if (item.Parent is ComboBoxRound cbx)
                {
                    item.SetValue(SelectedBackgroundProperty, cbx.SelectedBackground);
                    item.SetValue(SelectedForegroundProperty, cbx.SelectedForeground);
                    item.SetValue(MouseOverBackgroundProperty, cbx.MouseOverBackground);
                    item.SetValue(MouseOverForegroundProperty, cbx.MouseOverForeground);
                }
            }
        }
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.RegisterAttached("SelectedBackground", typeof(Brush), typeof(ComboBoxMonitor));
        public static readonly DependencyProperty SelectedForegroundProperty =
            DependencyProperty.RegisterAttached("SelectedForeground", typeof(Brush), typeof(ComboBoxMonitor));
        public static readonly DependencyProperty MouseOverBackgroundProperty =
           DependencyProperty.RegisterAttached("MouseOverBackground", typeof(Brush), typeof(ComboBoxMonitor));
        public static readonly DependencyProperty MouseOverForegroundProperty =
           DependencyProperty.RegisterAttached("MouseOverForeground", typeof(Brush), typeof(ComboBoxMonitor));
        [Category("扩展")]
        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }
        [Category("扩展")]
        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }
        [Category("扩展")]
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        [Category("扩展")]
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public ComboBoxMonitor() { }
    }
}
