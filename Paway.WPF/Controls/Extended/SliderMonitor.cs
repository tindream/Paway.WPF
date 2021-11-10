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
        #region 启用监听，获取Slider属性并设置到Thumb、RepeatButton
        /// <summary>
        /// 启用监听，获取Slider属性并设置到Thumb、RepeatButton
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
                }
            }
            else if (obj is RepeatButton repeatButton)
            {
                if (repeatButton.TemplatedParent is SliderEXT sliderEXT)
                {
                    repeatButton.SetValue(TrackColorLinearProperty, sliderEXT.TrackColorLinear);
                }
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
            DependencyProperty.RegisterAttached(nameof(ButtonType), typeof(ButtonType), typeof(SliderMonitor),
                new PropertyMetadata(ButtonType.Round));
        /// <summary>
        /// 按钮类型
        /// </summary>
        public ButtonType ButtonType
        {
            get { return (ButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TrackColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(TrackColorLinear), typeof(ColorLinear), typeof(SliderMonitor),
                new PropertyMetadata(new ColorLinear()));
        /// <summary>
        /// 轨道线性颜色
        /// </summary>
        public ColorLinear TrackColorLinear
        {
            get { return (ColorLinear)GetValue(TrackColorLinearProperty); }
            set { SetValue(TrackColorLinearProperty, value); }
        }

        #endregion
    }
}
