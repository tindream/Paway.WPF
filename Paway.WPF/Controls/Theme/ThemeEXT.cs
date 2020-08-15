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
    /// Theme扩展
    /// </summary>
    public class ThemeEXT
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ThemeEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(170, 255)));

        #endregion

        #region 扩展
        #region 自定义边框圆角
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

        #endregion

        #region 文本框的边框颜色
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

        #endregion

        #endregion
    }
}
