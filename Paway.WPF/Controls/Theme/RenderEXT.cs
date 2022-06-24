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
    /// 提供用于控制对象的呈现行为的选项
    /// </summary>
    public class RenderEXT
    {
        #region 项颜色
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(Brush), typeof(RenderEXT),
                new PropertyMetadata(Colors.LightGray.ToBrush(), OnItemBrushChanged));
        private static void OnItemBrushChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Border border && border.BorderBrush is DrawingBrush drawing)
            {
                ResetItemBrush(drawing, e.NewValue);
            }
            else if (obj is Panel panel && panel.Background is DrawingBrush drawing2)
            {
                ResetItemBrush(drawing2, e.NewValue);
            }
        }
        private static void ResetItemBrush(DrawingBrush drawing, object value)
        {
            if (drawing.Drawing is DrawingGroup group)
            {
                if (group.Children.FirstOrDefault() is GeometryDrawing geometry)
                {
                    geometry.SetValue(ItemBrushProperty, value);
                }
            }
        }
        /// <summary>
        /// 项颜色
        /// </summary>
        private Brush ItemBrush { get; set; }
        /// <summary>
        /// get字体大小
        /// </summary>
        public static Brush GetItemBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(ItemBrushProperty);
        }
        /// <summary>
        /// set字体大小
        /// </summary>
        public static void SetItemBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(ItemBrushProperty, value);
        }

        #endregion

        #region 渐变色
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(ColorLinear), typeof(ColorLinear), typeof(RenderEXT),
                new PropertyMetadata(new ColorLinear()));
        /// <summary>
        /// 渐变色
        /// </summary>
        private ColorLinear ColorLinear { get; set; }
        /// <summary>
        /// get渐变色
        /// </summary>
        public static ColorLinear GetColorLinear(DependencyObject obj)
        {
            return (ColorLinear)obj.GetValue(ColorLinearProperty);
        }
        /// <summary>
        /// set渐变色
        /// </summary>
        public static void SetColorLinear(DependencyObject obj, ColorLinear value)
        {
            obj.SetValue(ColorLinearProperty, value);
        }

        #endregion
    }
}
