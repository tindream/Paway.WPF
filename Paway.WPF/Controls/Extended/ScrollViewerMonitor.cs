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
                    if (scrollBar.TemplatedParent is ScrollViewerEXT ScrollViewerEXT)
                    {
                        thumb.SetValue(ScrollBarRadiusProperty, ScrollViewerEXT.ScrollBarRadius);
                        thumb.SetValue(ScrollBarColorProperty, ScrollViewerEXT.ScrollBarColor);
                        return;
                    }
                    else if (scrollBar.TemplatedParent is ScrollViewer scrollViewer)
                    {
                        if (scrollViewer.TemplatedParent is DataGridEXT dataGrid)
                        {
                            SetVerticalScrollBarMargin(thumb, dataGrid);
                            dataGrid.Loaded += delegate { SetVerticalScrollBarMargin(thumb, dataGrid); };
                        }
                    }
                }
                thumb.SetValue(ScrollBarRadiusProperty, new ScrollViewerEXT().ScrollBarRadius);
                thumb.SetValue(ScrollBarColorProperty, new ScrollViewerEXT().ScrollBarColor);
            }
        }
        private static void SetVerticalScrollBarMargin(Thumb thumb, DataGridEXT dataGrid)
        {
            if (Method.Child(dataGrid, out DataGridColumnHeadersPresenter headersPresenter))
            {
                thumb.SetValue(VerticalScrollBarMarginProperty, new Thickness(0, -headersPresenter.ActualHeight, 0, 0));
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

        #endregion
    }
}
