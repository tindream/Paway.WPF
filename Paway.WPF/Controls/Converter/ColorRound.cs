using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 自定义默认、鼠标划过时、鼠标点击时的Color颜色
    /// </summary>
    [TypeConverter(typeof(ColorRoundConverter))]
    public class ColorRound : IEquatable<ColorRound>
    {
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Color Normal { get; set; } = Colors.LightGray;
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Color Mouse { get; set; } = Color.FromArgb(210, 35, 175, 255);
        /// <summary>
        /// 鼠标点击时的颜色
        /// </summary>
        public Color Pressed { get; set; } = Color.FromArgb(250, 35, 175, 255);

        /// <summary>
        /// </summary>
        public ColorRound() { }
        /// <summary>
        /// </summary>
        public ColorRound(Color? normal, Color? mouse = null, Color? pressed = null, int add = 50)
        {
            if (normal != null) Normal = normal.Value;
            if (mouse != null) Mouse = mouse.Value;
            else if (normal != null) Reset(normal.Value, add);
            if (pressed != null) Pressed = pressed.Value;
            else if (mouse != null) Focused(mouse.Value, add);
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha差异
        /// </summary>
        public ColorRound Reset(Color color, int add = 50)
        {
            Normal = color;
            Mouse = Color.FromArgb((byte)(color.A - add), color.R, color.G, color.B);
            var a = color.A + add;
            if (a > 255) a = 255;
            Pressed = Color.FromArgb((byte)a, color.R, color.G, color.B);
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        public ColorRound Focused(Color color, int add = 50)
        {
            Mouse = color;
            var a = color.A + add;
            if (a > 255) a = 255;
            Pressed = Color.FromArgb((byte)a, color.R, color.G, color.B);
            return this;
        }
        /// <summary>
        /// </summary>
        public bool Equals(ColorRound other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转ColorRound
    /// </summary>
    public class ColorRoundConverter : TypeConverter
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
                var strs = str.Split(';');
                Color? normal = null;
                Color? mouse = null;
                Color? pressed = null;
                int add = 50;
                if (strs.Length > 0 && !string.IsNullOrEmpty(strs[0])) normal = (Color)ColorConverter.ConvertFromString(strs[0]);
                if (strs.Length > 1 && !string.IsNullOrEmpty(strs[1])) mouse = (Color)ColorConverter.ConvertFromString(strs[1]);
                if (strs.Length > 2 && !string.IsNullOrEmpty(strs[2])) pressed = (Color)ColorConverter.ConvertFromString(strs[2]);
                if (strs.Length > 3 && !string.IsNullOrEmpty(strs[3])) add = Convert.ToInt32(strs[3], culture);
                return new ColorRound(normal, mouse, pressed, add);
            }
            return base.ConvertFrom(context, culture, value);
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
