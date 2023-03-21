using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ComboBox扩展
    /// </summary>
    public partial class ComboBoxEXT : ComboBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ComboBoxEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(ComboBoxEXT),
            new PropertyMetadata(new BrushEXT(PConfig.TextColor, PConfig.TextColor, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ComboBoxEXT),
            new PropertyMetadata(new BrushEXT(null, PConfig.Alpha - PConfig.Interval * 2, PConfig.Alpha)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ComboBoxEXT),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached(nameof(Icon), typeof(ImageSource), typeof(ComboBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.RegisterAttached(nameof(IconWidth), typeof(double), typeof(ComboBoxEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.RegisterAttached(nameof(IconHeight), typeof(double), typeof(ComboBoxEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconStretchProperty =
            DependencyProperty.RegisterAttached(nameof(IconStretch), typeof(Stretch), typeof(ComboBoxEXT),
            new PropertyMetadata(Stretch.None));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterProperty =
            DependencyProperty.RegisterAttached(nameof(Water), typeof(string), typeof(ComboBoxEXT), new PropertyMetadata(PConfig.LanguageBase.PleaseInputWater));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached(nameof(WaterSize), typeof(double), typeof(ComboBoxEXT), new PropertyMetadata(0.85));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(ComboBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleMinWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TitleMinWidth), typeof(double), typeof(ComboBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IAnimationProperty =
            DependencyProperty.RegisterAttached(nameof(IAnimation), typeof(double), typeof(ComboBoxEXT));

        #endregion

        #region 扩展
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
        /// 项字体颜色
        /// </summary>
        [Category("扩展")]
        [Description("项字体颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }
        /// <summary>
        /// 项背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("项背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }
        /// <summary>
        /// 外边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("外边框颜色")]
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
        /// 图标宽度
        /// </summary>
        [Category("扩展")]
        [Description("图标宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        /// <summary>
        /// 图标高度
        /// </summary>
        [Category("扩展")]
        [Description("图标高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
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
        /// <summary>
        /// 输入框水印内容
        /// </summary>
        [Category("扩展")]
        [Description("输入框水印内容")]
        public string Water
        {
            get { return (string)GetValue(WaterProperty); }
            set { SetValue(WaterProperty, value); }
        }
        /// <summary>
        /// 水印字体大小系数
        /// </summary>
        [Category("扩展")]
        [Description("水印字体大小系数")]
        public double WaterSize
        {
            get { return (double)GetValue(WaterSizeProperty); }
            set { SetValue(WaterSizeProperty, value); }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [Category("扩展")]
        [Description("标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>
        /// 标题长度
        /// </summary>
        [Category("扩展")]
        [Description("标题长度")]
        public double TitleMinWidth
        {
            get { return (double)GetValue(TitleMinWidthProperty); }
            set { SetValue(TitleMinWidthProperty, value); }
        }
        /// <summary>
        /// 动画
        /// </summary>
        [Category("扩展")]
        [Description("IsEditable为true时，输入框的动画标记")]
        public double IAnimation
        {
            get { return (double)GetValue(IAnimationProperty); }
            set { SetValue(IAnimationProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ComboBoxEXT()
        {
            DefaultStyleKey = typeof(ComboBoxEXT);
        }
        /// <summary>
        /// Water未自定义设置时绑定多语言
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.Water != PConfig.LanguageBase.PleaseInputWater) return;
            var waterBinding = new Binding
            {
                Source = PConfig.LanguageBase,//设置要绑定源-语言类
                Path = new PropertyPath(nameof(PConfig.LanguageBase.PleaseInputWater)),//绑定绑定源下的属性。
                Mode = BindingMode.OneWay//绑定模式单向
            };
            this.SetBinding(WaterProperty, waterBinding);//设置绑定到要绑定的控件
        }
        /// <summary>
        /// 回车时移动焦点到下一控件
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            PMethod.OnKeyDown(e);
            base.OnKeyDown(e);
        }

        #region 动画
        /// <summary>
        /// 鼠标进入时启动
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (IAnimation > 0) PMethod.Animation(this, true, "line11", "line12");
        }
        /// <summary>
        /// 鼠标离开时关闭
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IAnimation > 0) PMethod.Animation(this, false, "line11", "line12");
        }

        #endregion
    }
}
