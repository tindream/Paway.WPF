﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            DependencyProperty.RegisterAttached(nameof(HorizontalOffset), typeof(double), typeof(ScrollViewerBehavior), new PropertyMetadata(0d, OnHorizontalOffsetChanged));
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
            if (target is ScrollViewer scrollViewer && e.NewValue is double toValue)
            {
                scrollViewer.ScrollToHorizontalOffset(toValue);
            }
        }

        /// <summary>
        /// 垂直偏移量
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached(nameof(VerticalOffset), typeof(double), typeof(ScrollViewerBehavior), new PropertyMetadata(0d, OnVerticalOffsetChanged));
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
            if (target is ScrollViewer scrollViewer && e.NewValue is double toValue)
            {
                scrollViewer.ScrollToVerticalOffset(toValue);
            }
        }

        /// <summary>
        /// 自动垂直滚动
        /// </summary>
        /// <param name="scrollViewer"></param>
        /// <param name="time">滚动耗时(s)</param>
        /// <param name="beginTime">开始延时(默认2s)</param>
        /// <param name="forever">无限重复(默认true)</param>
        public static void AutoScroll(ScrollViewer scrollViewer, double time, double beginTime = 2, bool forever = true)
        {
            if (beginTime <= 0) beginTime = 0.1;
            var storyboard = new Storyboard
            {
                Duration = new Duration(TimeSpan.FromSeconds(time + beginTime * 2))//上下滚动总时长
            };
            if (forever)
            {
                storyboard.RepeatBehavior = RepeatBehavior.Forever;
            }
            if (scrollViewer.ScrollableHeight > 0)
            {
                var animY = new DoubleAnimation(0, scrollViewer.ScrollableHeight, TimeSpan.FromSeconds(time))
                {
                    BeginTime = TimeSpan.FromSeconds(beginTime)
                };
                //animY.EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut };//EasingMode：滚动方式
                //animY.AutoReverse = true;
                Storyboard.SetTarget(animY, scrollViewer);
                Storyboard.SetTargetName(animY, scrollViewer.Name);
                Storyboard.SetTargetProperty(animY, new PropertyPath(ScrollViewerBehavior.VerticalOffsetProperty));
                storyboard.Children.Add(animY);
            }
            else if (scrollViewer.ScrollableWidth > 0)
            {
                var animX = new DoubleAnimation(0, scrollViewer.ScrollableWidth, TimeSpan.FromSeconds(time))
                {
                    BeginTime = TimeSpan.FromSeconds(beginTime)
                };
                //animY.EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut };//EasingMode：滚动方式
                //animY.AutoReverse = true;
                Storyboard.SetTarget(animX, scrollViewer);
                Storyboard.SetTargetName(animX, scrollViewer.Name);
                Storyboard.SetTargetProperty(animX, new PropertyPath(ScrollViewerBehavior.HorizontalOffsetProperty));
                storyboard.Children.Add(animX);
            }
            storyboard.Begin();
        }

        #endregion
    }
}
