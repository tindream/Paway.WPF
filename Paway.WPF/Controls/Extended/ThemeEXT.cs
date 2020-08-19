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
                new PropertyMetadata(new BrushEXT(170, 120, 250)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HighBrushProperty =
            DependencyProperty.RegisterAttached(nameof(HighBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(120, 255, 250, true)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(Background), typeof(ThemeBackground), typeof(ThemeEXT),
                new PropertyMetadata(new ThemeBackground(Config.Background)));

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
        /// 项颜色(禁止个性设置，通过主题色更新)
        /// </summary>
        [Category("扩展")]
        [Description("项颜色")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BrushEXT ItemBrush { get; set; }

        /// <summary>
        /// 主题深色(禁止个性设置，通过主题色更新)
        /// </summary>
        [Category("扩展")]
        [Description("主题深色")]
        public BrushEXT HighBrush { get; set; }

        /// <summary>
        /// 窗体背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("窗体背景颜色")]
        public ThemeBackground Background { get; set; }
        /// <summary>
        /// get窗体背景颜色
        /// </summary>
        public static ThemeBackground GetBackground(DependencyObject obj)
        {
            return (ThemeBackground)obj.GetValue(BackgroundProperty);
        }
        /// <summary>
        /// set窗体背景颜色
        /// </summary>
        public static void SetBackground(DependencyObject obj, ThemeBackground value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        #endregion
    }
}
