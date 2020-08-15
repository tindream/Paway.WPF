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
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached(nameof(FontSize), typeof(double), typeof(ThemeEXT), new PropertyMetadata(Config.FontSize));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(170, 255)));

        #endregion

        #region 扩展
        /// <summary>
        /// 字体大小
        /// </summary>
        [Category("扩展")]
        [Description("字体大小")]
        public double FontSize { get; set; }
        /// <summary>
        /// get字体大小
        /// </summary>
        public static double GetFontSize(DependencyObject obj)
        {
            return (double)obj.GetValue(FontSizeProperty);
        }
        /// <summary>
        /// set字体大小
        /// </summary>
        public static void SetFontSize(DependencyObject obj, double value)
        {
            obj.SetValue(FontSizeProperty, value);
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

        #endregion
    }
}
