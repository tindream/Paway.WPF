using System;
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
                element.Opacity = 0;

                if (element.IsLoaded)
                {
                    StartAnimation(element, type);
                }
                else
                {
                    element.Loaded += delegate
                    {
                        StartAnimation(element, type);
                    };
                }
            }
        }
        private static void StartAnimation(FrameworkElement element, TransitionType type)
        {
            switch (type)
            {
                case TransitionType.FadeIn:
                    element.BeginAnimation(FrameworkElement.OpacityProperty, GetDoubleAnimation(0, 1, element));
                    break;
                case TransitionType.FadeOut:
                    element.BeginAnimation(FrameworkElement.OpacityProperty, GetDoubleAnimation(1, 0, element));
                    break;
                case TransitionType.Left:
                case TransitionType.Right:
                case TransitionType.Top:
                case TransitionType.Bottom:
                    var transform = new TranslateTransform();
                    if (element.RenderTransform != null)
                    {
                        if (element.RenderTransform.GetType() == typeof(TransformGroup))
                        {
                            ((TransformGroup)element.RenderTransform).Children.Add(transform);
                        }
                        else if (element.RenderTransform.GetType() == typeof(Transform))
                        {
                            var group = new TransformGroup();
                            group.Children.Add(element.RenderTransform);
                            group.Children.Add(transform);
                        }
                        else
                        {
                            element.RenderTransform = transform;
                        }
                    }
                    else
                    {
                        element.RenderTransform = transform;
                    }
                    switch (type)
                    {
                        case TransitionType.Left:
                            transform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(-element.ActualWidth, 0, element));
                            break;
                        case TransitionType.Right:
                            transform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(element.ActualWidth, 0, element));
                            break;
                        case TransitionType.Top:
                            transform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(-element.ActualHeight, 0, element));
                            break;
                        case TransitionType.Bottom:
                            transform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(element.ActualHeight, 0, element));
                            break;
                    }
                    break;
            }
        }

        #region Function
        private static DoubleAnimation GetDoubleAnimation(double from, double to, FrameworkElement element, double animTime = 0)
        {
            if (animTime == 0)
            {
                var value = Math.Abs(from - to);
                if (value <= 1) value = Math.Max(element.ActualWidth, element.ActualHeight);
                animTime = TMethod.AnimTime(value);
            }
            var anima = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(animTime),
            };
            anima.Completed += delegate
            {
                element.RaiseEvent(new RoutedEventArgs(CompletedEvent));
            };
            return anima;
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
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.AddHandler(CompletedEvent, handler);
            }
        }
        /// <summary>
        /// Remove
        /// </summary>
        public static void RemoveCompletedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            UIElement uie = d as UIElement;
            if (uie != null)
            {
                uie.RemoveHandler(CompletedEvent, handler);
            }
        }

        #endregion
    }
}
