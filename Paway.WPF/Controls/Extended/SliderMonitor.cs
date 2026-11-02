using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        /// 每个 SliderEXT 只订阅一次的按钮样式回调
        /// <para>原实现每次设置附加属性都用一个捕获 Thumb 的匿名委托订阅 SliderEXT.OnButtonTypeEvent：
        /// 滑块长期存活而 Thumb 随模板重建，旧 Thumb 被回调永久持有（模板重复应用时还会累积回调）。
        /// 这里改为每个滑块一个回调，回调内按需取当前模板中的 Thumb，不持有任何 Thumb 引用。</para>
        /// </summary>
        private static readonly ConditionalWeakTable<SliderEXT, Action> handlerList = new ConditionalWeakTable<SliderEXT, Action>();

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
                    if (!handlerList.TryGetValue(sliderEXT, out Action handler))
                    {
                        var slider = sliderEXT;
                        handler = () => ApplyThumb(slider);
                        handlerList.Add(slider, handler);
                        sliderEXT.OnButtonTypeEvent += handler;
                    }
                    thumb.SetValue(ButtonTypeProperty, sliderEXT.ButtonType);
                    thumb.SetValue(ButtonImageProperty, sliderEXT.ButtonImage);
                }
            }
        }
        private static void ApplyThumb(SliderEXT slider)
        {
            //回调时才取当前模板中的 Thumb，模板重建后自动指向新实例
            if (!slider.Child(out Thumb current)) return;
            current.SetValue(ButtonTypeProperty, slider.ButtonType);
            current.SetValue(ButtonImageProperty, slider.ButtonImage);
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
