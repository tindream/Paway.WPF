﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Paway.WPF
{
    /// <summary>
    /// 动画辅助
    /// </summary>
    public class AnimationHelper
    {
        /// <summary>
        /// 使控件立即进行透明度从0至1的渐变动画。若控件尚未加载完成，则将在其加载完成后再执行动画。
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.RegisterAttached(nameof(Value), typeof(TransitionType), typeof(AnimationHelper), new PropertyMetadata(OnValueChanged));
        /// <summary>
        /// 动画类型
        /// </summary>
        public TransitionType Value { get; set; }
        /// <summary>
        /// Set动画类型
        /// </summary>
        public static TransitionType GetValue(DependencyObject obj)
        {
            return (TransitionType)obj.GetValue(ValueProperty);
        }
        /// <summary>
        /// Get动画类型
        /// </summary>
        public static void SetValue(DependencyObject obj, TransitionType value)
        {
            obj.SetValue(ValueProperty, value);
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && e.NewValue is TransitionType type)
            {
                if (element.IsLoaded)
                {
                    Start(element, type);
                }
                else
                {
                    element.Loaded += delegate
                    {
                        Start(element, type);
                    };
                }
            }
        }
        /// <summary>
        /// 启动动画
        /// </summary>
        public static void Start(FrameworkElement element, TransitionType type, double toValue, int time, Action completed = null, bool iReset = true)
        {
            Start(element, type, toValue, null, time, completed, iReset);
        }
        /// <summary>
        /// 启动动画
        /// </summary>
        public static void Start(FrameworkElement element, TransitionType type, double toValue = 0, double? fromValue = null, int time = 0, Action completed = null, bool iReset = true)
        {
            if (element == null) return;
            switch (type)
            {
                case TransitionType.Width:
                    Start(element, FrameworkElement.WidthProperty, fromValue ?? element.ActualWidth, toValue, time, completed, iReset);
                    break;
                case TransitionType.Height:
                    Start(element, FrameworkElement.HeightProperty, fromValue ?? element.ActualHeight, toValue, time, completed, iReset);
                    break;
                case TransitionType.Opacity:
                    Start(element, UIElement.OpacityProperty, fromValue ?? element.Opacity, toValue, time, completed, iReset);
                    break;
                case TransitionType.FadeIn:
                    Start(element, UIElement.OpacityProperty, 0, 1, time, completed, iReset);
                    break;
                case TransitionType.FadeOut:
                    Start(element, UIElement.OpacityProperty, 1, 0, time, completed, iReset);
                    break;
                #region TranslateTransform 平移
                case TransitionType.Left:
                case TransitionType.Right:
                case TransitionType.Top:
                case TransitionType.Bottom:
                case TransitionType.ToLeft:
                case TransitionType.ToRight:
                case TransitionType.ToTop:
                case TransitionType.ToBottom:
                    var translateTransform = new TranslateTransform();
                    if (element.RenderTransform != null)
                    {
                        if (element.RenderTransform.GetType() == typeof(TransformGroup))
                        {
                            ((TransformGroup)element.RenderTransform).Children.Add(translateTransform);
                        }
                        else if (element.RenderTransform.GetType() == typeof(TranslateTransform))
                        {
                            var group = new TransformGroup();
                            group.Children.Add(element.RenderTransform);
                            group.Children.Add(translateTransform);
                            element.RenderTransform = group;
                        }
                        else
                        {
                            element.RenderTransform = translateTransform;
                        }
                    }
                    else
                    {
                        element.RenderTransform = translateTransform;
                    }
                    switch (type)
                    {
                        case TransitionType.Left:
                            Start(translateTransform, element, TranslateTransform.XProperty, fromValue ?? -element.ActualWidth, toValue, time, completed, iReset);
                            break;
                        case TransitionType.Right:
                            Start(translateTransform, element, TranslateTransform.XProperty, fromValue ?? element.ActualWidth, toValue, time, completed, iReset);
                            break;
                        case TransitionType.Top:
                            Start(translateTransform, element, TranslateTransform.YProperty, fromValue ?? -element.ActualHeight, toValue, time, completed, iReset);
                            break;
                        case TransitionType.Bottom:
                            Start(translateTransform, element, TranslateTransform.YProperty, fromValue ?? element.ActualHeight, toValue, time, completed, iReset);
                            break;
                        case TransitionType.ToLeft:
                            Start(translateTransform, element, TranslateTransform.XProperty, toValue, fromValue ?? -element.ActualWidth, time, completed, iReset);
                            break;
                        case TransitionType.ToRight:
                            Start(translateTransform, element, TranslateTransform.XProperty, toValue, fromValue ?? element.ActualWidth, time, completed, iReset);
                            break;
                        case TransitionType.ToTop:
                            Start(translateTransform, element, TranslateTransform.YProperty, toValue, fromValue ?? -element.ActualHeight, time, completed, iReset);
                            break;
                        case TransitionType.ToBottom:
                            Start(translateTransform, element, TranslateTransform.YProperty, toValue, fromValue ?? element.ActualHeight, time, completed, iReset);
                            break;
                    }
                    break;

                #endregion
                #region ScaleTransform 缩放
                case TransitionType.ScanX:
                case TransitionType.ScanY:
                    var scaleTransform = new ScaleTransform();
                    if (element.RenderTransform != null)
                    {
                        if (element.RenderTransform.GetType() == typeof(TransformGroup))
                        {
                            ((TransformGroup)element.RenderTransform).Children.Add(scaleTransform);
                        }
                        else if (element.RenderTransform.GetType() == typeof(ScaleTransform))
                        {
                            var group = new TransformGroup();
                            group.Children.Add(element.RenderTransform);
                            group.Children.Add(scaleTransform);
                            element.RenderTransform = group;
                        }
                        else
                        {
                            element.RenderTransform = scaleTransform;
                        }
                    }
                    else
                    {
                        element.RenderTransform = scaleTransform;
                    }
                    switch (type)
                    {
                        case TransitionType.ScanX:
                            Start(scaleTransform, element, ScaleTransform.ScaleXProperty, fromValue ?? 0, toValue, time, completed);
                            break;
                        case TransitionType.ScanY:
                            Start(scaleTransform, element, ScaleTransform.ScaleYProperty, fromValue ?? 0, toValue, time, completed);
                            break;
                    }
                    break;

                #endregion
                #region RotateTransform 旋转
                case TransitionType.Rotate:
                    var rotateTransform = new RotateTransform();
                    if (element.RenderTransform != null)
                    {
                        if (element.RenderTransform.GetType() == typeof(TransformGroup))
                        {
                            ((TransformGroup)element.RenderTransform).Children.Add(rotateTransform);
                        }
                        else if (element.RenderTransform.GetType() == typeof(RotateTransform))
                        {
                            var group = new TransformGroup();
                            group.Children.Add(element.RenderTransform);
                            group.Children.Add(rotateTransform);
                            element.RenderTransform = group;
                        }
                        else
                        {
                            element.RenderTransform = rotateTransform;
                        }
                    }
                    else
                    {
                        element.RenderTransform = rotateTransform;
                    }
                    Start(rotateTransform, element, RotateTransform.AngleProperty, fromValue ?? 0, toValue, time, completed);
                    break;

                    #endregion
            }
        }
        /// <summary>
        /// 直接启动二维动画
        /// <para>iReset:还原动画</para>
        /// </summary>
        public static void Start(Transform transform, FrameworkElement element, DependencyProperty property, double fromValue, double toValue, int time = 0, Action completed = null, bool iReset = true)
        {
            transform.BeginAnimation(property, GetDoubleAnimation(fromValue, toValue, element, time, () => { if (iReset) transform.BeginAnimation(property, null); completed?.Invoke(); }));
        }
        /// <summary>
        /// 直接启动动画
        /// <para>iReset:还原动画</para>
        /// </summary>
        public static void Start(FrameworkElement element, DependencyProperty property, double fromValue, double toValue, int time = 0, Action completed = null, bool iReset = true)
        {
            if (element == null) return;
            element.BeginAnimation(property, GetDoubleAnimation(fromValue, toValue, element, time, () => { if (iReset) element.BeginAnimation(property, null); completed?.Invoke(); }));
        }

        #region Function
        private static DoubleAnimation GetDoubleAnimation(double from, double to, FrameworkElement element, double animTime = 0, Action completed = null)
        {
            if (animTime == 0)
            {
                var value = Math.Abs(from - to);
                if (value <= 1) value = Math.Max(element.ActualWidth, element.ActualHeight);
                animTime = PMethod.AnimTime(value);
            }
            var animation = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(animTime),
            };
            animation.Completed += delegate
            {
                completed?.Invoke();
                element.RaiseEvent(new RoutedEventArgs(CompletedEvent));
            };
            return animation;
        }

        #endregion

        #region (Event) 
        /// <summary>
        /// 完成事件
        /// </summary>
        public static readonly RoutedEvent CompletedEvent = EventManager.RegisterRoutedEvent("Completed", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AnimationHelper));
        /// <summary>
        /// Add
        /// </summary>
        public static void AddCompletedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie)
            {
                uie.AddHandler(CompletedEvent, handler);
            }
        }
        /// <summary>
        /// Remove
        /// </summary>
        public static void RemoveCompletedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie)
            {
                uie.RemoveHandler(CompletedEvent, handler);
            }
        }

        #endregion
    }
}
