using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// TextBox扩展
    /// </summary>
    public partial class TextBoxEXT : TextBox
    {
        #region 动画
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IAnimationProperty =
            DependencyProperty.RegisterAttached(nameof(IAnimation), typeof(double), typeof(TextBoxEXT));
        /// <summary>
        /// 动画
        /// </summary>
        [Category("扩展")]
        [Description("动画")]
        public double IAnimation
        {
            get { return (double)GetValue(IAnimationProperty); }
            set { SetValue(IAnimationProperty, value); }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterProperty =
            DependencyProperty.RegisterAttached(nameof(Water), typeof(string), typeof(TextBoxEXT), new PropertyMetadata("请输入.."));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(TextBoxEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TextBoxEXT),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached(nameof(Icon), typeof(ImageSource), typeof(TextBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconStretchProperty =
            DependencyProperty.RegisterAttached(nameof(IconStretch), typeof(Stretch), typeof(TextBoxEXT),
            new PropertyMetadata(Stretch.None));

        #endregion

        #region 扩展
        /// <summary>
        /// 水印内容
        /// </summary>
        [Category("扩展")]
        [Description("水印内容")]
        public string Water
        {
            get { return (string)GetValue(WaterProperty); }
            set { SetValue(WaterProperty, value); }
        }
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 文本框的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("文本框的边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 图片
        /// </summary>
        [Category("扩展")]
        [Description("图片")]
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// 图片的内容如何拉伸才适合其磁贴
        /// </summary>
        [Category("扩展")]
        [Description("图片的内容如何拉伸才适合其磁贴")]
        public Stretch IconStretch
        {
            get { return (Stretch)GetValue(IconStretchProperty); }
            set { SetValue(IconStretchProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TextBoxEXT()
        {
            DefaultStyleKey = typeof(TextBoxEXT);
        }
        /// <summary>
        /// 响应滚动条
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_ContentHost") is ScrollViewerEXT scrollViewer)
            {
                scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
                scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
            }
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewerEXT;
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (e.Delta >> 2));
        }

        #region 动画
        /// <summary>
        /// 鼠标进入时启动
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (IAnimation > 0) Animation(true);
        }
        /// <summary>
        /// 鼠标离开时关闭
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IAnimation > 0) Animation(false);
        }
        private void Animation(bool value)
        {
            PMethod.Child(this, out Line line1, "line1", false);
            PMethod.Child(this, out Line line2, "line2", false);
            var storyboard = new Storyboard();
            if (value)
            {
                var animX1 = new DoubleAnimation(line1.X2, this.ActualWidth / 2 - BorderThickness.Right, new Duration(TimeSpan.FromMilliseconds(100)));
                var animX2 = new DoubleAnimation(line2.X2, this.ActualWidth / 2 - BorderThickness.Right, new Duration(TimeSpan.FromMilliseconds(100)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            else
            {
                var animX1 = new DoubleAnimation(line1.X2, 0, new Duration(TimeSpan.FromMilliseconds(100)));
                var animX2 = new DoubleAnimation(line2.X2, 0, new Duration(TimeSpan.FromMilliseconds(100)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            if (line1 != null) storyboard.Begin(line1);
        }

        #endregion
    }
}
