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
    /// Progress扩展监听
    /// </summary>
    internal class ProgressMonitor : DependencyObject
    {
        #region 启用监听，设置水印大小
        /// <summary>
        /// 启用监听，设置水印大小
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(ProgressMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Progress progress)
            {
                //LayoutUpdated 在设计器设计可观察效果，但运行过程中一直触发
                progress.Loaded += delegate
                {
                    var width = 20d;
                    var height = 20d;
                    //if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(progress) == false)
                    {
                        width = double.IsNaN(progress.Width) == false ? progress.Width : progress.ActualWidth;
                        height = double.IsNaN(progress.Height) == false ? progress.Height : progress.ActualHeight;
                    }
                    progress.TemplateSettings = new ProgressTemplate(Math.Min(width, height));
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
