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
        public CustomAdorner(FrameworkElement adornedElement, Func<FrameworkElement> elementFunc, Func<Tuple<DependencyProperty, AnimationTimeline>> animFunc) : base(adornedElement)
        {
            //路由事件
            IsHitTestVisible = false;
            canvas = new Canvas() { ClipToBounds = true };
            //添加到可视化树中
            var visCollec = new VisualCollection(this);
            visCollec.Add(canvas);

            FrameworkElement element = elementFunc();
            canvas.Children.Add(element);
            this.Loaded += (sender, e) =>
            {
                Canvas.SetLeft(element, canvas.ActualWidth / 2 - element.ActualWidth / 2);
                Canvas.SetTop(element, canvas.ActualHeight / 2 - element.ActualHeight / 2);
            };
            element.Loaded += (sender, e) =>
            {
                Tuple<DependencyProperty, AnimationTimeline> animation = animFunc();
                animation.Item2.CurrentTimeInvalidated += (sender2, e2) =>
                {
                    Canvas.SetLeft(element, canvas.ActualWidth / 2 - element.ActualWidth / 2);
                    Canvas.SetTop(element, canvas.ActualHeight / 2 - element.ActualHeight / 2);
                };
                animation.Item2.Completed += (sender2, e2) =>
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
                element.BeginAnimation(animation.Item1, animation.Item2);
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
