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
            DependencyProperty.RegisterAttached(nameof(FontSize), typeof(FontSizeEXT), typeof(ThemeEXT), new PropertyMetadata(new FontSizeEXT(Config.FontSize)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(120, 70, 255)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HighBrushProperty =
            DependencyProperty.RegisterAttached(nameof(HighBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(true)));

        #endregion

        #region 扩展
        /// <summary>
        /// 字体大小
        /// </summary>
        [Category("扩展")]
        [Description("字体大小")]
        public FontSizeEXT FontSize { get; set; }
        /// <summary>
        /// get字体大小
        /// </summary>
        public static FontSizeEXT GetFontSize(DependencyObject obj)
        {
            return (FontSizeEXT)obj.GetValue(FontSizeProperty);
        }
        /// <summary>
        /// set字体大小
        /// </summary>
        public static void SetFontSize(DependencyObject obj, FontSizeEXT value)
        {
            obj.SetValue(FontSizeProperty, value);
        }

        /// <summary>
        /// 项颜色
        /// </summary>
        [Category("扩展")]
        [Description("项颜色")]
        public BrushEXT ItemBrush { get; set; }
        /// <summary>
        /// get项颜色
        /// </summary>
        public static BrushEXT GetItemBrush(DependencyObject obj)
        {
            return (BrushEXT)obj.GetValue(ItemBrushProperty);
        }
        /// <summary>
        /// set项颜色
        /// </summary>
        public static void SetItemBrush(DependencyObject obj, BrushEXT value)
        {
            obj.SetValue(ItemBrushProperty, value);
        }

        /// <summary>
        /// 主题深色
        /// </summary>
        [Category("扩展")]
        [Description("主题深色")]
        public BrushEXT HighBrush { get; set; }
        /// <summary>
        /// get主题深色
        /// </summary>
        public static BrushEXT GetHighBrush(DependencyObject obj)
        {
            return (BrushEXT)obj.GetValue(HighBrushProperty);
        }
        /// <summary>
        /// set主题深色
        /// </summary>
        public static void SetHighBrush(DependencyObject obj, BrushEXT value)
        {
            obj.SetValue(HighBrushProperty, value);
        }

        #endregion
    }
}
