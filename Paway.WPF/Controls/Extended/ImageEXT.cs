using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paway.WPF
{
    /// <summary>
    /// ImageEXT扩展
    /// </summary>
    public partial class ImageEXT : ContentControl
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ImageEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(ImageEXT), new PropertyMetadata(Stretch.Uniform));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchDirectionProperty =
            DependencyProperty.Register(nameof(StretchDirection), typeof(StretchDirection), typeof(ImageEXT), new PropertyMetadata(StretchDirection.Both));

        #endregion

        #region 扩展
        /// <summary>
        /// 获取或设置标题
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>
        /// 获取或设置图像
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置图像")]
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        /// <summary>
        /// 获取或设置图像填充方式
        /// <para>默认值：Uniform</para>
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置图像填充方式")]
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        /// <summary>
        /// 获取或设置图像缩放方向
        /// <para>默认值：Both</para>
        /// </summary>
        [Category("扩展")]
        [Description("获取或设置图像缩放方向")]
        public StretchDirection StretchDirection
        {
            get { return (StretchDirection)GetValue(StretchDirectionProperty); }
            set { SetValue(StretchDirectionProperty, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 双击重置判断 - 时间
        /// </summary>
        private DateTime clickTime;
        /// <summary>
        /// 行双击路由事件
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> DoubleEvent;

        #endregion

        private TextBlock tbTitle;
        /// <summary>
        /// </summary>
        public ImageEXT()
        {
            DefaultStyleKey = typeof(ImageEXT);
        }
        /// <summary>
        /// 获取模板控件
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            tbTitle = Template.FindName("tbTitle", this) as TextBlock;
        }

        #region 拖拽节点
        /// <summary>
        /// 双击重置
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && DateTime.Now.Subtract(clickTime).TotalMilliseconds < PConfig.DoubleInterval)
            {
                if (DoubleEvent != null) DoubleEvent.Invoke(this, e);
                else if (PMethod.Parent(this, out Window window))
                {
                    PMethod.PImageFullAdorner(window, this.Title, this.Source.ToSource());
                }
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                clickTime = DateTime.Now;
            }
            base.OnPreviewMouseDown(e);
        }
        /// <summary>
        /// 鼠标移动时移动图像
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.tbTitle != null) tbTitle.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 鼠标离开时清空
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (this.tbTitle != null) tbTitle.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}
