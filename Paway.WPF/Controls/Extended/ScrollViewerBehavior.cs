using System;
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
                var current = scrollViewer.HorizontalOffset;
                if (toValue > 0 && scrollViewer.Tag is ScrollInfo scrollInfo && Math.Abs(toValue - current) > scrollInfo.Interval)
                {
                    scrollInfo.Pause(scrollViewer);
                }
                else
                {
                    scrollViewer.ScrollToHorizontalOffset(toValue);
                }
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
                var current = scrollViewer.VerticalOffset;
                if (toValue > 0 && scrollViewer.Tag is ScrollInfo scrollInfo && Math.Abs(toValue - current) > scrollInfo.Interval)
                {
                    scrollInfo.Pause(scrollViewer);
                }
                else
                {
                    scrollViewer.ScrollToVerticalOffset(toValue);
                }
            }
        }

        /// <summary>
        /// 自动滚动
        /// </summary>
        /// <param name="scrollViewer"></param>
        /// <param name="time">滚动耗时(s)(-1时默认30距离/s)</param>
        /// <param name="beginTime">开始延时(默认2s)</param>
        /// <param name="forever">无限重复(默认true)</param>
        public static void AutoScroll(ScrollViewer scrollViewer, double time = -1, double beginTime = 2, bool forever = true)
        {
            if (beginTime <= 0) beginTime = 0.1;
            if (time < 0)
            {
                if (scrollViewer.ScrollableHeight > 0) time = scrollViewer.ScrollableHeight / 30;
                else if (scrollViewer.ScrollableWidth > 0) time = scrollViewer.ScrollableWidth / 30;
            }
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
                scrollViewer.Tag = new ScrollInfo(storyboard, time, beginTime, scrollViewer.ScrollableHeight / time / 12);
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
                scrollViewer.Tag = new ScrollInfo(storyboard, time, beginTime, scrollViewer.ScrollableWidth / time / 12);
            }
            if (storyboard.Children.Count > 0)
            {
                scrollViewer.PreviewMouseDown += ScrollViewer_PreviewMouseDown;
                storyboard.Begin();
            }
        }
        private static void ScrollViewer_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer && scrollViewer.Tag is ScrollInfo scrollInfo)
            {
                scrollInfo.Pause(scrollViewer);
            }
        }

        #endregion

        /// <summary>
        /// 自动滚动数据
        /// </summary>
        private class ScrollInfo
        {
            /// <summary>
            /// 故事板
            /// </summary>
            public Storyboard Storyboard { get; set; }
            /// <summary>
            /// 间隔判断
            /// </summary>
            public double Interval { get; set; }
            /// <summary>
            /// 运行时间
            /// </summary>
            public double Time { get; set; }
            /// <summary>
            /// 延迟停止时间
            /// </summary>
            public double BeginTime { get; set; }
            /// <summary>
            /// 暂停序号，用于重复暂停
            /// </summary>
            private int PauseIndex;

            public ScrollInfo(Storyboard storyboard, double time, double beginTime, double interval)
            {
                this.Storyboard = storyboard;
                this.Time = time;
                this.BeginTime = beginTime;
                this.Interval = interval;
            }
            public void Pause(ScrollViewer scrollViewer)
            {
                var autoTime = 0d;
                if (scrollViewer.ScrollableHeight > 0) autoTime = scrollViewer.VerticalOffset * this.Time / scrollViewer.ScrollableHeight + this.BeginTime;
                else if (scrollViewer.ScrollableWidth > 0) autoTime = scrollViewer.HorizontalOffset * this.Time / scrollViewer.ScrollableWidth + this.BeginTime;
                this.Storyboard.Pause();
                this.Storyboard.Seek(TimeSpan.FromSeconds(autoTime));
                var index = scrollViewer.Lock(() => { return ++this.PauseIndex; });
                Task.Run(() =>
                {
                    Thread.Sleep((int)(this.BeginTime * 1000));
                    if (index == this.PauseIndex)
                    {
                        if (scrollViewer is ScrollViewerEXT viewerEXT && viewerEXT.IDrag) this.Pause(scrollViewer);
                        else this.Storyboard.Resume();
                    }
                });
            }
        }
    }
}
