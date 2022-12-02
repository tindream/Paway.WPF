using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private static FrameworkElement Element;
        private static readonly string NameWater = $"{nameof(PMethod)}_{nameof(WaterAdornerFixed)}";
        private static readonly string NameHit = $"{nameof(PMethod)}_{nameof(Hit)}";

        #region 装饰器-特效
        /// <summary>
        /// 装饰器-收到消息装入列表
        /// </summary>
        public static void SlowIn(FrameworkElement parent, object msg, int time = 500, double xMove = 0, double yMove = 0, double size = 36, Color? color = null)
        {
            Invoke(parent, () =>
            {
                var myAdornerLayer = ReloadAdorner(parent);
                if (myAdornerLayer == null) return;

                var block = new TextBlock()
                {
                    Text = msg.ToStrings(),
                    FontSize = size
                };
                if (color != null) block.Foreground = color.Value.ToBrush();
                myAdornerLayer.Add(new CustomAdorner(parent, block, storyboardFunc: () =>
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

            var ellipse = new Ellipse() { Width = width, Height = width, Fill = AlphaColor(20, Colors.Black).ToBrush() };
            var waterAdornerFixed = new CustomAdorner(element, ellipse, null, () => point.X - ellipse.ActualWidth / 2, () => point.Y - ellipse.ActualHeight / 2, hitTest: false) { Name = NameWater };
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
            var myAdornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (myAdornerLayer == null) return null;

            var point = e.GetPosition(element);
            var x = Math.Max(element.ActualWidth - point.X, point.X);
            var y = Math.Max(element.ActualHeight - point.Y, point.Y);
            var autoWidth = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 2;
            if (width == 0 || width > autoWidth) width = autoWidth;
            if (maxWidth > 0 && width > maxWidth) width = maxWidth;

            if (color == null) color = PConfig.Color;
            var ellipse = new Ellipse() { Width = 10, Height = 10, Fill = color.Value.ToBrush() };
            myAdornerLayer.Add(new CustomAdorner(element, ellipse, null, () => point.X - ellipse.ActualWidth / 2, () => point.Y - ellipse.ActualHeight / 2, () =>
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

        #endregion

        #region 装饰器-提示信息
        /// <summary>
        /// 装饰器-自定义吐泡消息框-Toast
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, ColorType type = ColorType.Color, int? fontSize = null)
        {
            Toast(parent, msg, 0, type, fontSize);
        }
        /// <summary>
        /// 装饰器-自定义吐泡消息框-Toast
        /// </summary>
        public static void Toast(DependencyObject parent, object msg, int time, ColorType type = ColorType.Color, int? fontSize = null)
        {
            Invoke(parent, () =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is FrameworkElement element)
                {
                    var myAdornerLayer = ReloadAdorner(element);
                    if (myAdornerLayer == null) return;

                    var color = AlphaColor(PConfig.Alpha, type.Color());
                    var border = new Border
                    {
                        CornerRadius = new CornerRadius(5),
                        Background = color.ToBrush(),
                    };
                    var block = new TextBlock()
                    {
                        Text = msg.ToStrings(),
                        Margin = new Thickness(8, 0, 8, 0),
                        Padding = new Thickness(20, 10, 20, 10),
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Colors.White.ToBrush(),
                        MinWidth = 200,
                        MaxWidth = 600
                    };
                    if (fontSize != null) block.FontSize = fontSize.Value;
                    border.Child = block;
                    if (time == 0) time = 3000;
                    myAdornerLayer.Add(new CustomAdorner(element, border, yFunc: () =>
                    {
                        var top = window.ActualHeight - border.ActualHeight;
                        top = top * 17 / 20;
                        return top;
                    }, storyboardFunc: () =>
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
        public static void Hit(DependencyObject parent, object msg, ColorType type = ColorType.Color, int? fontSize = null)
        {
            Hit(parent, msg, 0, type, fontSize);
        }
        /// <summary>
        /// 装饰器-自定义提示框-Hit
        /// </summary>
        public static void Hit(DependencyObject parent, object msg, int time, ColorType type = ColorType.Color, int? fontSize = null)
        {
            Invoke(parent, () =>
            {
                if (!Parent(parent, out Window window)) return;
                if (window.Content is FrameworkElement element)
                {
                    var myAdornerLayer = ReloadAdorner(element);
                    if (myAdornerLayer == null) return;

                    var color = AlphaColor(PConfig.Alpha, type.Color());
                    var border = new Border
                    {
                        CornerRadius = new CornerRadius(5),
                        Background = color.ToBrush(),
                    };
                    var block = new TextBlock()
                    {
                        Text = msg.ToStrings(),
                        Margin = new Thickness(8, 0, 8, 0),
                        Padding = new Thickness(20, 10, 20, 10),
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
                    myAdornerLayer.Add(new CustomAdorner(element, border, storyboardFunc: () =>
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

        #region 装饰器-Window忙碌提示
        /// <summary>
        /// 模式显示Window忙提示框，执行完成后关闭
        /// </summary>
        public static void Progress(DependencyObject parent, Action action, Action success = null, Action<Exception> error = null, Action completed = null, bool iProgressBar = false, int? fontSize = null)
        {
            Progress(parent, null, adorner => action?.Invoke(), success, error, completed, iProgressBar, fontSize);
        }
        /// <summary>
        /// 模式显示Window忙提示框，执行完成后关闭
        /// </summary>
        public static void Progress(DependencyObject parent, Action<CustomAdorner> action, Action success = null, Action<Exception> error = null, Action completed = null, bool iProgressBar = false, int? fontSize = null)
        {
            Progress(parent, null, action, success, error, completed, iProgressBar, fontSize);
        }
        /// <summary>
        /// 模式显示Window忙提示框，执行完成后关闭
        /// </summary>
        public static void Progress(DependencyObject parent, object msg, Action<CustomAdorner> action, Action success = null, Action<Exception> error = null, Action completed = null, bool iProgressBar = false, int? fontSize = null)
        {
            BeginInvoke(parent, () =>
            {
                var progress = ProgressAdorner(parent, msg, iProgressBar, fontSize);
                Task.Run(() =>
                {
                    try
                    {
                        action.Invoke(progress);
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
                            ShowError(parent, ex.Message());
                        }
                    }
                    finally
                    {
                        ProgressClose(parent, progress);
                        if (completed != null)
                        {
                            BeginInvoke(parent, () =>
                            {
                                completed.Invoke();
                            }, error);
                        }
                    }
                });
            }, error);
        }
        /// <summary>
        /// 装饰器-同步显示Window进度条
        /// </summary>
        public static CustomAdorner ProgressAdorner(DependencyObject parent, object msg = null, bool iProgressBar = false, int? fontSize = null)
        {
            if (!Parent(parent, out Window window)) return null;
            if (window.Content is FrameworkElement element)
            {
                var myAdornerLayer = ReloadAdorner(element);
                if (myAdornerLayer == null) return null;

                var border = new Border
                {
                    CornerRadius = new CornerRadius(3),
                    BorderBrush = Colors.LightGray.ToBrush(),
                    BorderThickness = new Thickness(1),
                    Background = AlphaColor(PConfig.Alpha, Colors.White).ToBrush(),
                    MinWidth = 200,
                    MaxWidth = 350,
                };
                var dp = new DockPanel();
                border.Child = dp;
                var tbProgress = new TextBlock()
                {
                    Text = msg == null ? PConfig.Loading : msg.ToStrings(),
                    Foreground = Colors.Black.ToBrush(),
                    Padding = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextTrimming = TextTrimming.WordEllipsis,
                };
                if (fontSize != null) tbProgress.FontSize = fontSize.Value;
                dp.Children.Add(tbProgress);
                DockPanel.SetDock(tbProgress, Dock.Bottom);
                if (iProgressBar)
                {
                    var progressBar = new ProgressBarEXT
                    {
                        IText = false,
                        Margin = new Thickness(0),
                        Radius = new CornerRadius(0),
                    };
                    dp.Children.Add(progressBar);
                    DockPanel.SetDock(progressBar, Dock.Bottom);
                }
                dp.Children.Add(new Progress
                {
                    Margin = new Thickness(20),
                    Width = 80,
                    Height = 80,
                });
                var progressAd = new CustomAdorner(element, border, AlphaColor(0, Colors.Black));
                myAdornerLayer.Add(progressAd);
                progressAd.Tag = myAdornerLayer;
                return progressAd;
            }
            return null;
        }
        /// <summary>
        /// 装饰器-关闭Window进度条
        /// </summary>
        public static void ProgressClose(DependencyObject parent, CustomAdorner progress)
        {
            if (progress == null) return;
            BeginInvoke(parent, () =>
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
        private static AdornerLayer ReloadAdorner(UIElement element, int count = 1)
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
