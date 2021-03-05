using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// ScrollViewer行为
    /// </summary>
    public class ScrollViewerBehavior
    {
        #region 自动滚动
        /// <summary>
        /// 水平偏移量
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.RegisterAttached(nameof(HorizontalOffset), typeof(double), typeof(ScrollViewerBehavior), new UIPropertyMetadata(0.0, OnHorizontalOffsetChanged));
        /// <summary>
        /// </summary>
        private double HorizontalOffset { get; set; }
        /// <summary>
        /// </summary>
        public static void SetHorizontalOffset(FrameworkElement target, double value) => target.SetValue(HorizontalOffsetProperty, value);
        /// <summary>
        /// </summary>
        public static double GetHorizontalOffset(FrameworkElement target) => (double)target.GetValue(HorizontalOffsetProperty);
        private static void OnHorizontalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            (target as ScrollViewer)?.ScrollToHorizontalOffset((double)e.NewValue);
        }

        /// <summary>
        /// 垂直偏移量
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(nameof(VerticalOffset), typeof(double), typeof(ScrollViewerBehavior), new UIPropertyMetadata(0.0, OnVerticalOffsetChanged));
        /// <summary>
        /// </summary>
        private double VerticalOffset { get; set; }
        /// <summary>
        /// </summary>
        public static void SetVerticalOffset(FrameworkElement target, double value) => target.SetValue(VerticalOffsetProperty, value);
        /// <summary>
        /// </summary>
        public static double GetVerticalOffset(FrameworkElement target) => (double)target.GetValue(VerticalOffsetProperty);
        private static void OnVerticalOffsetChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            (target as ScrollViewer)?.ScrollToVerticalOffset((double)e.NewValue);
        }

        /// <summary>
        /// 自动垂直滚动
        /// </summary>
        /// <param name="scrollViewer"></param>
        /// <param name="time">滚动耗时</param>
        public static void AutoScroll(ScrollViewer scrollViewer, int time)
        {
            AutoScroll(scrollViewer, ScrollViewerBehavior.VerticalOffsetProperty, time);
        }
        /// <summary>
        /// 自动滚动
        /// </summary>
        /// <param name="scrollViewer"></param>
        /// <param name="property"></param>
        /// <param name="time">滚动耗时</param>
        /// <param name="forever">无限重复</param>
        public static void AutoScroll(ScrollViewer scrollViewer, DependencyProperty property, int time, bool forever = true)
        {
            var storyboard = new Storyboard();
            if (forever) storyboard.RepeatBehavior = RepeatBehavior.Forever;
            //storyboard.Duration = new Duration(TimeSpan.FromSeconds(time));//上下滚动总时长

            var animY = new DoubleAnimation(0, scrollViewer.ScrollableHeight, TimeSpan.FromSeconds(time));
            //animY.EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut };//EasingMode：滚动方式
            //animY.AutoReverse = true;
            Storyboard.SetTargetName(animY, scrollViewer.Name);
            Storyboard.SetTargetProperty(animY, new PropertyPath(property));
            storyboard.Children.Add(animY);

            storyboard.Begin(scrollViewer);
        }

        #endregion
    }
}
