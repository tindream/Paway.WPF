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
    /// 自定义动画装饰器
    /// </summary>
    public class CustomAdorner : Adorner
    {
        private readonly Canvas canvas;
        /// <summary>
        /// 构造要将绑定到的装饰器的元素
        /// </summary>
        public CustomAdorner(FrameworkElement adornedElement, FrameworkElement element, Color? color = null, Func<double> xFunc = null, Func<double> yFunc = null, Func<Storyboard> storyboardFunc = null, bool? hitTest = null) : base(adornedElement)
        {
            //true:不路由事件（不穿透）
            //false:路由事件（穿透）
            if (hitTest == null) hitTest = storyboardFunc == null;
            IsHitTestVisible = hitTest.Value;
            canvas = new Canvas() { ClipToBounds = true };
            if (color != null) canvas.Background = color.Value.ToBrush();
            //添加到可视化树中
            var visCollec = new VisualCollection(this)
            {
                canvas
            };

            canvas.Children.Add(element);
            this.Loaded += (sender, e) =>
            {
                Canvas.SetLeft(element, xFunc != null ? xFunc() : (canvas.ActualWidth - element.ActualWidth) / 2);
                Canvas.SetTop(element, yFunc != null ? yFunc() : (canvas.ActualHeight - element.ActualHeight) / 2);
            };
            element.Loaded += (sender, e) =>
            {
                if (storyboardFunc == null) return;
                var storyboard = storyboardFunc();
                storyboard.CurrentTimeInvalidated += (sender2, e2) =>
                {
                    Canvas.SetLeft(element, xFunc != null ? xFunc() : (canvas.ActualWidth - element.ActualWidth) / 2);
                    Canvas.SetTop(element, yFunc != null ? yFunc() : (canvas.ActualHeight - element.ActualHeight) / 2);
                };
                storyboard.Completed += (sender2, e2) =>
                {
                    var myAdornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
                    if (myAdornerLayer != null)
                    {
                        myAdornerLayer.Remove(this);
                    }
                };
                storyboard.Begin(element);
            };
        }
        /// <summary>
        /// 获取画板
        /// </summary>
        public Canvas GetCanvas()
        {
            return canvas;
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
