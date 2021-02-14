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
    public class TMethod : Paway.Helper.TMethod
    {
        #region Adorner调用
        /// <summary>
        /// 点击触发水波纹装饰器
        /// </summary>
        public static AdornerLayer EllipseAdorner(MouseEventArgs e, double width = 0, double maxWidth = 300)
        {
            if (!(e.OriginalSource is FrameworkElement element)) return null;
            if (element is Adorner adorner)
            {//暂未使用
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
                else if (TMethod.Parent(textBlock, out ContentPresenter content) && content.Parent is FrameworkElement framework2)
                {
                    element = framework2;
                }
            }
            var myAdornerLayer = EllipseAdorner(element, e, null, width, maxWidth);
            if (myAdornerLayer == null)
            {
                if (!Parent(element, out Window window)) return null;
                if (window.Content is Panel panel)
                {
                    TMethod.EllipseAdorner(panel, e, null, width, maxWidth);
                }
            }
            return myAdornerLayer;
        }
        /// <summary>
        /// 点击触发水波纹装饰器
        /// </summary>
        public static AdornerLayer EllipseAdorner(FrameworkElement element, MouseEventArgs e, Color? color = null, double width = 0, double maxWidth = 300)
        {
            var myAdornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (myAdornerLayer == null) return null;

            var point = e.GetPosition(element);
            var x = Math.Max(element.ActualWidth - point.X, point.X);
            var y = Math.Max(element.ActualHeight - point.Y, point.Y);
            var autoWidth = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 2;
            if (width == 0 || width > autoWidth) width = autoWidth;
            if (maxWidth > 0 && width > maxWidth) width = maxWidth;

            if (color == null) color = Color.FromArgb(255, 35, 175, 255);
            var ellipse = new Ellipse() { Width = 10, Height = 10, Fill = new SolidColorBrush(color.Value) };
            myAdornerLayer.Add(new CustomAdorner(element, () => ellipse, null, () => point.X - ellipse.ActualWidth / 2, () => point.Y - ellipse.ActualHeight / 2, () =>
            {
                var storyboard = new Storyboard();
                var time = TMethod.AnimTime(width) * 1.3;

                var animWidth = new DoubleAnimation(10, width, new Duration(TimeSpan.FromMilliseconds(time)));
                Storyboard.SetTargetProperty(animWidth, new PropertyPath(FrameworkElement.WidthProperty));
                storyboard.Children.Add(animWidth);

                var animHeight = new DoubleAnimation(10, width, new Duration(TimeSpan.FromMilliseconds(time)));
                Storyboard.SetTargetProperty(animHeight, new PropertyPath(FrameworkElement.HeightProperty));
                storyboard.Children.Add(animHeight);


                var animColor = new ColorAnimation(color.Value, Color.FromArgb(10, color.Value.R, color.Value.G, color.Value.B), new Duration(TimeSpan.FromMilliseconds(300)));
                ellipse.Fill.BeginAnimation(SolidColorBrush.ColorProperty, animColor);
                var propertyChain = new DependencyProperty[] { Ellipse.FillProperty, SolidColorBrush.ColorProperty };
                Storyboard.SetTargetProperty(animColor, new PropertyPath("(0).(1)", propertyChain));
                storyboard.Children.Add(animColor);

                return storyboard;
            }));
            return myAdornerLayer;
        }

        #endregion

        #region 统一Invoke处理
        /// <summary>
        /// 同步调用
        /// </summary>
        public static void Invoke(DependencyObject obj, Action action, Action<Exception> error = null)
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
        /// <summary>
        /// 带参数同步调用
        /// </summary>
        public static void Invoke<T>(DependencyObject obj, Action<T> action, T t, Action<Exception> error = null)
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
        /// <summary>
        /// 异步调用
        /// </summary>
        public static void BeginInvoke(DependencyObject obj, Action action, Action<Exception> error = null)
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
        /// <summary>
        /// 带参数异步调用
        /// </summary>
        public static void BeginInvoke<T>(DependencyObject obj, Action<T> action, T t, Action<Exception> error = null)
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

        #endregion

        #region 位移动画
        /// <summary>
        /// 位移动画-从X轴左边进入(离开)
        /// </summary>
        /// <param name="content">控件</param>
        /// <param name="display">显示(进入)/隐藏(离开)</param>
        /// <param name="value">变化量</param>
        /// <param name="time">变化时间</param>
        public static double AnimMoveLeft(ContentPresenter content, double value = 0, double time = 0, bool display = true)
        {
            return AnimMove(content, display, 1, value, time, true);
        }
        /// <summary>
        /// 位移动画-从Y轴上边进入(离开)
        /// </summary>
        /// <param name="content">控件</param>
        /// <param name="display">显示(进入)/隐藏(离开)</param>
        /// <param name="value">变化量</param>
        /// <param name="time">变化时间</param>
        public static double AnimMoveUp(ContentPresenter content, double value = 0, double time = 0, bool display = true)
        {
            return AnimMove(content, display, -1, value, time, false);
        }
        /// <summary>
        /// 位移动画-从X轴右边进入(离开)
        /// </summary>
        /// <param name="content">控件</param>
        /// <param name="display">显示(进入)/隐藏(离开)</param>
        /// <param name="value">变化量</param>
        /// <param name="time">变化时间</param>
        public static double AnimMoveRight(ContentPresenter content, double value = 0, double time = 0, bool display = true)
        {
            return AnimMove(content, display, -1, value, time, true);
        }
        /// <summary>
        /// 位移动画-从Y轴下边进入(离开)
        /// </summary>
        /// <param name="content">控件</param>
        /// <param name="display">显示(进入)/隐藏(离开)</param>
        /// <param name="value">变化量</param>
        /// <param name="time">变化时间</param>
        public static double AnimMoveDown(ContentPresenter content, double value = 0, double time = 0, bool display = true)
        {
            return AnimMove(content, display, 1, value, time, false);
        }
        /// <summary>
        /// 位移动画
        /// </summary>
        /// <param name="content">控件</param>
        /// <param name="display">显示(进入)/隐藏(离开)</param>
        /// <param name="direction">反向标记</param>
        /// <param name="value">变化量</param>
        /// <param name="time">变化时间</param>
        /// <param name="x">变化量为0时取值依据</param>
        private static double AnimMove(ContentPresenter content, bool display = true, int direction = 1, double value = 0, double time = 0, bool x = true)
        {
            if (content == null) return 0;
            //实例化旋转对象（顺时针旋转）
            var tt = new TranslateTransform();
            //让content控件平移
            content.RenderTransform = tt;
            var animValue = value == 0 ? x ? content.ActualWidth : content.ActualHeight : value;
            var animTime = AnimTime(content, value, time, x);
            //创建动画处理对象
            var animXY = new DoubleAnimation(display ? animValue * direction : 0, display ? 0 : animValue * direction, new Duration(TimeSpan.FromMilliseconds(animTime)));
            //反向运动
            //animBy.AutoReverse = true;
            //无限循环
            //animBy.RepeatBehavior = RepeatBehavior.Forever;
            tt.BeginAnimation(x ? TranslateTransform.XProperty : TranslateTransform.YProperty, animXY);
            return animTime;
        }

        /// <summary>
        /// 透明度动画
        /// </summary>
        /// <param name="content">控件</param>
        /// <param name="display">显示/隐藏</param>
        /// <param name="completed">完成触发事件</param>
        /// <param name="value">变化量</param>
        /// <param name="time">变化时间</param>
        /// <param name="x">变化量为0时取值依据</param>
        public static double AnimOpacity(ContentPresenter content, double value = 0, double time = 0, Action completed = null, bool display = false, bool x = true)
        {
            if (content == null) return 0;
            var animTime = AnimTime(content, value, time, x);
            var animOpacity = new DoubleAnimation(display ? 0 : 1, display ? 1 : 0, new Duration(TimeSpan.FromMilliseconds(animTime)));
            animOpacity.Completed += delegate { completed?.Invoke(); };
            content.BeginAnimation(UIElement.OpacityProperty, animOpacity);
            return animTime;
        }
        private static double AnimTime(ContentPresenter content, double value = 0, double time = 0, bool x = true)
        {
            var animTime = time;
            if (animTime == 0) animTime = AnimTime(value == 0 ? x ? content.ActualWidth : content.ActualHeight : value);
            return animTime;
        }
        /// <summary>
        /// 计算动画时间
        /// </summary>
        public static double AnimTime(double value, int minTime = 250)
        {
            var animTime = (int)(Math.Pow(value, 1.0 / 4) * 100);
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
        internal static Color ThemeColor(int alpha)
        {
            return AlphaColor(alpha, TConfig.Color);
        }
        /// <summary>
        /// 指定Alpha颜色
        /// </summary>
        internal static Color AlphaColor(int alpha, Color color)
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
            BeginInvoke(parent, () =>
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
                        BeginInvoke(parent, () =>
                        {
                            ProgressClose(parent);
                            completed?.Invoke();
                        });
                    }
                });
                Progress(parent);
            });
        }
        /// <summary>
        /// 同步显示Window进度条
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
                    Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255)),
                    MinWidth = 200,
                    MaxWidth = 350,
                };
                var dp = new DockPanel();
                border.Child = dp;
                var textBlock = new TextBlock()
                {
                    Text = msg == null ? TConfig.Loading : msg.ToStrs(),
                    Padding = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextTrimming = TextTrimming.WordEllipsis,
                };
                dp.Children.Add(textBlock);
                DockPanel.SetDock(textBlock, Dock.Bottom);
                dp.Children.Add(new Progress
                {
                    Margin = new Thickness(20),
                    Width = 80,
                    Height = 80,
                });
                myAdornerLayer.Add(new CustomAdorner(panel, () => border, Color.FromArgb(10, 0, 0, 0)));
            }
        }
        /// <summary>
        /// 隐藏Window进度条
        /// </summary>
        public static void ProgressClose(DependencyObject parent)
        {
            if (!Parent(parent, out Window window)) return;
            if (window.Content is Panel panel)
            {
                var myAdornerLayer = AdornerLayer.GetAdornerLayer(panel);
                if (myAdornerLayer == null) return;
                var list = myAdornerLayer.GetAdorners(panel);
                if (list != null && list.Count() > 0)
                {
                    myAdornerLayer.Remove(list.First());
                }
            }
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
            //父级窗体原来的内容
            var original = owner.Content as UIElement;
            //将父级窗体原来的内容在容器Grid中移除
            owner.Content = null;
            //容器Grid
            var container = new Grid();
            //放入原来的内容
            container.Children.Add(original);
            //蒙板
            var layer = new Grid() { Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0)) };
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

        #region 自定义消息框-Toast
        /// <summary>
        /// 自定义消息框-Toast
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, bool iError = false, int pos = 17)
        {
            Toast(parent, msg, 0, iError, pos);
        }
        /// <summary>
        /// 自定义消息框-Toast
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, double time, bool iError = false, int pos = 17)
        {
            if (!Parent(parent, out Window window)) return;
            if (window.Content is Panel panel)
            {
                var myAdornerLayer = AdornerLayer.GetAdornerLayer(panel);
                if (myAdornerLayer == null) return;

                var color = iError ? Color.FromArgb(200, 221, 51, 51) : Color.FromArgb(200, 93, 107, 153);
                var border = new Border
                {
                    CornerRadius = new CornerRadius(5),
                    Background = new SolidColorBrush(color),
                };
                var block = new TextBlock()
                {
                    Text = msg.ToStrs(),
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
                myAdornerLayer.Add(new CustomAdorner(panel, () => border, null, null, () =>
                {
                    var top = window.ActualHeight - border.ActualHeight;
                    top = top * pos / 20;
                    return top;
                }, () =>
                {
                    var storyboard = new Storyboard();

                    var tt = new TranslateTransform();
                    border.RenderTransform = tt;
                    var animIn = new DoubleAnimation(-border.ActualHeight, 0, new Duration(TimeSpan.FromMilliseconds(125)));
                    var propertyY = new DependencyProperty[] { UIElement.RenderTransformProperty, TranslateTransform.YProperty };
                    Storyboard.SetTargetProperty(animIn, new PropertyPath("(0).(1)", propertyY));
                    storyboard.Children.Add(animIn);

                    var animTime = TMethod.AnimTime(border.ActualHeight);
                    var animColor = new ColorAnimation(color, Color.FromArgb(0, color.R, color.G, color.B), new Duration(TimeSpan.FromMilliseconds(animTime)))
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
            //Invoke(parent, () =>
            //{
            //    var toast = new WindowToast();
            //    if (Parent(parent, out Window owner))
            //    {
            //        var window = Method.Window(parent);
            //        {
            //            toast.Owner = owner;
            //        }
            //        toast.Show(msg, time, iError);
            //    }
            //});
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
            if (Parent(parent, out Window window))
            {
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
        internal static Tuple<T, I?, I?, I?, int?> ElementStatu<T, I>(ITypeDescriptorContext context, CultureInfo culture, string str,
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
            return new Tuple<T, I?, I?, I?, int?>(old, normal, mouse, pressed, alpha);
        }

        #endregion

        #endregion
    }
}
