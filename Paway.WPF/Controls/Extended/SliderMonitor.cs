using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                    thumb.SetValue(TrackColorLinearProperty, sliderEXT.TrackColorLinear);
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
            DependencyProperty.RegisterAttached(nameof(ButtonType), typeof(SliderButtonType), typeof(SliderMonitor),
                new PropertyMetadata(SliderButtonType.Round));
        /// <summary>
        /// 按钮类型
        /// <para>默认值：Round</para>
        /// </summary>
        public SliderButtonType ButtonType
        {
            get { return (SliderButtonType)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        /// <summary>
        /// 轨道线性颜色
        /// </summary>
        public static readonly DependencyProperty TrackColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(TrackColorLinear), typeof(ColorLinear), typeof(SliderMonitor),
                new PropertyMetadata(new ColorLinear()));
        /// <summary>
        /// 轨道线性颜色
        /// <para>默认值：默认</para>
        /// </summary>
        public ColorLinear TrackColorLinear
        {
            get { return (ColorLinear)GetValue(TrackColorLinearProperty); }
            set { SetValue(TrackColorLinearProperty, value); }
        }

        #endregion
    }
}
