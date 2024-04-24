using Paway.Helper;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// ListView扩展
    /// </summary>
    public partial class ListViewEXT : ListViewCustom
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IAnimationProperty =
            DependencyProperty.RegisterAttached(nameof(IAnimation), typeof(bool), typeof(ListViewEXT));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageWidth), typeof(double), typeof(ListViewEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageHeightProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageHeight), typeof(double), typeof(ListViewEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageDockProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageDock), typeof(Dock), typeof(ListViewEXT), new PropertyMetadata(Dock.Top));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageMarginProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageMargin), typeof(ThicknessEXT), typeof(ListViewEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageStretch), typeof(Stretch), typeof(ListViewEXT),
            new PropertyMetadata(Stretch.None));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextPadding), typeof(Thickness), typeof(ListViewEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextBackground), typeof(BrushEXT), typeof(ListViewEXT),
                new PropertyMetadata(new BrushEXT(Colors.Transparent)));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescDockProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescDock), typeof(Dock), typeof(ListViewEXT), new PropertyMetadata(Dock.Right));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescPadding), typeof(Thickness), typeof(ListViewEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescForeground), typeof(BrushEXT), typeof(ListViewEXT),
                new PropertyMetadata(new BrushEXT(null, Colors.White, light: 60)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescBackground), typeof(BrushEXT), typeof(ListViewEXT),
                new PropertyMetadata(new BrushEXT(Colors.Transparent)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescFontSizeProperty =
        DependencyProperty.RegisterAttached(nameof(ItemDescFontSize), typeof(DoubleEXT), typeof(ListViewEXT), new PropertyMetadata(new DoubleEXT(PConfig.FontSize)));


        #endregion

        #region 扩展
        /// <summary>
        /// 动画
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("动画")]
        public bool IAnimation
        {
            get { return (bool)GetValue(IAnimationProperty); }
            set { SetValue(IAnimationProperty, value); }
        }

        #endregion
        #region 扩展.项图片
        /// <summary>
        /// 自定义项图片宽度
        /// <para>默认值：double.NaN</para>
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ItemImageWidth
        {
            get { return (double)GetValue(ItemImageWidthProperty); }
            set { SetValue(ItemImageWidthProperty, value); }
        }
        /// <summary>
        /// 自定义项图片高度
        /// <para>默认值：double.NaN</para>
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ItemImageHeight
        {
            get { return (double)GetValue(ItemImageHeightProperty); }
            set { SetValue(ItemImageHeightProperty, value); }
        }
        /// <summary>
        /// 自定义项图片位置
        /// <para>默认值：Top</para>
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片位置")]
        public Dock ItemImageDock
        {
            get { return (Dock)GetValue(ItemImageDockProperty); }
            set { SetValue(ItemImageDockProperty, value); }
        }
        /// <summary>
        /// 自定义项图片外边距
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片外边距")]
        public ThicknessEXT ItemImageMargin
        {
            get { return (ThicknessEXT)GetValue(ItemImageMarginProperty); }
            set { SetValue(ItemImageMarginProperty, value); }
        }
        /// <summary>
        /// 背景图片的内容如何拉伸才适合其磁贴
        /// <para>默认值：None</para>
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片的内容如何拉伸才适合其磁贴")]
        public Stretch ItemImageStretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        #endregion
        #region 扩展.项文本
        /// <summary>
        /// 自定义项文本内边距
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本内边距")]
        public Thickness ItemTextPadding
        {
            get { return (Thickness)GetValue(ItemTextPaddingProperty); }
            set { SetValue(ItemTextPaddingProperty, value); }
        }
        /// <summary>
        /// 自定义项文本背景颜色
        /// <para>默认值：Transparent</para>
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本背景颜色")]
        public BrushEXT ItemTextBackground
        {
            get { return (BrushEXT)GetValue(ItemTextBackgroundProperty); }
            set { SetValue(ItemTextBackgroundProperty, value); }
        }

        #endregion
        #region 扩展.项描述
        /// <summary>
        /// 自定义项描述位置
        /// <para>默认值：Right</para>
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述位置")]
        public Dock ItemDescDock
        {
            get { return (Dock)GetValue(ItemDescDockProperty); }
            set { SetValue(ItemDescDockProperty, value); }
        }
        /// <summary>
        /// 自定义项描述内边距
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述内边距")]
        public Thickness ItemDescPadding
        {
            get { return (Thickness)GetValue(ItemDescPaddingProperty); }
            set { SetValue(ItemDescPaddingProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体颜色
        /// <para>默认值：主题前景次色, White</para>
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushEXT ItemDescForeground
        {
            get { return (BrushEXT)GetValue(ItemDescForegroundProperty); }
            set { SetValue(ItemDescForegroundProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体颜色
        /// <para>默认值：Transparent</para>
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushEXT ItemDescBackground
        {
            get { return (BrushEXT)GetValue(ItemDescBackgroundProperty); }
            set { SetValue(ItemDescBackgroundProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体大小
        /// <para>默认值：主题字体大小</para>
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体大小")]
        public DoubleEXT ItemDescFontSize
        {
            get { return (DoubleEXT)GetValue(ItemDescFontSizeProperty); }
            set { SetValue(ItemDescFontSizeProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ListViewEXT()
        {
            DefaultStyleKey = typeof(ListViewEXT);
        }
        /// <summary>
        /// 更新字体大小
        /// </summary>
        protected override void Config_FontSizeChanged(double old)
        {
            base.Config_FontSizeChanged(old);
            if (this.ItemDescFontSize == null || this.ItemDescFontSize.Equals(new DoubleEXT(old * 0.85)))
            {
                this.ItemDescFontSize = new DoubleEXT(PConfig.FontSize * 0.85);
            }
        }

        #region 动画
        /// <summary>
        /// 判断按下状态
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (IAnimation && Mouse.DirectlyOver != null && PMethod.Parent(Mouse.DirectlyOver, out ListViewItem listViewItem) && this.moveItem != listViewItem)
            {
                if (this.moveItem != null) PMethod.Animation(moveItem, false);
                this.moveItem = listViewItem;
                PMethod.Animation(listViewItem, true);
            }
        }
        /// <summary>
        /// 鼠标离开控件
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IAnimation && this.moveItem != null)
            {
                PMethod.Animation(moveItem, false);
                this.moveItem = null;
            }
        }

        #endregion
    }
}
