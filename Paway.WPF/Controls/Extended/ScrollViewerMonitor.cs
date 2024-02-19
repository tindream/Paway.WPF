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
    /// ScrollViewer扩展监听
    /// </summary>
    internal class ScrollViewerMonitor : DependencyObject
    {
        #region 启用监听，获取ScrollViewer属性并设置到Thumb
        /// <summary>
        /// 启用监听，获取ScrollViewer属性并设置到Thumb
        /// </summary>
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached(nameof(IsMonitoring), typeof(bool), typeof(ScrollViewerMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Thumb thumb)
            {
                if (thumb.TemplatedParent is ScrollBar scrollBar)
                {
                    if (scrollBar.TemplatedParent is ScrollViewerEXT scrollViewerEXT)
                    {
                        thumb.SetValue(ScrollBarRadiusProperty, scrollViewerEXT.ScrollBarRadius);
                        thumb.SetValue(ScrollBarColorProperty, scrollViewerEXT.ScrollBarColor);
                    }
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
        /// 滚动条圆角
        /// </summary>
        public static readonly DependencyProperty ScrollBarRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarRadius), typeof(CornerRadius), typeof(ScrollViewerMonitor), new PropertyMetadata(new CornerRadius(4)));
        /// <summary>
        /// 滚动条圆角
        /// <para>默认值：4</para>
        /// </summary>
        public CornerRadius ScrollBarRadius
        {
            get { return (CornerRadius)GetValue(ScrollBarRadiusProperty); }
            set { SetValue(ScrollBarRadiusProperty, value); }
        }

        /// <summary>
        /// 滚动条颜色
        /// </summary>
        public static readonly DependencyProperty ScrollBarColorProperty =
            DependencyProperty.RegisterAttached(nameof(ScrollBarColor), typeof(Brush), typeof(ScrollViewerMonitor), new PropertyMetadata(PMethod.AlphaColor(PConfig.Alpha, Colors.DarkGray).ToBrush()));
        /// <summary>
        /// 滚动条颜色
        /// <para>默认值：(200, DarkGray)</para>
        /// </summary>
        [Category("扩展")]
        [Description("滚动条颜色")]
        public Brush ScrollBarColor
        {
            get { return (Brush)GetValue(ScrollBarColorProperty); }
            set { SetValue(ScrollBarColorProperty, value); }
        }

        #endregion
    }
}
