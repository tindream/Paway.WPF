using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// 圆环水波纹装饰器
    /// </summary>
    public class EllipseAdorner : Adorner
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Color), typeof(EllipseAdorner), new UIPropertyMetadata(Color.FromArgb(255, 35, 175, 255)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register(nameof(LineColor), typeof(Color), typeof(EllipseAdorner), new UIPropertyMetadata(Colors.Transparent));
        /// <summary>
        /// 圆环水波纹背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("圆环水波纹背景颜色")]
        public Color Background
        {
            get { return (Color)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }
        /// <summary>
        /// 装饰器的元素的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("装饰器的元素的边框颜色")]
        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        #endregion

        private readonly double width;
        private readonly Canvas canvas;
        /// <summary>
        /// 构造要将绑定到的装饰器的元素
        /// </summary>
        public EllipseAdorner(FrameworkElement adornedElement, Point point) : base(adornedElement)
        {
            var x = Math.Max(adornedElement.ActualWidth - point.X, point.X);
            var y = Math.Max(adornedElement.ActualHeight - point.Y, point.Y);
            this.width = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)) * 2;
            canvas = new Canvas() { ClipToBounds = true };
            //添加到可视化树中
            var visCollec = new VisualCollection(this);
            visCollec.Add(canvas);

            var ellipse = new Ellipse() { Name = "adorner", Width = 10, Height = 10, Fill = new SolidColorBrush(Background) };
            canvas.Children.Add(ellipse);
            ellipse.Loaded += (sender, e) =>
            {
                var time = Method.AnimTime(this.width) * 1.5;
                var animation = new DoubleAnimation(10, this.width, new Duration(TimeSpan.FromMilliseconds(time)));
                var animation2 = new DoubleAnimation(10, this.width, new Duration(TimeSpan.FromMilliseconds(time)));
                animation.CurrentTimeInvalidated += (sender2, e2) =>
                {
                    var width = Math.Max(ellipse.Width, ellipse.Height);
                    Canvas.SetLeft(ellipse, point.X - width / 2);
                    Canvas.SetTop(ellipse, point.Y - width / 2);
                };
                animation.Completed += (sender2, e2) =>
                {
                    var colorAnm2 = new ColorAnimation((ellipse.Fill as SolidColorBrush).Color, Colors.Transparent, new Duration(TimeSpan.FromMilliseconds(30)));
                    colorAnm2.Completed += (sender3, e3) =>
                    {
                        var myAdornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
                        if (myAdornerLayer != null)
                        {
                            var list = myAdornerLayer.GetAdorners(adornedElement);
                            if (list != null && list.Count() > 0)
                            {
                                myAdornerLayer.Remove(list.First());
                            }
                        }
                    };
                    var solid = ellipse.Fill = (SolidColorBrush)ellipse.Fill.Clone();
                    solid.BeginAnimation(SolidColorBrush.ColorProperty, colorAnm2);
                };
                ellipse.BeginAnimation(FrameworkElement.WidthProperty, animation2);
                ellipse.BeginAnimation(FrameworkElement.HeightProperty, animation);


                var colorAnm = new ColorAnimation(Background, Color.FromArgb(20, Background.R, Background.G, Background.B), new Duration(TimeSpan.FromMilliseconds(300)));
                ellipse.Fill.BeginAnimation(SolidColorBrush.ColorProperty, colorAnm);
            };
        }
        /// <summary>
        /// 指定子元素
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            return canvas;
        }
        /// <summary>
        /// 获取此元素内可视子元素的数目
        /// </summary>
        protected override int VisualChildrenCount { get { return 1; } }
        /// <summary>
        /// 为控件指定位置和大小 
        /// </summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (canvas != null) canvas.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }
        /// <summary>
        /// 绘制边框
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            var rect = new Rect(this.AdornedElement.RenderSize);
            var pen = new Pen(new SolidColorBrush(LineColor), 1.0);
            drawingContext.DrawRectangle(new SolidColorBrush(Colors.Transparent), pen, rect);
        }
    }
}
