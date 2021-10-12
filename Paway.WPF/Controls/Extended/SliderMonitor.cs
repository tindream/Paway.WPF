using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// Slider扩展监听
    /// </summary>
    internal class SliderMonitor : DependencyObject
    {
        #region 启用监听，获取Slider属性并设置到Thumb
        /// <summary>
        /// 启用监听，获取ScrollViewer属性并设置到Thumb
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(SliderMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Thumb thumb)
            {
                if (thumb.TemplatedParent is SliderEXT sliderEXT)
                {
                    thumb.SetValue(ButtonTypeProperty, sliderEXT.ButtonType);
                    return;
                }
                thumb.SetValue(ButtonTypeProperty, new SliderEXT().ButtonType);
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

        #region 依赖属性
        /// <summary>
        /// 按钮类型
        /// </summary>
        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.RegisterAttached(nameof(ButtonType), typeof(ButtonType), typeof(SliderMonitor));
        /// <summary>
        /// 按钮类型
        /// </summary>
        public ButtonType ButtonType
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        #endregion
    }
}
