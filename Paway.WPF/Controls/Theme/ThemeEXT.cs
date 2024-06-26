﻿using System;
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
        #region 主题字体
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.RegisterAttached(nameof(FontFamily), typeof(ThemeFontFamily), typeof(ThemeEXT), new PropertyMetadata(new ThemeFontFamily(PConfig.FontFamily)));
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

        #endregion

        #region 主题字体大小
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached(nameof(FontSize), typeof(ThemeFontSize), typeof(ThemeEXT), new PropertyMetadata(new ThemeFontSize(PConfig.FontSize)));
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

        #endregion

        #region 主题前景色
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(Foreground), typeof(ThemeForeground), typeof(ThemeEXT),
                new PropertyMetadata(new ThemeForeground(PConfig.Foreground)));
        /// <summary>
        /// 前景颜色
        /// </summary>
        private ThemeForeground Foreground { get; set; }
        /// <summary>
        /// get前景颜色
        /// </summary>
        public static ThemeForeground GetForeground(DependencyObject obj)
        {
            return (ThemeForeground)obj.GetValue(ForegroundProperty);
        }
        /// <summary>
        /// set前景颜色
        /// </summary>
        public static void SetForeground(DependencyObject obj, ThemeForeground value)
        {
            obj.SetValue(ForegroundProperty, value);
        }

        #endregion

        #region 主题背景色
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(Background), typeof(ThemeForeground), typeof(ThemeEXT),
                new PropertyMetadata(new ThemeForeground(PConfig.Background, 0, true)));
        /// <summary>
        /// 背景颜色
        /// </summary>
        private ThemeForeground Background { get; set; }
        /// <summary>
        /// get背景颜色
        /// </summary>
        public static ThemeForeground GetBackground(DependencyObject obj)
        {
            return (ThemeForeground)obj.GetValue(BackgroundProperty);
        }
        /// <summary>
        /// set背景颜色
        /// </summary>
        public static void SetBackground(DependencyObject obj, ThemeForeground value)
        {
            obj.SetValue(BackgroundProperty, value);
        }

        #endregion

        #region 主题颜色
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ThemeEXT),
                new PropertyMetadata(new BrushEXT(PConfig.Alpha)));
        /// <summary>
        /// 项颜色(禁止个性设置，通过主题色更新)
        /// </summary>
        public BrushEXT ItemBrush { get; set; }

        #endregion
    }
}
