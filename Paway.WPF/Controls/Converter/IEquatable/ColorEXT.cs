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
    /// 自定义默认、鼠标划过时、鼠标点击时的Color颜色
    /// </summary>
    [TypeConverter(typeof(ColorEXTConverter))]
    public class ColorEXT : IEquatable<ColorEXT>, IElementStatu<Color>
    {
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Color Normal { get; set; } = Colors.LightGray;
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Color Mouse { get; set; } = Color.FromArgb(210, Config.Color.R, Config.Color.G, Config.Color.B);
        /// <summary>
        /// 鼠标点击时的颜色
        /// </summary>
        public Color Pressed { get; set; } = Color.FromArgb(250, Config.Color.R, Config.Color.G, Config.Color.B);
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = 50;

        /// <summary>
        /// </summary>
        public ColorEXT() { }
        /// <summary>
        /// </summary>
        public ColorEXT(Color? normal, Color? mouse = null, Color? pressed = null, int? alpha = null, ColorEXT value = null)
        {
            if (alpha != null) Alpha = alpha.Value;
            else if (value != null) Alpha = value.Alpha;

            if (normal != null) Normal = normal.Value;
            else if (value != null) Normal = value.Normal;

            if (mouse != null) Mouse = mouse.Value;
            else if (normal != null) Reset(normal.Value, Alpha);
            else if (value != null) Mouse = value.Mouse;

            if (pressed != null) Pressed = pressed.Value;
            else if (mouse != null) Focused(mouse.Value, Alpha);
            else if (normal == null && value != null) Pressed = value.Pressed;
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha差异
        /// </summary>
        public ColorEXT Reset(Color color, int alpha = 50)
        {
            Normal = color;
            var a = color.A - alpha;
            if (a < 0) a = 0;
            Mouse = Color.FromArgb((byte)a, color.R, color.G, color.B);
            a = color.A + alpha;
            if (color == Colors.Transparent) a = 0;
            else if (a > 255) a = 255;
            Pressed = Color.FromArgb((byte)a, color.R, color.G, color.B);
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        public ColorEXT Focused(Color color, int alpha = 50)
        {
            Mouse = color;
            var a = color.A + alpha;
            if (color == Colors.Transparent) a = 0;
            else if (a > 255) a = 255;
            Pressed = Color.FromArgb((byte)a, color.R, color.G, color.B);
            return this;
        }
        /// <summary>
        /// </summary>
        public bool Equals(ColorEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed) && Alpha.Equals(other.Alpha);
        }
    }
    /// <summary>
    /// 字符串转ColorEXT
    /// </summary>
    internal class ColorEXTConverter : TypeConverter
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
                var result = Method.ElementStatu<ColorEXT, Color>(context, culture, str, Parse, ParseValue);
                return new ColorEXT(result.Item2, result.Item3, result.Item4, result.Item5, result.Item1);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private Color? ParseValue(ColorEXT old, string name)
        {
            if (old == null) return null;
            return (Color)old.GetValue(name);
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
