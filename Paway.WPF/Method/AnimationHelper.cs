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
        public static void Start(FrameworkElement element, TransitionType type, double value = 0, double time = 0, Action completed = null)
        {
            if (element == null) return;
            switch (type)
            {
                case TransitionType.FadeIn:
                    element.BeginAnimation(FrameworkElement.OpacityProperty, GetDoubleAnimation(0, 1, element, value, time, completed));
                    break;
                case TransitionType.FadeOut:
                    element.BeginAnimation(FrameworkElement.OpacityProperty, GetDoubleAnimation(1, 0, element, value, time, completed));
                    break;
                case TransitionType.Left:
                case TransitionType.Right:
                case TransitionType.Top:
                case TransitionType.Bottom:
                case TransitionType.ToLeft:
                case TransitionType.ToRight:
                case TransitionType.ToTop:
                case TransitionType.ToBottom:
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
                            transform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(-element.ActualWidth, 0, element, value, time, completed));
                            break;
                        case TransitionType.Right:
                            transform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(element.ActualWidth, 0, element, value, time, completed));
                            break;
                        case TransitionType.Top:
                            transform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(-element.ActualHeight, 0, element, value, time, completed));
                            break;
                        case TransitionType.Bottom:
                            transform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(element.ActualHeight, 0, element, value, time, completed));
                            break;
                        case TransitionType.ToLeft:
                            transform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(0, -element.ActualWidth, element, value, time, completed));
                            break;
                        case TransitionType.ToRight:
                            transform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(0, element.ActualWidth, element, value, time, completed));
                            break;
                        case TransitionType.ToTop:
                            transform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(0, -element.ActualHeight, element, value, time, completed));
                            break;
                        case TransitionType.ToBottom:
                            transform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(0, element.ActualHeight, element, value, time, completed));
                            break;
                    }
                    break;
            }
        }

        #region Function
        private static DoubleAnimation GetDoubleAnimation(double from, double to, FrameworkElement element, double value = 0, double animTime = 0, Action completed = null)
        {
            if (animTime == 0)
            {
                if (value == 0)
                {
                    value = Math.Abs(from - to);
                    if (value <= 1) value = Math.Max(element.ActualWidth, element.ActualHeight);
                }
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
                completed?.Invoke();
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
