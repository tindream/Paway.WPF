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
        public static void Start(FrameworkElement element, TransitionType type, double toValue, int time, Action completed = null)
        {
            Start(element, type, toValue, null, time, completed);
        }
        /// <summary>
        /// 启动动画
        /// </summary>
        public static void Start(FrameworkElement element, TransitionType type, double toValue = 0, double? fromValue = null, int time = 0, Action completed = null)
        {
            if (element == null) return;
            switch (type)
            {
                case TransitionType.Width:
                    element.BeginAnimation(FrameworkElement.WidthProperty, GetDoubleAnimation(fromValue ?? element.ActualWidth, toValue, element, time, () => { element.BeginAnimation(FrameworkElement.WidthProperty, null); completed?.Invoke(); }));
                    break;
                case TransitionType.Height:
                    element.BeginAnimation(FrameworkElement.HeightProperty, GetDoubleAnimation(fromValue ?? element.ActualHeight, toValue, element, time, () => { element.BeginAnimation(FrameworkElement.HeightProperty, null); completed?.Invoke(); }));
                    break;
                case TransitionType.Opacity:
                    element.BeginAnimation(UIElement.OpacityProperty, GetDoubleAnimation(fromValue ?? element.Opacity, toValue, element, time, () => { element.BeginAnimation(UIElement.OpacityProperty, null); completed?.Invoke(); }));
                    break;
                case TransitionType.FadeIn:
                    element.BeginAnimation(UIElement.OpacityProperty, GetDoubleAnimation(0, 1, element, time, () => { element.BeginAnimation(UIElement.OpacityProperty, null); completed?.Invoke(); }));
                    break;
                case TransitionType.FadeOut:
                    element.BeginAnimation(UIElement.OpacityProperty, GetDoubleAnimation(1, 0, element, time, () => { element.BeginAnimation(UIElement.OpacityProperty, null); completed?.Invoke(); }));
                    break;
                #region TranslateTransform
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
                            translateTransform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(fromValue ?? -element.ActualWidth, toValue, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.XProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.Right:
                            translateTransform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(fromValue ?? element.ActualWidth, toValue, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.XProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.Top:
                            translateTransform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(fromValue ?? -element.ActualHeight, toValue, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.YProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.Bottom:
                            translateTransform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(fromValue ?? element.ActualHeight, toValue, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.YProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.ToLeft:
                            translateTransform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(toValue, fromValue ?? -element.ActualWidth, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.XProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.ToRight:
                            translateTransform.BeginAnimation(TranslateTransform.XProperty, GetDoubleAnimation(toValue, fromValue ?? element.ActualWidth, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.XProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.ToTop:
                            translateTransform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(toValue, fromValue ?? -element.ActualHeight, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.YProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.ToBottom:
                            translateTransform.BeginAnimation(TranslateTransform.YProperty, GetDoubleAnimation(toValue, fromValue ?? element.ActualHeight, element, time, () => { translateTransform.BeginAnimation(TranslateTransform.YProperty, null); completed?.Invoke(); }));
                            break;
                    }
                    break;

                #endregion
                #region ScaleTransform
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
                            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, GetDoubleAnimation(fromValue ?? 0, toValue, element, time, () => { scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, null); completed?.Invoke(); }));
                            break;
                        case TransitionType.ScanY:
                            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, GetDoubleAnimation(fromValue ?? 0, toValue, element, time, () => { scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, null); completed?.Invoke(); }));
                            break;
                    }
                    break;

                    #endregion
            }
        }
        /// <summary>
        /// 启动动画
        /// </summary>
        public static void Start(FrameworkElement element, DependencyProperty property, double toValue = 0, double? fromValue = null, int time = 0, Action completed = null)
        {
            if (element == null) return;
            element.BeginAnimation(property, GetDoubleAnimation(fromValue ?? element.ActualWidth, toValue, element, time, () => { element.BeginAnimation(property, null); completed?.Invoke(); }));
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
