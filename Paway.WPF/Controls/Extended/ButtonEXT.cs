using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ButtonEXT扩展
    /// </summary>
    public partial class ButtonEXT : Button
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
                DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ButtonEXT), new PropertyMetadata(new CornerRadius(4)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessEXT), typeof(ButtonEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(ButtonEXT),
            new PropertyMetadata(new BrushEXT(Colors.White, Colors.White, Colors.White)));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ButtonEXT),
            new PropertyMetadata(new BrushEXT(PConfig.Alpha)));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ImageWidth), typeof(double), typeof(ButtonEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.RegisterAttached(nameof(ImageHeight), typeof(double), typeof(ButtonEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ImageDockProperty =
            DependencyProperty.RegisterAttached(nameof(ImageDock), typeof(Dock), typeof(ButtonEXT), new PropertyMetadata(Dock.Left));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ImageMarginProperty =
            DependencyProperty.RegisterAttached(nameof(ImageMargin), typeof(Thickness), typeof(ButtonEXT), new PropertyMetadata(new Thickness(0, 0, 2, 0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ImageStretchProperty =
            DependencyProperty.RegisterAttached(nameof(ImageStretch), typeof(Stretch), typeof(ButtonEXT),
            new PropertyMetadata(Stretch.None));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.RegisterAttached(nameof(Image), typeof(ImageEXT), typeof(ButtonEXT));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsLightProperty =
            DependencyProperty.RegisterAttached(nameof(IsLight), typeof(bool), typeof(ButtonEXT),
            new UIPropertyMetadata(false, OnColorTypeChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.RegisterAttached(nameof(Type), typeof(ColorType), typeof(ButtonEXT),
            new UIPropertyMetadata(ColorType.Color, OnColorTypeChanged));
        private static void OnColorTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonEXT view)
            {
                if (view.Type != ColorType.None)
                {
                    var color = view.Type.Color();
                    view.ItemBackground = new BrushEXT(PMethod.AlphaColor(PConfig.Alpha, color));
                }
                if (view.IsLight)
                {
                    view.ItemBorder = new ThicknessEXT(1, 0);
                    view.BorderBrush = Colors.LightGray.ToBrush();
                    view.ItemForeground = new BrushEXT(PConfig.TextColor, Colors.White, Colors.White);
                    view.ItemBackground.Normal = Colors.Transparent.ToBrush();
                }
                view.UpdateDefaultStyle();
            }
        }

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：4</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 自定义边框线
        /// <para>默认值：0</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框线")]
        public ThicknessEXT ItemBorder
        {
            get { return (ThicknessEXT)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        /// <summary>
        /// 自定义文本颜色
        /// <para>默认值：White, White, White</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义文本颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        /// <summary>
        /// 自定义背景颜色
        /// <para>默认值：200</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        /// <summary>
        /// 背景图片宽度
        /// <para>默认值：double.NaN</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景图片宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        /// <summary>
        /// 背景图片高度
        /// <para>默认值：double.NaN</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景图片高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }
        /// <summary>
        /// 背景图片位置
        /// <para>默认值：Left</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景图片位置")]
        public Dock ImageDock
        {
            get { return (Dock)GetValue(ImageDockProperty); }
            set { SetValue(ImageDockProperty, value); }
        }
        /// <summary>
        /// 背景图片外边距
        /// <para>默认值：0, 0, 2, 0</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景图片外边距")]
        public Thickness ImageMargin
        {
            get { return (Thickness)GetValue(ImageMarginProperty); }
            set { SetValue(ImageMarginProperty, value); }
        }
        /// <summary>
        /// 背景图片的内容如何拉伸才适合其磁贴
        /// <para>默认值：None</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景图片的内容如何拉伸才适合其磁贴")]
        public Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }
        /// <summary>
        /// 背景图片
        /// <para>默认值：无</para>
        /// </summary>
        [Category("扩展")]
        [Description("背景图片")]
        public ImageEXT Image
        {
            get { return (ImageEXT)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        /// <summary>
        /// 轻颜色样式
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("轻颜色样式")]
        public bool IsLight
        {
            get { return (bool)GetValue(IsLightProperty); }
            set { SetValue(IsLightProperty, value); }
        }
        /// <summary>
        /// 颜色样式
        /// <para>默认值：Color</para>
        /// </summary>
        [Category("扩展")]
        [Description("颜色样式")]
        public ColorType Type
        {
            get { return (ColorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ButtonEXT()
        {
            DefaultStyleKey = typeof(ButtonEXT);
        }
        /// <summary>
        /// 事件日志记录
        /// </summary>
        protected override void OnClick()
        {
            var desc = this.ToolTip.ToStrings();
            if (desc.IsEmpty()) desc = this.Content.ToStrings();
            PConfig.AddLog(this, desc);
            base.OnClick();
        }
    }
}
