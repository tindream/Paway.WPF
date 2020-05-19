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
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached("ItemWidth", typeof(double), typeof(ListViewRound), new PropertyMetadata(90d));
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached("ItemHeight", typeof(double), typeof(ListViewRound), new PropertyMetadata(42d));
        public static readonly DependencyProperty ItemRadiusProperty =
            DependencyProperty.RegisterAttached("ItemRadius", typeof(RadiusRound), typeof(ListViewRound), new PropertyMetadata(new RadiusRound(5)));
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.RegisterAttached("ItemMargin", typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(1)));
        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.RegisterAttached("ItemPadding", typeof(ThicknessRound), typeof(ListViewRound), new PropertyMetadata(new ThicknessRound(0)));
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached("ItemBorder", typeof(ThicknessRound), typeof(ListViewRound), new PropertyMetadata(new ThicknessRound(1)));
        public static readonly DependencyProperty ItemBorderBrushProperty =
            DependencyProperty.RegisterAttached("ItemBorderBrush", typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound().Focused(Color.FromArgb(170, 35, 175, 255), 50)));
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemBackground", typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Color.FromArgb(255, 243, 243, 243), Color.FromArgb(120, 35, 175, 255), null, 50)));

        public static readonly DependencyProperty ItemImageWidthProperty =
            DependencyProperty.RegisterAttached("ItemImageWidth", typeof(double), typeof(ListViewRound), new PropertyMetadata(24d));
        public static readonly DependencyProperty ItemImageHeightProperty =
            DependencyProperty.RegisterAttached("ItemImageHeight", typeof(double), typeof(ListViewRound), new PropertyMetadata(24d));
        public static readonly DependencyProperty ItemImageDockProperty =
            DependencyProperty.RegisterAttached("ItemImageDock", typeof(Dock), typeof(ListViewRound), new PropertyMetadata(Dock.Left));
        public static readonly DependencyProperty ItemImageMarginProperty =
            DependencyProperty.RegisterAttached("ItemImageMargin", typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(0)));

        public static readonly DependencyProperty ItemTextPaddingProperty =
            DependencyProperty.RegisterAttached("ItemTextPadding", typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(0)));
        public static readonly DependencyProperty ItemTextForegroundProperty =
            DependencyProperty.RegisterAttached("ItemTextForeground", typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Red)));
        public static readonly DependencyProperty ItemTextBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemTextBackground", typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Transparent, null, null, 0)));
        public static readonly DependencyProperty ItemTextFontSizeProperty =
            DependencyProperty.RegisterAttached("ItemTextFontSize", typeof(DoubleRound), typeof(ListViewRound), new PropertyMetadata(new DoubleRound()));

        public static readonly DependencyProperty ItemDescDockProperty =
            DependencyProperty.RegisterAttached("ItemDescDock", typeof(Dock), typeof(ListViewRound), new PropertyMetadata(Dock.Right));
        public static readonly DependencyProperty ItemDescPaddingProperty =
            DependencyProperty.RegisterAttached("ItemDescPadding", typeof(Thickness), typeof(ListViewRound), new PropertyMetadata(new Thickness(0)));
        public static readonly DependencyProperty ItemDescForegroundProperty =
            DependencyProperty.RegisterAttached("ItemDescForeground", typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Red)));
        public static readonly DependencyProperty ItemDescBackgroundProperty =
            DependencyProperty.RegisterAttached("ItemDescBackground", typeof(BrushRound), typeof(ListViewRound), new PropertyMetadata(new BrushRound(Colors.Transparent, null, null, 0)));
        public static readonly DependencyProperty ItemDescFontSizeProperty =
            DependencyProperty.RegisterAttached("ItemDescFontSize", typeof(DoubleRound), typeof(ListViewRound), new PropertyMetadata(new DoubleRound(13)));

        #endregion

        #region 扩展.项
        [Category("扩展.项")]
        [Description("自定义项宽度")]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        [Category("扩展.项")]
        [Description("自定义项高度")]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        [Category("扩展.项")]
        [Description("自定义项圆角")]
        public RadiusRound ItemRadius
        {
            get { return (RadiusRound)GetValue(ItemRadiusProperty); }
            set { SetValue(ItemRadiusProperty, value); }
        }
        [Category("扩展.项")]
        [Description("自定义项外边距")]
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
        [Category("扩展.项")]
        [Description("自定义项内边距")]
        public ThicknessRound ItemPadding
        {
            get { return (ThicknessRound)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }
        [Category("扩展.项")]
        [Description("自定义项外边框")]
        public ThicknessRound ItemBorder
        {
            get { return (ThicknessRound)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        [Category("扩展.项")]
        [Description("自定义项外边框颜色")]
        public BrushRound ItemBorderBrush
        {
            get { return (BrushRound)GetValue(ItemBorderBrushProperty); }
            set { SetValue(ItemBorderBrushProperty, value); }
        }
        [Category("扩展.项")]
        [Description("自定义项背景颜色")]
        public BrushRound ItemBackground
        {
            get { return (BrushRound)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        #endregion

        #region 扩展.项图片
        [Category("扩展.项图片")]
        [Description("自定义项图片宽度")]
        public double ItemImageWidth
        {
            get { return (double)GetValue(ItemImageWidthProperty); }
            set { SetValue(ItemImageWidthProperty, value); }
        }
        [Category("扩展.项图片")]
        [Description("自定义项图片高度")]
        public double ItemImageHeight
        {
            get { return (double)GetValue(ItemImageHeightProperty); }
            set { SetValue(ItemImageHeightProperty, value); }
        }
        [Category("扩展.项图片")]
        [Description("自定义项图片位置")]
        public Dock ItemImageDock
        {
            get { return (Dock)GetValue(ItemImageDockProperty); }
            set { SetValue(ItemImageDockProperty, value); }
        }
        [Category("扩展.项图片")]
        [Description("自定义项图片外边距")]
        public Thickness ItemImageMargin
        {
            get { return (Thickness)GetValue(ItemImageMarginProperty); }
            set { SetValue(ItemImageMarginProperty, value); }
        }

        #endregion

        #region 扩展.项文本
        [Category("扩展.项文本")]
        [Description("自定义项文本内边距")]
        public Thickness ItemTextPadding
        {
            get { return (Thickness)GetValue(ItemTextPaddingProperty); }
            set { SetValue(ItemTextPaddingProperty, value); }
        }
        [Category("扩展.项文本")]
        [Description("自定义项文本字体颜色")]
        public BrushRound ItemTextForeground
        {
            get { return (BrushRound)GetValue(ItemTextForegroundProperty); }
            set { SetValue(ItemTextForegroundProperty, value); }
        }
        [Category("扩展.项文本")]
        [Description("自定义项文本背景颜色")]
        public BrushRound ItemTextBackground
        {
            get { return (BrushRound)GetValue(ItemTextBackgroundProperty); }
            set { SetValue(ItemTextBackgroundProperty, value); }
        }
        [Category("扩展.项文本")]
        [Description("自定义项文本字体大小")]
        public DoubleRound ItemTextFontSize
        {
            get { return (DoubleRound)GetValue(ItemTextFontSizeProperty); }
            set { SetValue(ItemTextFontSizeProperty, value); }
        }

        #endregion

        #region 扩展.项描述
        [Category("扩展.项描述")]
        [Description("自定义项描述位置")]
        public Dock ItemDescDock
        {
            get { return (Dock)GetValue(ItemDescDockProperty); }
            set { SetValue(ItemDescDockProperty, value); }
        }
        [Category("扩展.项描述")]
        [Description("自定义项描述内边距")]
        public Thickness ItemDescPadding
        {
            get { return (Thickness)GetValue(ItemDescPaddingProperty); }
            set { SetValue(ItemDescPaddingProperty, value); }
        }
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushRound ItemDescForeground
        {
            get { return (BrushRound)GetValue(ItemDescForegroundProperty); }
            set { SetValue(ItemDescForegroundProperty, value); }
        }
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushRound ItemDescBackground
        {
            get { return (BrushRound)GetValue(ItemDescBackgroundProperty); }
            set { SetValue(ItemDescBackgroundProperty, value); }
        }
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
