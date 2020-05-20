using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ListView扩展
    /// </summary>
    public partial class ListViewRound : ListView
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ItemWidth), typeof(double), typeof(ListViewRound), new PropertyMetadata(90d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached(nameof(ItemHeight), typeof(double), typeof(ListViewRound), new PropertyMetadata(42d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(ItemRadius), typeof(RadiusRound), typeof(ListViewRound), new PropertyMetadata(new RadiusRound(5)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.RegisterAttached(nameof(ItemMargin), typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(1)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemPadding), typeof(ThicknessRound), typeof(ListViewRound), new PropertyMetadata(new ThicknessRound(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessRound), typeof(ListViewRound), new PropertyMetadata(new ThicknessRound(1)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorderBrush), typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound().Focused(Color.FromArgb(170, 35, 175, 255))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Color.FromArgb(255, 243, 243, 243), Color.FromArgb(120, 35, 175, 255))));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageWidth), typeof(double), typeof(ListViewRound), new PropertyMetadata(24d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageHeightProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageHeight), typeof(double), typeof(ListViewRound), new PropertyMetadata(24d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageDockProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageDock), typeof(Dock), typeof(ListViewRound), new PropertyMetadata(Dock.Left));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageMarginProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageMargin), typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextPadding), typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextForeground), typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Black, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextBackground), typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Transparent, null, null, 0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextFontSizeProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextFontSize), typeof(DoubleRound), typeof(ListViewRound), new PropertyMetadata(new DoubleRound()));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescDockProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescDock), typeof(Dock), typeof(ListViewRound), new PropertyMetadata(Dock.Right));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescPadding), typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescForeground), typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Black, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescBackground), typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Transparent, null, null, 0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescFontSizeProperty =
        DependencyProperty.RegisterAttached(nameof(ItemDescFontSize), typeof(DoubleRound), typeof(ListViewRound), new PropertyMetadata(new DoubleRound(13)));

        #endregion

        #region 扩展.项
        /// <summary>
        /// 自定义项宽度
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项宽度")]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        /// <summary>
        /// 自定义项高度
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项高度")]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        /// <summary>
        /// 自定义项圆角
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项圆角")]
        public RadiusRound ItemRadius
        {
            get { return (RadiusRound)GetValue(ItemRadiusProperty); }
            set { SetValue(ItemRadiusProperty, value); }
        }
        /// <summary>
        /// 自定义项外边距
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项外边距")]
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
        /// <summary>
        /// 自定义项内边距
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项内边距")]
        public ThicknessRound ItemPadding
        {
            get { return (ThicknessRound)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }
        /// <summary>
        /// 自定义项外边框
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项外边框")]
        public ThicknessRound ItemBorder
        {
            get { return (ThicknessRound)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        /// <summary>
        /// 自定义项外边框颜色
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项外边框颜色")]
        public BrushRound ItemBorderBrush
        {
            get { return (BrushRound)GetValue(ItemBorderBrushProperty); }
            set { SetValue(ItemBorderBrushProperty, value); }
        }
        /// <summary>
        /// 自定义项背景颜色
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项背景颜色")]
        public BrushRound ItemBackground
        {
            get { return (BrushRound)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        #endregion
        #region 扩展.项图片
        /// <summary>
        /// 自定义项图片宽度
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片宽度")]
        public double ItemImageWidth
        {
            get { return (double)GetValue(ItemImageWidthProperty); }
            set { SetValue(ItemImageWidthProperty, value); }
        }
        /// <summary>
        /// 自定义项图片高度
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片高度")]
        public double ItemImageHeight
        {
            get { return (double)GetValue(ItemImageHeightProperty); }
            set { SetValue(ItemImageHeightProperty, value); }
        }
        /// <summary>
        /// 自定义项图片位置
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
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片外边距")]
        public Thickness ItemImageMargin
        {
            get { return (Thickness)GetValue(ItemImageMarginProperty); }
            set { SetValue(ItemImageMarginProperty, value); }
        }

        #endregion
        #region 扩展.项文本
        /// <summary>
        /// 自定义项文本内边距
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本内边距")]
        public Thickness ItemTextPadding
        {
            get { return (Thickness)GetValue(ItemTextPaddingProperty); }
            set { SetValue(ItemTextPaddingProperty, value); }
        }
        /// <summary>
        /// 自定义项文本字体颜色
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本字体颜色")]
        public BrushRound ItemTextForeground
        {
            get { return (BrushRound)GetValue(ItemTextForegroundProperty); }
            set { SetValue(ItemTextForegroundProperty, value); }
        }
        /// <summary>
        /// 自定义项文本背景颜色
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本背景颜色")]
        public BrushRound ItemTextBackground
        {
            get { return (BrushRound)GetValue(ItemTextBackgroundProperty); }
            set { SetValue(ItemTextBackgroundProperty, value); }
        }
        /// <summary>
        /// 自定义项文本字体大小
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本字体大小")]
        public DoubleRound ItemTextFontSize
        {
            get { return (DoubleRound)GetValue(ItemTextFontSizeProperty); }
            set { SetValue(ItemTextFontSizeProperty, value); }
        }

        #endregion
        #region 扩展.项描述
        /// <summary>
        /// 自定义项描述位置
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
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushRound ItemDescForeground
        {
            get { return (BrushRound)GetValue(ItemDescForegroundProperty); }
            set { SetValue(ItemDescForegroundProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体颜色
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushRound ItemDescBackground
        {
            get { return (BrushRound)GetValue(ItemDescBackgroundProperty); }
            set { SetValue(ItemDescBackgroundProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体大小
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体大小")]
        public DoubleRound ItemDescFontSize
        {
            get { return (DoubleRound)GetValue(ItemDescFontSizeProperty); }
            set { SetValue(ItemDescFontSizeProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ListViewRound() { }
    }
}
