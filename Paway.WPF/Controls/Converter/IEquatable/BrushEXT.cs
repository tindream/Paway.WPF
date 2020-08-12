using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 自定义默认、鼠标划过时、鼠标点击时的Brush颜色
    /// </summary>
    [TypeConverter(typeof(BrushEXTConverter))]
    public class BrushEXT : IEquatable<BrushEXT>, IElementStatu<Brush>
    {
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Brush Normal { get; set; } = new SolidColorBrush(Colors.LightGray);
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Brush Mouse { get; set; } = new SolidColorBrush(Color.FromArgb(210, Config.Color.R, Config.Color.G, Config.Color.B));
        /// <summary>
        /// 鼠标点击时的颜色
        /// </summary>
        public Brush Pressed { get; set; } = new SolidColorBrush(Color.FromArgb(250, Config.Color.R, Config.Color.G, Config.Color.B));
        /// <summary>
        /// 鼠标划过时的选中颜色
        /// </summary>
        public Brush PressedMouse
        {
            get
            {
                var pressed = Pressed as SolidColorBrush;
                var alpha = pressed.Color.A - 20;
                if (alpha < 0) alpha = 0;
                return new SolidColorBrush(Color.FromArgb((byte)alpha, pressed.Color.R, pressed.Color.G, pressed.Color.B));
            }
        }
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = 50;

        /// <summary>
        /// </summary>
        public BrushEXT() { }
        /// <summary>
        /// </summary>
        public BrushEXT(Color? normal, Color? mouse = null, Color? pressed = null, int? alpha = null, BrushEXT value = null)
        {
            if (alpha != null) Alpha = alpha.Value;
            else if (value != null) Alpha = value.Alpha;

            if (normal != null) Normal = new SolidColorBrush(normal.Value);
            else if (value != null) Normal = value.Normal;

            if (mouse != null) Mouse = new SolidColorBrush(mouse.Value);
            else if (normal != null) Reset(normal.Value, Alpha);
            else if (value != null) Mouse = value.Mouse;

            if (pressed != null) Pressed = new SolidColorBrush(pressed.Value);
            else if (mouse != null) Focused(mouse.Value, Alpha);
            else if (normal == null && value != null) Pressed = value.Pressed;
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha差异
        /// </summary>
        public BrushEXT Reset(Color color, int alpha = 50)
        {
            Normal = new SolidColorBrush(color);
            var a = color.A - alpha;
            if (a < 0) a = 0;
            Mouse = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
            a = color.A + alpha;
            if (color == Colors.Transparent) a = 0;
            else if (a > 255) a = 255;
            Pressed = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        public BrushEXT Focused(Color color, int alpha = 50)
        {
            Mouse = new SolidColorBrush(color);
            var a = color.A + alpha;
            if (color == Colors.Transparent) a = 0;
            else if (a > 255) a = 255;
            Pressed = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
            return this;
        }
        /// <summary>
        /// </summary>
        public bool Equals(BrushEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed) && Alpha.Equals(other.Alpha);
        }
    }
    /// <summary>
    /// 字符串转BrushEXT
    /// </summary>
    internal class BrushEXTConverter : TypeConverter
    {
        /// <summary>
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// </summary>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        /// <summary>
        /// 自定义字符串转换，以分号(;)隔开
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw base.GetConvertFromException(value);
            }
            if (value is string str)
            {
                var result = Method.ElementStatu<BrushEXT, Color>(context, culture, str, Parse, ParseValue);
                return new BrushEXT(result.Item2, result.Item3, result.Item4, result.Item5, result.Item1);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private Color? ParseValue(BrushEXT old, string name)
        {
            if (old == null) return null;
            return (old.GetValue(name) as SolidColorBrush).Color;
        }
        private Color Parse(string str)
        {
            return (Color)ColorConverter.ConvertFromString(str);
        }
        /// <summary>
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);
            return value.ToString();
        }
    }
}
