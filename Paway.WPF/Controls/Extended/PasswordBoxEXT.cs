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
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(PasswordBoxEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(PasswordBoxEXT),
                new PropertyMetadata(new BrushEXT(Colors.LightGray, Color.FromArgb(170, Config.Color.R, Config.Color.G, Config.Color.B), null, 85)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached(nameof(Icon), typeof(ImageSource), typeof(PasswordBoxEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IconStretchProperty =
            DependencyProperty.RegisterAttached(nameof(IconStretch), typeof(Stretch), typeof(PasswordBoxEXT),
            new PropertyMetadata(Stretch.None));

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
        public static BrushEXT GetBorderPressedBrush(DependencyObject obj)
        {
            return (BrushEXT)obj.GetValue(ItemBrushProperty);
        }
        /// <summary>
        /// set文本框的边框颜色
        /// </summary>
        public static void SetBorderPressedBrush(DependencyObject obj, BrushEXT value)
        {
            obj.SetValue(ItemBrushProperty, value);
        }

        /// <summary>
        /// 图片
        /// </summary>
        [Category("扩展")]
        [Description("图片")]
        public ImageSource Icon { get; set; }
        /// <summary>
        /// get图片
        /// </summary>
        public static ImageSource GetIcon(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(IconProperty);
        }
        /// <summary>
        /// set图片
        /// </summary>
        public static void SetIcon(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(IconProperty, value);
        }

        /// <summary>
        /// 图片的内容如何拉伸才适合其磁贴
        /// </summary>
        [Category("扩展")]
        [Description("图片的内容如何拉伸才适合其磁贴")]
        public Stretch IconStretch { get; set; }
        /// <summary>
        /// get图片的内容如何拉伸才适合其磁贴
        /// </summary>
        public static Stretch GetIconStretch(DependencyObject obj)
        {
            return (Stretch)obj.GetValue(IconStretchProperty);
        }
        /// <summary>
        /// set图片的内容如何拉伸才适合其磁贴
        /// </summary>
        public static void SetIconStretch(DependencyObject obj, Stretch value)
        {
            obj.SetValue(IconStretchProperty, value);
        }

        #endregion
    }
}
