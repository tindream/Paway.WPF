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
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(SliderMonitor), new PropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Thumb thumb)
            {
                if (thumb.TemplatedParent is SliderEXT sliderEXT)
                {
                    sliderEXT.OnButtonTypeEvent += () =>
                    {
                        thumb.SetValue(ButtonTypeProperty, sliderEXT.ButtonType);
                        thumb.SetValue(ButtonImageProperty, sliderEXT.ButtonImage);
                    };
                    thumb.SetValue(ButtonTypeProperty, sliderEXT.ButtonType);
                    thumb.SetValue(ButtonImageProperty, sliderEXT.ButtonImage);
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

        #region 依赖属性_按钮类型
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
        /// </summary>
        public static readonly DependencyProperty ButtonImageProperty =
            DependencyProperty.RegisterAttached(nameof(ButtonImage), typeof(ImageSource), typeof(SliderMonitor));
        /// <summary>
        /// 按钮图标
        /// <para>默认值：无</para>
        /// </summary>
        public ImageSource ButtonImage
        {
            get { return (ImageSource)GetValue(ButtonImageProperty); }
            set { SetValue(ButtonImageProperty, value); }
        }

        #endregion
    }
}
