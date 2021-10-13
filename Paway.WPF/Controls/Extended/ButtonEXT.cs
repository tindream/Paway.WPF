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
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessEXT), typeof(ButtonEXT), new PropertyMetadata(new ThicknessEXT(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty EffectRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(EffectRadius), typeof(double), typeof(ButtonEXT), new PropertyMetadata(0d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(ButtonEXT),
            new PropertyMetadata(new BrushEXT(Colors.White, Colors.White, Colors.White)));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ButtonEXT),
            new PropertyMetadata(new BrushEXT(160)));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.RegisterAttached(nameof(Viewport), typeof(Rect), typeof(ButtonEXT),
            new PropertyMetadata(new Rect(0, 0, 1, 1)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.RegisterAttached(nameof(Stretch), typeof(Stretch), typeof(ButtonEXT),
            new PropertyMetadata(Stretch.None));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundImage), typeof(ImageEXT), typeof(ButtonEXT));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsLightProperty =
            DependencyProperty.RegisterAttached(nameof(IsLight), typeof(bool), typeof(ButtonEXT),
            new UIPropertyMetadata(false, OnColorTypeChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColorTypeProperty =
            DependencyProperty.RegisterAttached(nameof(ColorType), typeof(ColorType), typeof(ButtonEXT),
            new UIPropertyMetadata(ColorType.None, OnColorTypeChanged));
        private static void OnColorTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonEXT btn)
            {
                if (btn.ColorType != ColorType.None)
                {
                    var color = btn.ColorType.Color();
                    btn.ItemBackground = new BrushEXT(PMethod.AlphaColor(160, color));
                }
                if (btn.IsLight)
                {
                    btn.ItemBorder = new ThicknessEXT(1, 0);
                    btn.BorderBrush = new SolidColorBrush(Colors.LightGray);
                    btn.ItemForeground = new BrushEXT(PConfig.TextColor, Colors.White, Colors.White);
                    btn.ItemBackground.Normal = new SolidColorBrush(Colors.White);
                }
                btn.UpdateDefaultStyle();
            }
        }

        #endregion

        #region 扩展.前景
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展.前景")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 自定义边框线
        /// </summary>
        [Category("扩展.前景")]
        [Description("自定义边框线")]
        public ThicknessEXT ItemBorder
        {
            get { return (ThicknessEXT)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        /// <summary>
        /// 自定义边框圆角阴影宽度
        /// </summary>
        [Category("扩展.前景")]
        [Description("自定义边框圆角阴影宽度")]
        public double EffectRadius
        {
            get { return (double)GetValue(EffectRadiusProperty); }
            set { SetValue(EffectRadiusProperty, value); }
        }
        /// <summary>
        /// 自定义文本颜色
        /// </summary>
        [Category("扩展.前景")]
        [Description("自定义文本颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        #endregion
        #region 扩展.背景颜色
        /// <summary>
        /// 自定义背景颜色
        /// </summary>
        [Category("扩展.背景颜色")]
        [Description("自定义背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        #endregion
        #region 扩展.背景图片
        /// <summary>
        /// 背景图片位置
        /// </summary>
        [Category("扩展.背景图片")]
        [Description("背景图片位置")]
        public Rect Viewport
        {
            get { return (Rect)GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }
        /// <summary>
        /// 背景图片的内容如何拉伸才适合其磁贴
        /// </summary>
        [Category("扩展.背景图片")]
        [Description("背景图片的内容如何拉伸才适合其磁贴")]
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        /// <summary>
        /// 背景图片
        /// </summary>
        [Category("扩展.背景图片")]
        [Description("背景图片")]
        public ImageEXT BackgroundImage
        {
            get { return (ImageEXT)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        #endregion
        #region 扩展.颜色样式
        /// <summary>
        /// 轻颜色样式
        /// </summary>
        [Category("扩展.轻颜色样式")]
        [Description("轻颜色样式")]
        public bool IsLight
        {
            get { return (bool)GetValue(IsLightProperty); }
            set { SetValue(IsLightProperty, value); }
        }
        /// <summary>
        /// 颜色样式
        /// </summary>
        [Category("扩展.颜色样式")]
        [Description("颜色样式")]
        public ColorType ColorType
        {
            get { return (ColorType)GetValue(ColorTypeProperty); }
            set { SetValue(ColorTypeProperty, value); }
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
