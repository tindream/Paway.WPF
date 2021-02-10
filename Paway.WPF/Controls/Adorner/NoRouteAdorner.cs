using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Paway.WPF
{
    /// <summary>
    /// 无路由动画装饰器
    /// </summary>
    public class NoRouteAdorner : Adorner
    {
        private readonly Canvas canvas;
        /// <summary>
        /// 构造要将绑定到的装饰器的元素
        /// </summary>
        public NoRouteAdorner(FrameworkElement adornedElement, Func<double> xFunc, Func<double> yFunc, Func<FrameworkElement> elementFunc, Func<Storyboard> boardFunc) : base(adornedElement)
        {
            //无路由事件
            IsHitTestVisible = false;
            canvas = new Canvas() { ClipToBounds = true };
            //添加到可视化树中
            var visCollec = new VisualCollection(this);
            visCollec.Add(canvas);

            FrameworkElement element = elementFunc();
            canvas.Children.Add(element);
            this.Loaded += (sender, e) =>
            {
                Canvas.SetLeft(element, xFunc != null ? xFunc() : (canvas.ActualWidth - element.ActualWidth) / 2);
                Canvas.SetTop(element, yFunc != null ? yFunc() : (canvas.ActualHeight - element.ActualHeight) / 2);
            };
            element.Loaded += (sender, e) =>
            {
                var board = boardFunc();
                board.CurrentTimeInvalidated += (sender2, e2) =>
                {
                    Canvas.SetLeft(element, xFunc != null ? xFunc() : (canvas.ActualWidth - element.ActualWidth) / 2);
                    Canvas.SetTop(element, yFunc != null ? yFunc() : (canvas.ActualHeight - element.ActualHeight) / 2);
                };
                board.Completed += (sender2, e2) =>
                {
                    var myAdornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
                    if (myAdornerLayer != null)
                    {
                        var list = myAdornerLayer.GetAdorners(adornedElement);
                        if (list != null)
                        {
                            myAdornerLayer.Remove(this);
                        }
                    }
                };
                board.Begin(element);
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
    }
}
