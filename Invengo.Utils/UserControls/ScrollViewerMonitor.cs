using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Invengo.Utils
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

        public static readonly DependencyProperty ScrollBarRadiusProperty =
            DependencyProperty.RegisterAttached("ScrollBarRadius", typeof(CornerRadius), typeof(ScrollViewerMonitor));
        public static readonly DependencyProperty ScrollBarColorProperty =
            DependencyProperty.RegisterAttached("ScrollBarColor", typeof(Brush), typeof(ScrollViewerMonitor));
    }
}
