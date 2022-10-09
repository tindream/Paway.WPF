using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// PasswordBox扩展
    /// </summary>
    public class PasswordBoxEXT
    {
        #region 密码
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(nameof(Password), typeof(string), typeof(PasswordBoxEXT), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged));
        /// <summary>
        /// </summary>
        internal static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached(nameof(IsUpdating), typeof(bool), typeof(PasswordBoxEXT));
        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is PasswordBox pad)
            {
                if (!GetIsUpdating(pad))
                {
                    pad.Password = (string)e.NewValue;
                }
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        [Category("扩展")]
        [Description("密码")]
        public string Password { get; set; }
        /// <summary>
        /// get密码
        /// </summary>
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }
        /// <summary>
        /// set密码
        /// </summary>
        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// 密码扩展更新
        /// </summary>
        internal bool IsUpdating { get; set; }
        /// <summary>
        /// get密码扩展更新
        /// </summary>
        internal static bool GetIsUpdating(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsUpdatingProperty);
        }
        /// <summary>
        /// set密码扩展更新
        /// </summary>
        internal static void SetIsUpdating(DependencyObject obj, bool value)
        {
            obj.SetValue(IsUpdatingProperty, value);
        }

        #endregion

        #region 动画
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IAnimationProperty =
            DependencyProperty.RegisterAttached(nameof(IAnimation), typeof(double), typeof(PasswordBoxEXT));
        /// <summary>
        /// 动画
        /// </summary>
        [Category("扩展")]
        [Description("动画")]
        public double IAnimation { get; set; }
        /// <summary>
        /// get动画
        /// </summary>
        public static double GetIAnimation(DependencyObject obj)
        {
            return (double)obj.GetValue(IAnimationProperty);
        }
        /// <summary>
        /// set动画
        /// </summary>
        public static void SetIAnimation(DependencyObject obj, double value)
        {
            obj.SetValue(IAnimationProperty, value);
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(PasswordBoxEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(PasswordBoxEXT),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached(nameof(Icon), typeof(ImageSource), typeof(PasswordBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.RegisterAttached(nameof(IconWidth), typeof(double), typeof(PasswordBoxEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.RegisterAttached(nameof(IconHeight), typeof(double), typeof(PasswordBoxEXT), new PropertyMetadata(double.NaN));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconStretchProperty =
            DependencyProperty.RegisterAttached(nameof(IconStretch), typeof(Stretch), typeof(PasswordBoxEXT),
            new PropertyMetadata(Stretch.None));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterProperty =
            DependencyProperty.RegisterAttached(nameof(Water), typeof(string), typeof(PasswordBoxEXT), new PropertyMetadata("请输入密码"));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty WaterSizeProperty =
            DependencyProperty.RegisterAttached(nameof(WaterSize), typeof(double), typeof(PasswordBoxEXT), new PropertyMetadata(0.85));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(PasswordBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleMinWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TitleMinWidth), typeof(double), typeof(PasswordBoxEXT));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius { get; set; }
        /// <summary>
        /// get自定义边框圆角
        /// </summary>
        public static CornerRadius GetRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(RadiusProperty);
        }
        /// <summary>
        /// set自定义边框圆角
        /// </summary>
        public static void SetRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(RadiusProperty, value);
        }

        /// <summary>
        /// 文本框的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("文本框的边框颜色")]
        public BrushEXT ItemBrush { get; set; }
        /// <summary>
        /// get文本框的边框颜色
        /// </summary>
        public static BrushEXT GetItemBrush(DependencyObject obj)
        {
            return (BrushEXT)obj.GetValue(ItemBrushProperty);
        }
        /// <summary>
        /// set文本框的边框颜色
        /// </summary>
        public static void SetItemBrush(DependencyObject obj, BrushEXT value)
        {
            obj.SetValue(ItemBrushProperty, value);
        }

        /// <summary>
        /// 图标
        /// </summary>
        [Category("扩展")]
        [Description("图标")]
        public ImageSource Icon { get; set; }
        /// <summary>
        /// get图标
        /// </summary>
        public static ImageSource GetIcon(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconProperty);
        }
        /// <summary>
        /// set图标
        /// </summary>
        public static void SetIcon(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconProperty, value);
        }

        /// <summary>
        /// 图标宽度
        /// </summary>
        [Category("扩展")]
        [Description("图标宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double IconWidth { get; set; }
        /// <summary>
        /// get图标宽度
        /// </summary>
        public static double GetIconWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(IconWidthProperty);
        }
        /// <summary>
        /// set图标宽度
        /// </summary>
        public static void SetIconWidth(DependencyObject obj, double value)
        {
            obj.SetValue(IconWidthProperty, value);
        }

        /// <summary>
        /// 图标高度
        /// </summary>
        [Category("扩展")]
        [Description("图标高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double IconHeight { get; set; }
        /// <summary>
        /// get图标高度
        /// </summary>
        public static double GetIconHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(IconHeightProperty);
        }
        /// <summary>
        /// set图标高度
        /// </summary>
        public static void SetIconHeight(DependencyObject obj, double value)
        {
            obj.SetValue(IconHeightProperty, value);
        }

        /// <summary>
        /// 图标的内容如何拉伸才适合其磁贴
        /// </summary>
        [Category("扩展")]
        [Description("图标的内容如何拉伸才适合其磁贴")]
        public Stretch IconStretch { get; set; }
        /// <summary>
        /// get图标的内容如何拉伸才适合其磁贴
        /// </summary>
        public static Stretch GetIconStretch(DependencyObject obj)
        {
            return (Stretch)obj.GetValue(IconStretchProperty);
        }
        /// <summary>
        /// set图标的内容如何拉伸才适合其磁贴
        /// </summary>
        public static void SetIconStretch(DependencyObject obj, Stretch value)
        {
            obj.SetValue(IconStretchProperty, value);
        }

        /// <summary>
        /// 水印内容
        /// </summary>
        [Category("扩展")]
        [Description("水印内容")]
        public string Water { get; set; }
        /// <summary>
        /// get水印内容
        /// </summary>
        public static string GetWater(DependencyObject obj)
        {
            return (string)obj.GetValue(WaterProperty);
        }
        /// <summary>
        /// set水印内容
        /// </summary>
        public static void SetWater(DependencyObject obj, string value)
        {
            obj.SetValue(WaterProperty, value);
        }

        /// <summary>
        /// 水印字体大小系数
        /// </summary>
        [Category("扩展")]
        [Description("水印字体大小系数")]
        public double WaterSize { get; set; }
        /// <summary>
        /// get水印字体大小系数
        /// </summary>
        public static double GetWaterSize(DependencyObject obj)
        {
            return (double)obj.GetValue(WaterSizeProperty);
        }
        /// <summary>
        /// set水印字体大小系数
        /// </summary>
        public static void SetWaterSize(DependencyObject obj, double value)
        {
            obj.SetValue(WaterSizeProperty, value);
        }

        /// <summary>
        /// 标题
        /// </summary>
        [Category("扩展")]
        [Description("标题")]
        public string Title { get; set; }
        /// <summary>
        /// get标题
        /// </summary>
        public static string GetTitle(DependencyObject obj)
        {
            return (string)obj.GetValue(TitleProperty);
        }
        /// <summary>
        /// set标题
        /// </summary>
        public static void SetTitle(DependencyObject obj, string value)
        {
            obj.SetValue(TitleProperty, value);
        }
        /// <summary>
        /// 标题长度
        /// </summary>
        [Category("扩展")]
        [Description("标题长度")]
        public double TitleMinWidth { get; set; }
        /// <summary>
        /// get标题
        /// </summary>
        public static double GetTitleMinWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(TitleMinWidthProperty);
        }
        /// <summary>
        /// set标题
        /// </summary>
        public static void SetTitleMinWidth(DependencyObject obj, double value)
        {
            obj.SetValue(TitleMinWidthProperty, value);
        }

        #endregion
    }
}
