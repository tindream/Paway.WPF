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
        public static readonly DependencyProperty BorderMouseBrushProperty =
            DependencyProperty.RegisterAttached(nameof(BorderMouseBrush), typeof(Brush), typeof(PasswordBoxEXT),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(170, Config.Color.R, Config.Color.G, Config.Color.B))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BorderPressedBrushProperty =
            DependencyProperty.RegisterAttached(nameof(BorderPressedBrush), typeof(Brush), typeof(PasswordBoxEXT),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, Config.Color.R, Config.Color.G, Config.Color.B))));
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
        /// 文本框的鼠标移过时的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("文本框的鼠标移过时的边框颜色")]
        public Brush BorderMouseBrush { get; set; }
        /// <summary>
        /// get文本框的鼠标移过时的边框颜色
        /// </summary>
        public static Brush GetBorderMouseBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BorderMouseBrushProperty);
        }
        /// <summary>
        /// set文本框的鼠标移过时的边框颜色
        /// </summary>
        public static void SetBorderMouseBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(BorderMouseBrushProperty, value);
        }
        /// <summary>
        /// 文本框的鼠标按下时的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("文本框的鼠标按下时的边框颜色")]
        public Brush BorderPressedBrush { get; set; }
        /// <summary>
        /// get文本框的鼠标按下时的边框颜色
        /// </summary>
        public static Brush GetBorderPressedBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(BorderPressedBrushProperty);
        }
        /// <summary>
        /// set文本框的鼠标按下时的边框颜色
        /// </summary>
        public static void SetBorderPressedBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(BorderPressedBrushProperty, value);
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
