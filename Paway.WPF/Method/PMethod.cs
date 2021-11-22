using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public class PMethod : TMethod
    {
        private static string NameWater = $"{nameof(PMethod)}_{nameof(WaterAdornerFixed)}";
        private static string NameProgress = $"{nameof(PMethod)}_{nameof(Progress)}";
        private static string NameHit = $"{nameof(PMethod)}_{nameof(Hit)}";

        #region 统一Invoke处理
        /// <summary>
        /// 同步调用
        /// </summary>
        public static void Invoke(DependencyObject obj, Action action, Action<Exception> error = null)
        {
            try
            {
                obj.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                });
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 带参数同步调用
        /// </summary>
        public static void Invoke<T>(DependencyObject obj, Action<T> action, T t, Action<Exception> error = null)
        {
            try
            {
                obj.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        action.Invoke(t);
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                });
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 异步调用
        /// </summary>
        public static void BeginInvoke(DependencyObject obj, Action action, Action<Exception> error = null)
        {
            try
            {
                obj.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                }));
            }
            catch (Exception) { }
        }
        /// <summary>
        /// 带参数异步调用
        /// </summary>
        public static void BeginInvoke<T>(DependencyObject obj, Action<T> action, T t, Action<Exception> error = null)
        {
            try
            {
                obj.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action.Invoke(t);
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                }));
            }
            catch (Exception) { }
        }

        #endregion

        #region 位移动画
        /// <summary>
        /// 计算动画时间
        /// </summary>
        public static double AnimTime(double value, int minTime = 250)
        {
            var animTime = (int)(Math.Pow(Math.Abs(value), 1.0 / 4) * 100);
            if (animTime < minTime) animTime = minTime;
            if (animTime > 1000) animTime = 1000;
            return animTime;
        }

        #endregion

        #region 获取控件模板的XAML代码
        /// <summary>
        /// 获取控件模板的XAML代码
        /// </summary>
        public static string GetTemplateXaml(Control ctrl)
        {
            string xaml;
            if (ctrl.Template != null)
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = new string(' ', 4),
                    NewLineOnAttributes = true
                };
                var strbuild = new StringBuilder();
                var xmlwrite = XmlWriter.Create(strbuild, settings);
                try
                {
                    XamlWriter.Save(ctrl.Template, xmlwrite);
                    xaml = strbuild.ToString();
                }
                catch (Exception ex)
                {
                    xaml = ex.Message;
                }
            }
            else
            {
                xaml = "no template";
            }
            return xaml;
        }

        #endregion

        #region 装饰器-自定义消息
        /// <summary>
        /// 装饰器-收到消息装入列表
        /// </summary>
        public static void Slowly(FrameworkElement parent, object msg, int time = 500, double xMove = 0, double yMove = 0, double size = 36, Color? color = null)
        {
            BeginInvoke(parent, () =>
            {
                var myAdornerLayer = AdornerLayer.GetAdornerLayer(parent);
                if (myAdornerLayer == null) return;

                var block = new TextBlock()
                {
                    Text = msg.ToStrings(),
                    FontSize = size
                };
                if (color != null) block.Foreground = new SolidColorBrush(color.Value);
                myAdornerLayer.Add(new CustomAdorner(parent, () => block, boardFunc: () =>
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
                }));
            });
        }

        private static FrameworkElement Element;
        /// <summary>
        /// 装饰器-移动触发水波纹底色
        /// </summary>
        public static AdornerLayer WaterAdornerFixed(FrameworkElement element, MouseEventArgs e, double width = 100)
        {
            var myAdornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (myAdornerLayer == null) return null;

            if (Element != null) ClearAdorner(AdornerLayer.GetAdornerLayer(Element), Element, NameWater);
            var point = e.GetPosition(element);
            var x = Math.Max(element.ActualWidth - point.X, point.X);
            var y = Math.Max(element.ActualHeight - point.Y, point.Y);
            var autoWidth = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 2;
            if (width == 0 || width > autoWidth) width = autoWidth;

            var ellipse = new Ellipse() { Width = width, Height = width, Fill = new SolidColorBrush(AlphaColor(20, Colors.Black)) };
            var waterAdornerFixed = new CustomAdorner(element, () => ellipse, null, () => point.X - ellipse.ActualWidth / 2, () => point.Y - ellipse.ActualHeight / 2, hitTest: false) { Name = NameWater };
            myAdornerLayer.Add(waterAdornerFixed);
            Element = element;
            return myAdornerLayer;
        }
        /// <summary>
        /// 清除装饰器上指定名称装饰
        /// </summary>
        private static void ClearAdorner(AdornerLayer myAdornerLayer, FrameworkElement element, string name)
        {
            if (myAdornerLayer == null) return;
            var list = myAdornerLayer.GetAdorners(element);
            while (list != null)
            {
                var last = list.FirstOrDefault(c => c.Name == name);
                if (last == null) break;
                myAdornerLayer.Remove(last);
                list = myAdornerLayer.GetAdorners(element);
            }
        }
        /// <summary>
        /// 装饰器-点击触发水波纹特效
        /// </summary>
        public static void WaterAdorner(MouseEventArgs e, double width = 0, double maxWidth = 300)
        {
            if (!(e.OriginalSource is FrameworkElement element)) return;
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
                if (window.Content is Panel panel)
                {
                    WaterAdorner(panel, e, null, width, maxWidth);
                }
            }
        }
        /// <summary>
        /// 装饰器-点击触发水波纹特效
        /// </summary>
        private static AdornerLayer WaterAdorner(FrameworkElement element, MouseEventArgs e, Color? color = null, double width = 0, double maxWidth = 300)
        {
            var myAdornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (myAdornerLayer == null) return null;

            var point = e.GetPosition(element);
            var x = Math.Max(element.ActualWidth - point.X, point.X);
            var y = Math.Max(element.ActualHeight - point.Y, point.Y);
            var autoWidth = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 2;
            if (width == 0 || width > autoWidth) width = autoWidth;
            if (maxWidth > 0 && width > maxWidth) width = maxWidth;

            if (color == null) color = PConfig.Color;
            var ellipse = new Ellipse() { Width = 10, Height = 10, Fill = new SolidColorBrush(color.Value) };
            myAdornerLayer.Add(new CustomAdorner(element, () => ellipse, null, () => point.X - ellipse.ActualWidth / 2, () => point.Y - ellipse.ActualHeight / 2, () =>
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
            }));
            return myAdornerLayer;
        }
        private static TextBlock tbProgress;
        /// <summary>
        /// 装饰器-同步显示Window进度条
        /// </summary>
        public static void Progress(DependencyObject parent, object msg = null)
        {
            if (!Parent(parent, out Window window)) return;
            if (window.Content is Panel panel)
            {
                var myAdornerLayer = AdornerLayer.GetAdornerLayer(panel);
                if (myAdornerLayer == null) return;

                var border = new Border
                {
                    CornerRadius = new CornerRadius(3),
                    BorderBrush = new SolidColorBrush(Colors.LightGray),
                    BorderThickness = new Thickness(1),
                    Background = new SolidColorBrush(AlphaColor(PConfig.Alpha, Colors.White)),
                    MinWidth = 200,
                    MaxWidth = 350,
                };
                var dp = new DockPanel();
                border.Child = dp;
                tbProgress = new TextBlock()
                {
                    Text = msg == null ? PConfig.Loading : msg.ToStrings(),
                    FontSize = 15,
                    Foreground = new SolidColorBrush(Colors.Black),
                    Padding = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextTrimming = TextTrimming.WordEllipsis,
                };
                dp.Children.Add(tbProgress);
                DockPanel.SetDock(tbProgress, Dock.Bottom);
                dp.Children.Add(new Progress
                {
                    Margin = new Thickness(20),
                    Width = 80,
                    Height = 80,
                });
                var progress = new CustomAdorner(panel, () => border, AlphaColor(0, Colors.Black)) { Name = NameProgress };
                myAdornerLayer.Add(progress);
            }
        }
        /// <summary>
        /// 装饰器-Window进度条提示信息
        /// </summary>
        public static void ProgressMsg(object msg = null)
        {
            if (tbProgress == null) return;
            BeginInvoke(tbProgress, () =>
            {
                tbProgress.Text = msg == null ? Paway.WPF.PConfig.Loading : msg.ToStrings();
            });
        }
        /// <summary>
        /// 装饰器-关闭Window进度条
        /// </summary>
        public static void ProgressClose(DependencyObject parent)
        {
            Invoke(parent, () =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is Panel panel)
                {
                    var myAdornerLayer = AdornerLayer.GetAdornerLayer(panel);
                    if (myAdornerLayer == null) return;
                    var list = myAdornerLayer.GetAdorners(panel);
                    if (list != null)
                    {
                        myAdornerLayer.Remove(list.FirstOrDefault(c => c.Name == NameProgress));
                    }
                }
            });
        }
        /// <summary>
        /// 装饰器-自定义吐泡消息框-Toast
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, ColorType type = ColorType.Color)
        {
            Toast(parent, msg, 0, type);
        }
        /// <summary>
        /// 装饰器-自定义吐泡消息框-Toast
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, int time, ColorType type = ColorType.Color, bool repeat = true)
        {
            BeginInvoke(parent, () =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is Panel panel)
                {
                    var myAdornerLayer = AdornerLayer.GetAdornerLayer(panel);
                    if (myAdornerLayer == null)
                    {
                        if (repeat)
                        {//未获取到装饰器层时重复一次
                            DoEvents();
                            Toast(parent, msg, time, type, false);
                        }
                        return;
                    }

                    var color = AlphaColor(PConfig.Alpha, type.Color());
                    var border = new Border
                    {
                        CornerRadius = new CornerRadius(5),
                        Background = new SolidColorBrush(color),
                    };
                    var block = new TextBlock()
                    {
                        Text = msg.ToStrings(),
                        Margin = new Thickness(8, 0, 8, 0),
                        Padding = new Thickness(20, 10, 20, 10),
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Colors.White),
                        MinWidth = 200,
                        MaxWidth = 600
                    };
                    border.Child = block;
                    if (time == 0) time = 3000;
                    myAdornerLayer.Add(new CustomAdorner(panel, () => border, yFunc: () =>
                    {
                        var top = window.ActualHeight - border.ActualHeight;
                        top = top * 17 / 20;
                        return top;
                    }, boardFunc: () =>
                    {
                        var storyboard = new Storyboard();

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
                    }));
                }
            });
            //Invoke(parent, () =>
            //{
            //    var toast = new WindowToast();
            //    if (Parent(parent, out Window owner))
            //    {
            //        var window = Window(parent);
            //        {
            //            toast.Owner = owner;
            //        }
            //        toast.Show(msg, time, iError);
            //    }
            //});
        }
        /// <summary>
        /// 装饰器-自定义提示框-Hit
        /// </summary>
        public static void Hit(DependencyObject parent, object msg, ColorType type = ColorType.Color)
        {
            Hit(parent, msg, 0, type);
        }
        /// <summary>
        /// 装饰器-自定义提示框-Hit
        /// </summary>
        public static void Hit(DependencyObject parent, object msg, int time, ColorType type = ColorType.Color, bool repeat = true)
        {
            BeginInvoke(parent, () =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is Panel panel)
                {
                    var myAdornerLayer = AdornerLayer.GetAdornerLayer(panel);
                    if (myAdornerLayer == null)
                    {
                        if (repeat)
                        {//未获取到装饰器层时重复一次
                            DoEvents();
                            Hit(parent, msg, time, type, false);
                        }
                        return;
                    }

                    var color = AlphaColor(PConfig.Alpha, type.Color());
                    var border = new Border
                    {
                        CornerRadius = new CornerRadius(5),
                        Background = new SolidColorBrush(color),
                    };
                    var block = new TextBlock()
                    {
                        Text = msg.ToStrings(),
                        Margin = new Thickness(8, 0, 8, 0),
                        Padding = new Thickness(20, 10, 20, 10),
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = new SolidColorBrush(Colors.White),
                        MinWidth = 200,
                        MaxWidth = 600
                    };
                    border.Child = block;
                    if (time == 0) time = 3000;

                    ClearAdorner(myAdornerLayer, panel, NameHit);
                    myAdornerLayer.Add(new CustomAdorner(panel, () => border, boardFunc: () =>
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
                    })
                    { Name = NameHit });
                }
            });
        }

        #endregion

        #region Window
        #region 让系统可以处理队列中的所有Windows消息
        /// <summary>
        /// 让系统可以处理队列中的所有Windows消息
        /// </summary>
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        private static Object ExitFrame(Object state)
        {
            ((DispatcherFrame)state).Continue = false;
            return null;
        }
        /// <summary>
        /// 刷新主题样式
        /// </summary>
        public static void DoStyles()
        {
            var list = new List<ResourceDictionary>();
            foreach (var item in Application.Current.Resources.MergedDictionaries) list.Add(item);

            Application.Current.Resources.MergedDictionaries.Clear();
            foreach (var item in list) Application.Current.Resources.MergedDictionaries.Add(item);
        }
        /// <summary>
        /// 取进度条中的颜色值
        /// </summary>
        /// <param name="offset"></param>
        public static Color ColorSelector(double offset)
        {
            if (!(Application.Current.FindResource("ColorSelector") is LinearGradientBrush linearGradientBrush)) return Colors.Transparent;

            var collection = linearGradientBrush.GradientStops;
            var stops = collection.OrderBy(x => x.Offset).ToArray();
            if (offset <= 0) return stops[0].Color;
            if (offset >= 1) return stops[stops.Length - 1].Color;
            var left = stops.Where(s => s.Offset <= offset).Last();
            var right = stops.Where(s => s.Offset > offset).First();
            offset = Math.Round((offset - left.Offset) / (right.Offset - left.Offset), 2);
            var a = (byte)((right.Color.A - left.Color.A) * offset + left.Color.A);
            var r = (byte)((right.Color.R - left.Color.R) * offset + left.Color.R);
            var g = (byte)((right.Color.G - left.Color.G) * offset + left.Color.G);
            var b = (byte)((right.Color.B - left.Color.B) * offset + left.Color.B);
            return Color.FromArgb(a, r, g, b);
        }
        /// <summary>
        /// 主题颜色
        /// </summary>
        internal static Color ThemeColor(int alpha = PConfig.Alpha)
        {
            return AlphaColor(alpha, PConfig.Color);
        }
        /// <summary>
        /// 指定Alpha颜色
        /// </summary>
        public static Color AlphaColor(int alpha, Color color)
        {
            if (color == Colors.Transparent) alpha = 0;
            if (alpha < 0) alpha = 0;
            else if (alpha > 255) alpha = 255;
            return Color.FromArgb((byte)alpha, color.R, color.G, color.B);
        }

        #endregion

        #region Window忙碌提示
        private static WindowProgress progress;
        /// <summary>
        /// 模式显示Window忙提示框，执行完成后关闭
        /// </summary>
        public static void Progress(DependencyObject parent, Action action, Action success = null, Action<Exception> error = null, Action completed = null)
        {
            Invoke(parent, () =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        action.Invoke();
                        if (success != null)
                        {
                            BeginInvoke(parent, () =>
                            {
                                success.Invoke();
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        if (error != null)
                        {
                            BeginInvoke(parent, () =>
                            {
                                error.Invoke(ex);
                            });
                        }
                        else
                        {
                            ex.Log();
                            Error(parent, ex.Message());
                        }
                    }
                    finally
                    {
                        ProgressClose(parent);
                        if (completed != null)
                        {
                            BeginInvoke(parent, () =>
                            {
                                completed.Invoke();
                            });
                        }
                    }
                });
                Progress(parent);
            });
        }
        /// <summary>
        /// 同步显示Window进度条(弹框模式)
        /// </summary>
        public static void ProgressWindow(DependencyObject parent, object msg = null)
        {
            if (progress == null) progress = new WindowProgress(msg);
            if (Parent(parent, out Window owner))
            {
                owner.Closed += delegate
                {
                    ProgressWindowClose(owner);
                };
                progress.Owner = owner;
                progress.ShowDialog();
            }
        }
        /// <summary>
        /// 隐藏Window进度条(弹框模式)
        /// </summary>
        public static void ProgressWindowClose(DependencyObject parent)
        {
            if (progress != null) progress.Close();
            progress = null;
            if (Parent(parent, out Window owner))
            {
                owner.Focus();
            }
        }

        #endregion

        #region Window弹框
        /// <summary>
        /// Window弹框
        /// </summary>
        public static bool? Show(DependencyObject parent, Window window, bool iEscExit = true)
        {
            if (!Parent(parent, out Window owner)) return null;
            window.ShowInTaskbar = false;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Closed += delegate
            {
                //容器Grid
                Grid grid = owner.Content as Grid;
                //父级窗体原来的内容
                var originalOld = VisualTreeHelper.GetChild(grid, 0) as UIElement;
                //将父级窗体原来的内容在容器Grid中移除
                grid.Children.Remove(originalOld);
                //赋给父级窗体
                owner.Content = originalOld;
            };
            window.LostKeyboardFocus += delegate
            {
                window.Focus();
            };
            if (!(window is WindowEXT))
            {
                //window.Loaded += delegate { window.Activate(); };
            }
            //父级窗体原来的内容
            var original = owner.Content as UIElement;
            //将父级窗体原来的内容在容器Grid中移除
            owner.Content = null;
            //容器Grid
            var container = new Grid();
            //放入原来的内容
            container.Children.Add(original);
            //蒙板
            var layer = new Grid() { Background = new SolidColorBrush(AlphaColor(100, Colors.Black)) };
            //在上面放一层蒙板
            container.Children.Add(layer);
            //将装有原来内容和蒙板的容器赋给父级窗体
            owner.Content = container;
            //弹出消息框 
            window.Owner = owner;
            if (iEscExit)
            {
                var iExit = false;
                var exitTime = DateTime.MinValue;
                var interval = NativeMethods.GetDoubleClickTime();
                window.KeyDown += delegate (object sender, System.Windows.Input.KeyEventArgs e)
                {
                    if (e.Key == System.Windows.Input.Key.Escape)
                    {
                        if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < interval)
                        {
                            window.Close();
                            return;
                        }
                        iExit = true;
                        exitTime = DateTime.Now;
                    }
                };
            }
            return window.ShowDialog();
        }

        #endregion

        #region Window系统消息框
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Information图标。</para>
        /// </summary>
        public static void Debug(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Debug);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Warning图标。</para>
        /// </summary>
        public static void Warning(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Warn);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Error图标。</para>
        /// </summary>
        public static void Error(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Error);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和指定图标。</para>
        /// </summary>
        public static void Show(DependencyObject parent, string msg, LeveType level = LeveType.Debug)
        {
            if (!Parent(parent, out Window window)) return;
            BeginInvoke(parent, obj =>
            {
                switch (level)
                {
                    case LeveType.Debug:
                    default:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case LeveType.Warn:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    case LeveType.Error:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }, msg);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OKCancel按钮和Question图标;它也会返回一个结果。</para>
        /// </summary>
        public static bool Ask(DependencyObject parent, string msg)
        {
            if (Parent(parent, out Window window))
            {
                return MessageBox.Show(window, msg, window.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
            }
            return false;
        }
        /// <summary>
        /// 返回控件的顶层Window
        /// </summary>
        public static Window Window(DependencyObject obj)
        {
            if (Parent(obj, out Window window)) return window;
            return null;
        }

        #endregion

        #region 返回指定控件的上下层控件
        /// <summary>
        /// 返回控件树中指定类型控件
        /// </summary>
        public static bool Find<T>(object obj, out T parent, string name = null) where T : FrameworkElement
        {
            parent = null;
            if (!(obj is DependencyObject dependency)) return false;
            if (Child(obj, out parent, name))
            {
                return true;
            }
            var hasParent = false;
            while (dependency != null)
            {
                if (dependency is T t)
                {
                    if (name == null || t.Name == name)
                    {
                        parent = t;
                        return true;
                    }
                }
                var temp = VisualTreeHelper.GetParent(dependency);
                if (temp == null) break;
                dependency = temp;
                hasParent = true;
            }
            if (hasParent && Child(dependency, out parent, name))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回控件的顶层指定类型控件
        /// </summary>
        public static bool Parent<T>(object obj, out T parent, string name = null) where T : FrameworkElement
        {
            if (obj is T t1)
            {
                if (name == null || t1.Name == name)
                {
                    parent = t1;
                    return true;
                }
            }
            parent = null;
            if (!(obj is DependencyObject dependency)) return false;
            dependency = VisualTreeHelper.GetParent(dependency);
            while (dependency != null)
            {
                if (dependency is T t)
                {
                    if (name == null || t.Name == name)
                    {
                        parent = t;
                        return true;
                    }
                }
                dependency = VisualTreeHelper.GetParent(dependency);
            }
            return false;
        }
        /// <summary>
        /// 查找指定类型子(同级)控件
        /// </summary>
        /// <typeparam name="T">查找控件类型</typeparam>
        /// <param name="obj">控件</param>
        /// <param name="child">返回指定类型控件</param>
        /// <param name="name">指定控件名称</param>
        /// <param name="iParent">指定搜索同级控件</param>
        /// <returns></returns>
        public static bool Child<T>(object obj, out T child, string name = null, bool iParent = true) where T : FrameworkElement
        {
            child = null;
            if (!(obj is DependencyObject dependency)) return false;
            if (iParent)
            {
                var parent = VisualTreeHelper.GetParent(dependency);
                if (parent != null) dependency = parent;
            }
            var count = VisualTreeHelper.GetChildrenCount(dependency);
            for (int i = 0; i < count; i++)
            {
                var value = VisualTreeHelper.GetChild(dependency, i);
                if (value is T temp)
                {
                    if (name == null || temp.Name == name)
                    {
                        child = temp;
                        return true;
                    }
                }
                if (Child(value, out child, name, false))
                {
                    return true;
                }
            }
            while (dependency is ContentControl control)
            {
                dependency = control.Content as DependencyObject;
                if (dependency is T temp)
                {
                    if (name == null || temp.Name == name)
                    {
                        child = temp;
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #endregion

        #region internal
        #region TypeConverter
        /// <summary>
        /// 获取ITypeDescriptorContext中的属性值
        /// </summary>
        internal static T GetValue<T>(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                var service = (IProvideValueTarget)context.GetService(typeof(IProvideValueTarget));
                if (service != null && service.TargetObject != null)
                {
                    var objType = service.TargetObject.GetType();
                    var obj = (DependencyObject)Activator.CreateInstance(objType);
                    var property = (DependencyProperty)service.TargetProperty;
                    return (T)obj.GetValue(property);
                }
            }
            return default;
        }
        /// <summary>
        /// 控件状态转换
        /// </summary>
        internal static ElementData<T, I> ElementStatu<T, I>(ITypeDescriptorContext context, CultureInfo culture, string str,
            Func<string, I> func, Func<T, string, I?> funcValue)
            //where T : IElementStatu<I> 
            where T : class
            where I : struct
        {
            var old = GetValue<T>(context);

            var strs = str.Split(';');
            I? normal = null;
            I? mouse = null;
            I? pressed = null;
            int? alpha = null;
            if (strs.Length > 0)
            {
                if (!string.IsNullOrEmpty(strs[0])) normal = func(strs[0]);
                else normal = funcValue(old, "Normal");
            }
            if (strs.Length > 1)
            {
                if (!string.IsNullOrEmpty(strs[1])) mouse = func(strs[1]);
                else mouse = funcValue(old, "Mouse");
            }
            if (strs.Length > 2)
            {
                if (!string.IsNullOrEmpty(strs[2])) pressed = func(strs[2]);
                else pressed = funcValue(old, "Pressed");
            }
            if (strs.Length > 3)
            {
                if (!string.IsNullOrEmpty(strs[3])) alpha = Convert.ToInt32(strs[3], culture);
                else if (old != null) alpha = (int)old.GetValue("Alpha");
            }
            return new ElementData<T, I>(old, normal, mouse, pressed, alpha);
        }

        #endregion

        #endregion
    }
}
