using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法 - 装饰器
    /// </summary>
    public partial class PMethod
    {
        private static FrameworkElement LastWaterElement;
        private static readonly string NameWater = $"{nameof(PMethod)}_{nameof(WaterAdornerFixed)}";
        private static readonly string NameHit = $"{nameof(PMethod)}_{nameof(Hit)}";
        private static readonly string NameToast = $"{nameof(PMethod)}_{nameof(Toast)}";

        #region 装饰器-空白
        /// <summary>
        /// 装饰器-空白画板
        /// <para>element:当前控件</para>
        /// <para>hitTest=true 不路由事件（不穿透）</para>
        /// <para>hitTest=false 路由事件（穿透）</para>
        /// <para>iBottom:置于最底层</para>
        /// </summary>
        public static CustomAdorner CustomAdorner(FrameworkElement element, FrameworkElement content = null, bool hitTest = false, bool iBottom = false)
        {
            return Invoke(() =>
            {
                if (element is Window window && window.Content is FrameworkElement temp) element = temp;
                {
                    var myAdornerLayer = ReloadAdorner(element);
                    if (myAdornerLayer == null) return null;

                    var customAdorner = new CustomAdorner(element, hitTest: hitTest);
                    if (iBottom)
                    {
                        lock (myAdornerLayer)
                        {
                            var list = myAdornerLayer.GetAdorners(element);
                            if (list != null) for (var i = 0; i < list.Length; i++) myAdornerLayer.Remove(list[i]);
                            myAdornerLayer.Add(customAdorner);
                            if (list != null) for (var i = 0; i < list.Length; i++) myAdornerLayer.Add(list[i]);
                        }
                    }
                    else
                    {
                        lock (myAdornerLayer) myAdornerLayer.Add(customAdorner);
                    }
                    if (content != null)
                    {
                        var canvas = customAdorner.GetCanvas();
                        canvas.Children.Add(content);
                        var widthBinding = new Binding
                        {
                            Source = canvas,
                            Path = new PropertyPath(nameof(canvas.ActualWidth)),
                        };
                        var heightBinding = new Binding
                        {
                            Source = canvas,
                            Path = new PropertyPath(nameof(canvas.ActualHeight)),
                        };
                        content.SetBinding(FrameworkElement.WidthProperty, widthBinding);
                        content.SetBinding(FrameworkElement.HeightProperty, heightBinding);
                    }
                    return customAdorner;
                }
            });
        }
        /// <summary>
        /// 清除指定装饰器
        /// </summary>
        public static void ClearAdorner(FrameworkElement element, CustomAdorner adorner)
        {
            Invoke(() =>
            {
                if (element is Window window && window.Content is FrameworkElement temp) element = temp;
                {
                    var myAdornerLayer = ReloadAdorner(element);
                    if (myAdornerLayer == null) return;

                    lock (myAdornerLayer) myAdornerLayer.Remove(adorner);
                }
            });
        }

        #endregion

        #region 装饰器-通知消息
        /// <summary>
        /// 装饰器-通知消息
        /// <para>取父窗体</para>
        /// <para>tag为hitAction点击参数，且具体排它性，会移除上一条tag</para>
        /// </summary>
        public static void Notice(DependencyObject parent, object msg, ColorType type = ColorType.Color, double? fontSize = null, int bottom = 35, object tag = null, Action<object> hitAction = null, Action completed = null)
        {
            Notice(parent, msg, 0, type, fontSize, bottom, tag, hitAction, completed);
        }
        /// <summary>
        /// 装饰器-通知消息
        /// <para>取父窗体</para>
        /// </summary>
        public static void Notice(DependencyObject parent, object msg, int time, ColorType type = ColorType.Color, double? fontSize = null, int bottom = 35, object tag = null, Action<object> hitAction = null, Action completed = null)
        {
            Invoke(() =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is FrameworkElement element)
                {
                    var myAdornerLayer = ReloadAdorner(element);
                    if (myAdornerLayer == null) return;

                    var border = new Border
                    {
                        CornerRadius = new CornerRadius(3),
                    };
                    if (hitAction != null)
                    {
                        var block = new ButtonEXT()
                        {
                            Content = msg.ToStrings(),
                            HorizontalContentAlignment = HorizontalAlignment.Left,
                            Padding = new Thickness(10),
                            Margin = new Thickness(0),
                            Type = type,
                            MinWidth = 200,
                            MaxWidth = 600
                        };
                        if (fontSize != null) block.FontSize = fontSize.Value;
                        block.Click += (sender, e) =>
                        {
                            hitAction.Invoke(tag);
                            AnimationHelper.Start(border, TransitionType.ToRight, completed: () =>
                            {
                                if (border.Tag is Storyboard storyboard1)
                                {
                                    storyboard1.Remove(border);
                                }
                            });
                        };
                        border.Child = block;
                    }
                    else
                    {
                        border.Background = AlphaColor(PConfig.Alpha, type.Color()).ToBrush();
                        var block = new TextBlock()
                        {
                            Text = msg.ToStrings(),
                            Padding = new Thickness(10),
                            TextWrapping = TextWrapping.Wrap,
                            Foreground = Colors.White.ToBrush(),
                            MinWidth = 200,
                            MaxWidth = 600
                        };
                        if (fontSize != null) block.FontSize = fontSize.Value;
                        border.Child = block;
                    }
                    if (time == 0) time = 3000;
                    else if (time < 0) time = int.MaxValue - 125;
                    if (tag != null) NoticeClear(tag);
                    var noticeAdorner = new CustomAdorner(element, border,
                        xFunc: () =>
                        {
                            var right = window is WindowEXT || (window.WindowStyle == System.Windows.WindowStyle.None && window.ResizeMode == ResizeMode.NoResize) ? 2 : 16;
                            return window.ActualWidth - border.ActualWidth - right;
                        },
                        yFunc: () =>
                        {
                            var code = border.GetHashCode();
                            var location = 0d;
                            lock (adornerNoticeLock)
                            {
                                var temp = adornerNoticeList.Find(c => c.Code == code);
                                if (temp == null && border.ActualHeight > 0)
                                {
                                    adornerIndex++;
                                    temp = new AdornerNoticeInfo(adornerIndex, code, border, tag);
                                    adornerNoticeList.Add(temp);
                                }
                                location = adornerNoticeList.Where(c => c.Index < temp?.Index).Sum(c => c.Height);
                            }
                            var top = window.WindowStyle != System.Windows.WindowStyle.None ? (window is WindowEXT windowEXT) ? windowEXT.HeaderHeight : 30 : 0;
                            var bottom2 = window is WindowEXT || (window.WindowStyle == System.Windows.WindowStyle.None && window.ResizeMode == ResizeMode.NoResize) ? 4 : 11;
                            return window.ActualHeight - border.ActualHeight - bottom - top - bottom2 - location;
                        },
                        completedFunc: () =>
                        {
                            var code = border.GetHashCode();
                            lock (adornerNoticeLock)
                            {
                                var temp = adornerNoticeList.Find(c => c.Code == code);
                                if (temp != null) adornerNoticeList.Remove(temp);
                            }
                            completed?.Invoke();
                        },
                        storyboardFunc: () =>
                        {
                            var storyboard = new Storyboard();
                            border.Tag = storyboard;

                            var tt = new TranslateTransform();
                            border.RenderTransform = tt;
                            var animIn = new DoubleAnimation(border.ActualWidth, -2, new Duration(TimeSpan.FromMilliseconds(125)));
                            var propertyX = new DependencyProperty[] { UIElement.RenderTransformProperty, TranslateTransform.XProperty };
                            Storyboard.SetTargetProperty(animIn, new PropertyPath("(0).(1)", propertyX));
                            storyboard.Children.Add(animIn);

                            var animOut = new DoubleAnimation(0, border.ActualHeight, new Duration(TimeSpan.FromMilliseconds(125)))
                            {
                                BeginTime = TimeSpan.FromMilliseconds(time + 125)
                            };
                            var propertyY2 = new DependencyProperty[] { UIElement.RenderTransformProperty, TranslateTransform.YProperty };
                            Storyboard.SetTargetProperty(animOut, new PropertyPath("(0).(1)", propertyY2));
                            storyboard.Children.Add(animOut);

                            return storyboard;
                        }, hitTest: hitAction != null);
                    lock (myAdornerLayer) myAdornerLayer.Add(noticeAdorner);
                }
            });
        }
        /// <summary>
        /// 清空指定tag消息列表装饰器
        /// </summary>
        public static bool NoticeClear(object tag, Action completed = null)
        {
            lock (adornerNoticeLock)
            {
                var list = adornerNoticeList.FindAll(c => c.Tag != null && c.Tag.Equals(tag));
                for (var i = 0; i < list.Count; i++)
                {
                    NoticeClear(list[i], i == list.Count - 1 ? completed : null);
                }
                return list.Count > 0;
            }
        }
        private static void NoticeClear(AdornerNoticeInfo adornerNotice, Action completed)
        {
            AnimationHelper.Start(adornerNotice.Border, TransitionType.ToRight, completed: () =>
            {
                if (adornerNotice.Border.Tag is Storyboard storyboard1)
                {
                    storyboard1.Remove(adornerNotice.Border);
                }
                completed?.Invoke();
            });
        }
        private static readonly object adornerNoticeLock = new object();
        /// <summary>
        /// 通知消息计数器，作为唯一顺序标识
        /// </summary>
        private static int adornerIndex;
        private static readonly List<AdornerNoticeInfo> adornerNoticeList = new List<AdornerNoticeInfo>();
        /// <summary>
        /// 通知消息数据
        /// </summary>
        public class AdornerNoticeInfo
        {
            /// <summary>
            /// 记录创建时的顺序计数，作为唯一标识
            /// <para>批量弹出时，时间戳可能会重复</para>
            /// </summary>
            public int Index { get; set; }
            /// <summary>
            /// 控件Code
            /// </summary>
            public int Code { get; set; }
            /// <summary>
            /// 记录实际高度
            /// </summary>
            public double Height { get; set; }

            /// <summary>
            /// 当前消息控件
            /// </summary>
            public Border Border { get; set; }
            /// <summary>
            /// 标记
            /// </summary>
            public object Tag { get; set; }

            /// <summary>
            /// </summary>
            public AdornerNoticeInfo(int adornerIndex, int code, Border border, object tag)
            {
                this.Index = adornerIndex;
                this.Code = code;
                this.Border = border;
                this.Height = border.ActualHeight + 2;
                this.Tag = tag;
            }
        }

        #endregion

        #region 装饰器-特效
        /// <summary>
        /// 装饰器-收到消息装入列表
        /// <para>当前控件</para>
        /// </summary>
        public static void SlowIn(FrameworkElement parent, object msg, int time = 500, double xMove = 0, double yMove = 0, double size = 36, Color? color = null, Action completed = null)
        {
            Invoke(() =>
            {
                var myAdornerLayer = ReloadAdorner(parent);
                if (myAdornerLayer == null) return;

                var block = new TextBlock()
                {
                    Text = msg.ToStrings(),
                    FontSize = size
                };
                if (color != null) block.Foreground = color.Value.ToBrush();
                var slowInAdorner = new CustomAdorner(parent, block, storyboardFunc: () =>
                {
                    var storyboard = new Storyboard();

                    var animTime = AnimTime(block.FontSize - 12) * 3;
                    var animation = new DoubleAnimation(block.FontSize, 12, new Duration(TimeSpan.FromMilliseconds(animTime)))
                    {
                        BeginTime = TimeSpan.FromMilliseconds(time),
                        AccelerationRatio = 0.9,
                        DecelerationRatio = 0.1
                    };
                    Storyboard.SetTargetProperty(animation, new PropertyPath(TextBlock.FontSizeProperty));
                    storyboard.Children.Add(animation);

                    var tt = new TranslateTransform();
                    block.RenderTransform = tt;
                    if (xMove != 0)
                    {
                        var wMove = (parent.ActualWidth / 2 - block.ActualWidth / 2) * xMove;
                        var animX = new DoubleAnimation(0, wMove, new Duration(TimeSpan.FromMilliseconds(animTime)))
                        {
                            BeginTime = TimeSpan.FromMilliseconds(time),
                            AccelerationRatio = 0.9,
                            DecelerationRatio = 0.1
                        };
                        var propertyX = new DependencyProperty[] { TextBlock.RenderTransformProperty, TranslateTransform.XProperty };
                        Storyboard.SetTargetProperty(animX, new PropertyPath("(0).(1)", propertyX));
                        storyboard.Children.Add(animX);
                    }
                    if (yMove != 0)
                    {
                        var hMove = (parent.ActualHeight / 2 - block.ActualHeight / 2) * yMove;
                        var animY = new DoubleAnimation(0, hMove, new Duration(TimeSpan.FromMilliseconds(animTime)))
                        {
                            AccelerationRatio = 0.1,
                            DecelerationRatio = 0.9,
                            BeginTime = TimeSpan.FromMilliseconds(time)
                        };
                        var propertyY = new DependencyProperty[] { TextBlock.RenderTransformProperty, TranslateTransform.YProperty };
                        Storyboard.SetTargetProperty(animY, new PropertyPath("(0).(1)", propertyY));
                        storyboard.Children.Add(animY);
                    }

                    return storyboard;
                }, completedFunc: completed);
                lock (myAdornerLayer) myAdornerLayer.Add(slowInAdorner);
            });
        }

        /// <summary>
        /// 装饰器-移动触发水波纹底色
        /// </summary>
        public static void WaterAdornerFixed(FrameworkElement element, MouseEventArgs e, double width = 100)
        {
            var myAdornerLayer = ReloadAdorner(element);
            if (myAdornerLayer == null) return;

            var point = e.GetPosition(element);
            var x = Math.Max(element.ActualWidth - point.X, point.X);
            var y = Math.Max(element.ActualHeight - point.Y, point.Y);
            var autoWidth = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 2;
            if (width == 0 || width > autoWidth) width = autoWidth;

            var ellipse = new Ellipse() { Width = width, Height = width, Fill = AlphaColor(20, Colors.Black).ToBrush() };
            if (LastWaterElement != null) ClearAdorner(ReloadAdorner(LastWaterElement), LastWaterElement, NameWater);
            var waterAdornerFixed = new CustomAdorner(element, ellipse, null, () => point.X - ellipse.ActualWidth / 2, () => point.Y - ellipse.ActualHeight / 2, hitTest: false) { Name = NameWater };
            lock (myAdornerLayer) myAdornerLayer.Add(waterAdornerFixed);
            LastWaterElement = element;
        }
        /// <summary>
        /// 装饰器-点击触发水波纹特效
        /// </summary>
        public static void WaterAdorner(MouseEventArgs e, FrameworkElement element = null, double width = 0, double maxWidth = 300)
        {
            if (element == null)
            {
                if (e.OriginalSource is FrameworkElement temp) element = temp;
                else return;
            }
            if (element is Adorner adorner)
            {//暂未使用(响应事件时?)
                if (adorner.AdornedElement is FrameworkElement framework)
                {
                    element = framework;
                }
            }
            if (element is TextBlock textBlock)
            {
                if (textBlock.Parent is FrameworkElement framework)
                {
                    element = framework;
                }
                else if (Parent(textBlock, out ContentPresenter content) && content.Parent is FrameworkElement framework2)
                {
                    element = framework2;
                }
            }
            var myAdornerLayer = WaterAdorner(element, e, null, width, maxWidth);
            if (myAdornerLayer == null)
            {
                if (!Parent(element, out Window window)) return;
                if (window.Content is FrameworkElement element2)
                {
                    WaterAdorner(element2, e, null, width, maxWidth);
                }
            }
        }
        /// <summary>
        /// 装饰器-点击触发水波纹特效
        /// </summary>
        private static AdornerLayer WaterAdorner(FrameworkElement element, MouseEventArgs e, Color? color = null, double width = 0, double maxWidth = 300)
        {
            var myAdornerLayer = ReloadAdorner(element);
            if (myAdornerLayer == null) return null;

            var point = e.GetPosition(element);
            var x = Math.Max(element.ActualWidth - point.X, point.X);
            var y = Math.Max(element.ActualHeight - point.Y, point.Y);
            var autoWidth = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 2;
            if (width == 0 || width > autoWidth) width = autoWidth;
            if (maxWidth > 0 && width > maxWidth) width = maxWidth;

            if (color == null) color = PConfig.Color;
            var ellipse = new Ellipse() { Width = 10, Height = 10, Fill = color.Value.ToBrush() };
            var waterAdorner = new CustomAdorner(element, ellipse, null, () => point.X - ellipse.ActualWidth / 2, () => point.Y - ellipse.ActualHeight / 2, () =>
            {
                var storyboard = new Storyboard();
                var animTime = AnimTime(width) * 1.3;

                var animWidth = new DoubleAnimation(10, width, new Duration(TimeSpan.FromMilliseconds(animTime)));
                Storyboard.SetTargetProperty(animWidth, new PropertyPath(FrameworkElement.WidthProperty));
                storyboard.Children.Add(animWidth);

                var animHeight = new DoubleAnimation(10, width, new Duration(TimeSpan.FromMilliseconds(animTime)));
                Storyboard.SetTargetProperty(animHeight, new PropertyPath(FrameworkElement.HeightProperty));
                storyboard.Children.Add(animHeight);

                var animColor = new ColorAnimation(color.Value, AlphaColor(10, color.Value), new Duration(TimeSpan.FromMilliseconds(300)));
                //ellipse.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animColor);
                var propertyChain = new DependencyProperty[] { Ellipse.FillProperty, SolidColorBrush.ColorProperty };
                Storyboard.SetTargetProperty(animColor, new PropertyPath("(0).(1)", propertyChain));
                storyboard.Children.Add(animColor);

                return storyboard;
            });
            lock (myAdornerLayer) myAdornerLayer.Add(waterAdorner);
            return myAdornerLayer;
        }
        /// <summary>
        /// 清除装饰器上指定名称装饰
        /// </summary>
        private static void ClearAdorner(AdornerLayer myAdornerLayer, FrameworkElement element, string name)
        {
            if (myAdornerLayer == null) return;
            lock (myAdornerLayer)
            {
                var list = myAdornerLayer.GetAdorners(element);
                while (list != null)
                {
                    var last = list.FirstOrDefault(c => c.Name == name);
                    if (last == null) break;
                    myAdornerLayer.Remove(last);
                    list = myAdornerLayer.GetAdorners(element);
                }
            }
        }

        #endregion

        #region 装饰器-提示信息
        /// <summary>
        /// 装饰器-自定义吐泡消息框-Toast
        /// <para>取父窗体</para>
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, ColorType type = ColorType.Color, int? fontSize = null, Action completed = null)
        {
            Toast(parent, msg, 0, type, fontSize, completed);
        }
        /// <summary>
        /// 装饰器-自定义吐泡消息框-Toast
        /// <para>取父窗体</para>
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, int time, ColorType type = ColorType.Color, int? fontSize = null, Action completed = null)
        {
            Invoke(() =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is FrameworkElement element)
                {
                    var myAdornerLayer = ReloadAdorner(element);
                    if (myAdornerLayer == null) return;

                    var color = AlphaColor(PConfig.Alpha, type.Color());
                    var border = new Border
                    {
                        CornerRadius = new CornerRadius(3),
                        Background = color.ToBrush(),
                    };
                    var block = new TextBlock()
                    {
                        Text = msg.ToStrings(),
                        Padding = new Thickness(10),
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Colors.White.ToBrush(),
                        MinWidth = 200,
                        MaxWidth = 600
                    };
                    if (fontSize != null) block.FontSize = fontSize.Value;
                    border.Child = block;
                    if (time == 0) time = 3000;

                    ToastClear(myAdornerLayer, element, NameToast);
                    var toastAdorner = new CustomAdorner(element, border, yFunc: () =>
                    {
                        var top = window.ActualHeight - border.ActualHeight;
                        top = top * 17 / 20;
                        return top;
                    }, storyboardFunc: () =>
                    {
                        var storyboard = new Storyboard();
                        border.Tag = storyboard;

                        var tt = new TranslateTransform();
                        border.RenderTransform = tt;
                        var animIn = new DoubleAnimation(-border.ActualHeight, 0, new Duration(TimeSpan.FromMilliseconds(125)));
                        var propertyY = new DependencyProperty[] { UIElement.RenderTransformProperty, TranslateTransform.YProperty };
                        Storyboard.SetTargetProperty(animIn, new PropertyPath("(0).(1)", propertyY));
                        storyboard.Children.Add(animIn);

                        var animTime = AnimTime(border.ActualHeight);
                        var animColor = new ColorAnimation(color, AlphaColor(0, color), new Duration(TimeSpan.FromMilliseconds(animTime)))
                        {
                            BeginTime = TimeSpan.FromMilliseconds(time + 125)
                        };
                        var propertyChain = new DependencyProperty[] { Border.BackgroundProperty, SolidColorBrush.ColorProperty };
                        Storyboard.SetTargetProperty(animColor, new PropertyPath("(0).(1)", propertyChain));
                        storyboard.Children.Add(animColor);

                        var animOut = new DoubleAnimation(0, border.ActualHeight + 0, new Duration(TimeSpan.FromMilliseconds(animTime)))
                        {
                            BeginTime = TimeSpan.FromMilliseconds(time + 125)
                        };
                        var propertyY2 = new DependencyProperty[] { UIElement.RenderTransformProperty, TranslateTransform.YProperty };
                        Storyboard.SetTargetProperty(animOut, new PropertyPath("(0).(1)", propertyY2));
                        storyboard.Children.Add(animOut);

                        return storyboard;
                    }, completedFunc: completed)
                    { Name = NameToast };
                    lock (myAdornerLayer) myAdornerLayer.Add(toastAdorner);
                }
            });
        }
        private static void ToastClear(AdornerLayer myAdornerLayer, FrameworkElement element, string name)
        {
            Adorner[] list = null;
            lock (myAdornerLayer) list = myAdornerLayer.GetAdorners(element);
            if (list == null) return;
            for (var i = 0; i < list.Length; i++)
            {
                if (list[i].Name == name && list[i] is CustomAdorner customAdorner)
                {
                    var element2 = customAdorner.GetElement();
                    AnimationHelper.Start(element2, TransitionType.ToBottom, completed: () =>
                    {
                        if (element2.Tag is Storyboard storyboard1)
                        {
                            storyboard1.Remove(element2);
                        }
                    });
                }
            }
        }
        /// <summary>
        /// 装饰器-自定义提示框-Hit
        /// <para>取父窗体</para>
        /// </summary>
        public static void Hit(DependencyObject parent, object msg, ColorType type = ColorType.Color, int? fontSize = null, Action completed = null)
        {
            Hit(parent, msg, 0, type, fontSize, completed);
        }
        /// <summary>
        /// 装饰器-自定义提示框-Hit
        /// <para>取父窗体</para>
        /// </summary>
        public static void Hit(DependencyObject parent, object msg, int time, ColorType type = ColorType.Color, int? fontSize = null, Action completed = null)
        {
            Invoke(() =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is FrameworkElement element)
                {
                    var myAdornerLayer = ReloadAdorner(element);
                    if (myAdornerLayer == null) return;

                    var color = AlphaColor(PConfig.Alpha, type.Color());
                    var border = new Border
                    {
                        CornerRadius = new CornerRadius(3),
                        Background = color.ToBrush(),
                    };
                    var block = new TextBlock()
                    {
                        Text = msg.ToStrings(),
                        Padding = new Thickness(10),
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Colors.White.ToBrush(),
                        MinWidth = 200,
                        MaxWidth = 600
                    };
                    if (fontSize != null) block.FontSize = fontSize.Value;
                    border.Child = block;
                    if (time == 0) time = 3000;

                    ClearAdorner(myAdornerLayer, element, NameHit);
                    var hitAdorner = new CustomAdorner(element, border, storyboardFunc: () =>
                    {
                        var storyboard = new Storyboard();

                        var animInColor = new ColorAnimation(AlphaColor(0, color), color, new Duration(TimeSpan.FromMilliseconds(125)));
                        var propertyChain = new DependencyProperty[] { Border.BackgroundProperty, SolidColorBrush.ColorProperty };
                        Storyboard.SetTargetProperty(animInColor, new PropertyPath("(0).(1)", propertyChain));
                        storyboard.Children.Add(animInColor);

                        //放大
                        var scale = new ScaleTransform
                        {
                            CenterX = border.ActualWidth / 2,
                            CenterY = border.ActualHeight / 2
                        };
                        border.RenderTransform = scale;
                        var animInX = new DoubleAnimation(0.1, 1, new Duration(TimeSpan.FromMilliseconds(125)))
                        {
                            AccelerationRatio = 0.1,
                            DecelerationRatio = 0.9
                        };
                        var animInY = new DoubleAnimation(0.8, 1, new Duration(TimeSpan.FromMilliseconds(100)));
                        var propertyX = new PropertyPath("(0).(1)", new DependencyProperty[] { FrameworkElement.RenderTransformProperty, ScaleTransform.ScaleXProperty });
                        var propertyY = new PropertyPath("(0).(1)", new DependencyProperty[] { FrameworkElement.RenderTransformProperty, ScaleTransform.ScaleYProperty });
                        Storyboard.SetTargetProperty(animInX, propertyX);
                        //Storyboard.SetTargetProperty(animInY, propertyY);
                        storyboard.Children.Add(animInX);
                        //storyboard.Children.Add(animInY);

                        var animTime = AnimTime(Math.Max(border.ActualWidth, border.ActualHeight));
                        var animColor = new ColorAnimation(color, AlphaColor(0, color), new Duration(TimeSpan.FromMilliseconds(animTime)))
                        {
                            BeginTime = TimeSpan.FromMilliseconds(time + 125)
                        };
                        Storyboard.SetTargetProperty(animColor, new PropertyPath("(0).(1)", propertyChain));
                        storyboard.Children.Add(animColor);

                        return storyboard;
                    }, completedFunc: completed)
                    { Name = NameHit };
                    lock (myAdornerLayer) myAdornerLayer.Add(hitAdorner);
                }
            });
        }

        #endregion

        #region 装饰器-Window忙碌提示
        /// <summary>
        /// 模式显示Window忙提示框，执行完成后关闭
        /// </summary>
        public static Task Progress(FrameworkElement element, Action action, Action success = null, Action<Exception> error = null, Action completed = null, bool iProgressBar = false, bool iProgressRound = true, int? fontSize = null)
        {
            return Progress(element, null, adorner => action?.Invoke(), success, error, completed, iProgressBar, iProgressRound, fontSize);
        }
        /// <summary>
        /// 模式显示Window忙提示框，执行完成后关闭
        /// </summary>
        public static Task Progress(FrameworkElement element, Action<CustomAdorner> action, Action success = null, Action<Exception> error = null, Action completed = null, bool iProgressBar = false, bool iProgressRound = true, int? fontSize = null)
        {
            return Progress(element, null, action, success, error, completed, iProgressBar, iProgressRound, fontSize);
        }
        /// <summary>
        /// 模式显示Window忙提示框，执行完成后关闭
        /// </summary>
        public static Task Progress(FrameworkElement element, object msg, Action<CustomAdorner> action, Action success = null, Action<Exception> error = null, Action completed = null, bool iProgressBar = false, bool iProgressRound = true, int? fontSize = null)
        {
            return Invoke(() =>
            {
                var progress = ProgressAdorner(element, msg, iProgressBar, iProgressRound, fontSize);
                if (progress == null) throw new WarningException("Decorator not found on control");
                return Task.Run(() =>
                {
                    try
                    {
                        action.Invoke(progress);
                        if (success != null)
                        {
                            BeginInvoke(() =>
                            {
                                success.Invoke();
                            }, ex => InvokeError(error, ex));
                        }
                    }
                    catch (Exception ex)
                    {
                        BeginInvoke(() =>
                        {
                            if (error != null)
                            {
                                error.Invoke(ex);
                            }
                            else
                            {
                                ex.Log();
                                ShowError(element, ex.Message());
                            }
                        });
                    }
                    finally
                    {
                        ProgressClose(progress);
                        if (completed != null)
                        {
                            BeginInvoke(() => completed.Invoke(), ex => InvokeError(error, ex));
                        }
                    }
                });
            }, ex => InvokeError(error, ex));
        }
        private static void InvokeError(Action<Exception> error, Exception ex)
        {
            if (error != null) BeginInvoke(() => error.Invoke(ex));
            else ex.Log();
        }
        /// <summary>
        /// 装饰器-同步显示Window进度条
        /// <para>当前控件</para>
        /// </summary>
        public static CustomAdorner ProgressAdorner(FrameworkElement element, object msg = null, bool iProgressBar = false, bool iProgressRound = true, int? fontSize = null)
        {
            if (element != null)
            {
                if (element is Window window && window.Content is FrameworkElement temp) element = temp;
                var myAdornerLayer = ReloadAdorner(element);
                if (myAdornerLayer == null) return null;

                var border = new Border
                {
                    CornerRadius = new CornerRadius(3),
                    BorderBrush = Colors.LightGray.ToBrush(),
                    BorderThickness = new Thickness(1),
                    Background = AlphaColor(PConfig.Alpha, Colors.White).ToBrush(),
                    MinWidth = 250,
                    MaxWidth = 350,
                    MinHeight = 45,
                };
                Panel panel = new DockPanel();
                if (!iProgressRound) panel = new Grid();
                border.Child = panel;
                {
                    var tbProgress = new TextBlock()
                    {
                        Text = msg == null ? PConfig.Loading : msg.ToStrings(),
                        Foreground = Colors.Black.ToBrush(),
                        Padding = new Thickness(10),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextTrimming = TextTrimming.WordEllipsis,
                    };
                    if (fontSize != null) tbProgress.FontSize = fontSize.Value;
                    DockPanel.SetDock(tbProgress, Dock.Bottom);
                    panel.Children.Add(tbProgress);
                }
                if (iProgressRound)
                {
                    var progressBar = new ProgressBarEXT
                    {
                        IText = false,
                        Margin = new Thickness(0),
                        Radius = new CornerRadius(0),
                        Height = 1,
                    };
                    DockPanel.SetDock(progressBar, Dock.Bottom);
                    panel.Children.Add(progressBar);
                }
                else if (iProgressBar)
                {
                    border.MinWidth = 0;
                    panel.Children.Add(new ProgressRound
                    {
                        IText = false,
                        Margin = new Thickness(20),
                        Width = 100,
                        BorderThickness = new Thickness(1),
                    });
                }
                if (iProgressRound)
                {
                    panel.Children.Add(new Progress
                    {
                        Margin = new Thickness(10),
                        Width = 100,
                        Height = 100,
                    });
                }
                var progressAdorner = new CustomAdorner(element, border, AlphaColor(0, Colors.Black));
                lock (myAdornerLayer) myAdornerLayer.Add(progressAdorner);
                progressAdorner.Tag = myAdornerLayer;
                return progressAdorner;
            }
            return null;
        }
        /// <summary>
        /// 装饰器-关闭Window进度条
        /// </summary>
        public static void ProgressClose(CustomAdorner progress)
        {
            if (progress == null) return;
            BeginInvoke(() =>
            {
                if (progress.Tag is AdornerLayer adorner)
                {
                    adorner.Remove(progress);
                    progress.Tag = null;
                }
            });
        }
        /// <summary>
        /// 重复一次获取装饰器
        /// </summary>
        internal static AdornerLayer ReloadAdorner(UIElement element, int count = 1)
        {
            var myAdornerLayer = AdornerLayer.GetAdornerLayer(element);
            while (myAdornerLayer == null && count > 0)
            {
                DoEvents();
                myAdornerLayer = AdornerLayer.GetAdornerLayer(element);
                count--;
            }
            return myAdornerLayer;
        }

        #endregion
    }
}
