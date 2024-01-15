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
            DependencyProperty.RegisterAttached(nameof(FontSize), typeof(ThemeFontSize), typeof(ThemeEXT), new PropertyMetadata(new ThemeFontSize(PConfig.FontSize)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.RegisterAttached(nameof(FontFamily), typeof(ThemeFontFamily), typeof(ThemeEXT), new PropertyMetadata(new ThemeFontFamily(PConfig.FontFamily)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(PConfig.Alpha)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty HighBrushProperty =
            DependencyProperty.RegisterAttached(nameof(HighBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(true)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(Background), typeof(ThemeBackground), typeof(ThemeEXT),
                new PropertyMetadata(new ThemeBackground(PConfig.Background)));

        #endregion

        #region 扩展
        /// <summary>
        /// 字体大小
        /// <para>默认值：主题字体大小</para>
        /// </summary>
        private ThemeFontSize FontSize { get; set; }
        /// <summary>
        /// get字体大小
        /// </summary>
        public static ThemeFontSize GetFontSize(DependencyObject obj)
        {
            return (ThemeFontSize)obj.GetValue(FontSizeProperty);
        }
        /// <summary>
        /// set字体大小
        /// </summary>
        public static void SetFontSize(DependencyObject obj, ThemeFontSize value)
        {
            obj.SetValue(FontSizeProperty, value);
        }

        /// <summary>
        /// 文本字体
        /// <para>默认值：主题文本字体</para>
        /// </summary>
        private ThemeFontFamily FontFamily { get; set; }
        /// <summary>
        /// get文本字体
        /// </summary>
        public static ThemeFontFamily GetFontFamily(DependencyObject obj)
        {
            return (ThemeFontFamily)obj.GetValue(FontFamilyProperty);
        }
        /// <summary>
        /// set文本字体
        /// </summary>
        public static void SetFontFamily(DependencyObject obj, ThemeFontFamily value)
        {
            obj.SetValue(FontFamilyProperty, value);
        }

        /// <summary>
        /// 项颜色(禁止个性设置，通过主题色更新)
        /// </summary>
        public BrushEXT ItemBrush { get; set; }

        /// <summary>
        /// 主题深色(禁止个性设置，通过主题色更新)
        /// </summary>
        public BrushEXT HighBrush { get; set; }

        /// <summary>
        /// 窗体背景颜色
        /// </summary>
        private ThemeBackground Background { get; set; }
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
