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
    public class ScrollViewerMonitor : DependencyObject
    {
        #region 启用监听，获取ScrollViewer属性并设置到Thumb
        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(ScrollViewerMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));
        private static void OnIsMonitoringChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Thumb thumb)
            {
                if (thumb.TemplatedParent is ScrollBar scrollBar)
                {
                    if (scrollBar.TemplatedParent is ScrollViewerRound scrollViewerRound)
                    {
                        thumb.SetValue(ScrollBarRadiusProperty, scrollViewerRound.ScrollBarRadius);
                        thumb.SetValue(ScrollBarColorProperty, scrollViewerRound.ScrollBarColor);
                        return;
                    }
                    else if (scrollBar.TemplatedParent is ScrollViewer scrollViewer)
                    {
                        if (scrollViewer.TemplatedParent is DataGridAuto dataGrid)
                        {
                            thumb.SetValue(VerticalScrollBarMarginProperty, new Thickness(0, -dataGrid.ColumnHeaderHeight, 0, 0));
                        }
                    }
                }
                thumb.SetValue(ScrollBarRadiusProperty, new ScrollViewerRound().ScrollBarRadius);
                thumb.SetValue(ScrollBarColorProperty, new ScrollViewerRound().ScrollBarColor);
            }
        }
        public bool IsMonitoring
        {
            get { return (bool)GetValue(IsMonitoringProperty); }
            set { SetValue(IsMonitoringProperty, value); }
        }

        #endregion

        /// <summary>
        /// 滚动条圆角
        /// </summary>
        public static readonly DependencyProperty ScrollBarRadiusProperty =
            DependencyProperty.RegisterAttached("ScrollBarRadius", typeof(CornerRadius), typeof(ScrollViewerMonitor));
        /// <summary>
        /// 滚动条颜色
        /// </summary>
        public static readonly DependencyProperty ScrollBarColorProperty =
            DependencyProperty.RegisterAttached("ScrollBarColor", typeof(Brush), typeof(ScrollViewerMonitor));
        /// <summary>
        /// 垂直滚动条外边距
        /// </summary>
        public static readonly DependencyProperty VerticalScrollBarMarginProperty =
            DependencyProperty.RegisterAttached("VerticalScrollBarMargin", typeof(Thickness), typeof(ScrollViewerMonitor));
    }
}
