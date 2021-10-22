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
    public class ColorEXT : ModelBase, IEquatable<ColorEXT>, IElementStatu<Color>
    {
        private Color normal = Colors.LightGray;
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Color Normal { get { return normal; } set { normal = value; OnPropertyChanged(); } }
        private Color mouse = PMethod.ThemeColor(210);
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Color Mouse { get { return mouse; } set { mouse = value; OnPropertyChanged(); } }
        private Color pressed = PMethod.ThemeColor(240);
        /// <summary>
        /// 鼠标点击时的颜色
        /// </summary>
        public Color Pressed { get { return pressed; } set { pressed = value; OnPropertyChanged(); } }
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = 35;

        /// <summary>
        /// </summary>
        public ColorEXT()
        {
            PConfig.ColorChanged += Config_ColorChanged;
        }
        private void Config_ColorChanged(Color obj)
        {
            if (this.Normal is Color normal && normal.R == obj.R && normal.G == obj.G && normal.B == obj.B)
            {
                if (normal != Colors.LightGray && normal != Color.FromArgb(255, 254, 254, 254) && normal != Colors.LightGray)
                {
                    this.Normal = PMethod.ThemeColor(normal.A);
                }
            }
            if (this.Mouse is Color mouse && mouse.R == obj.R && mouse.G == obj.G && mouse.B == obj.B)
            {
                this.Mouse = PMethod.ThemeColor(mouse.A);
            }
            if (this.Pressed is Color pressed && pressed.R == obj.R && pressed.G == obj.G && pressed.B == obj.B)
            {
                this.Pressed = PMethod.ThemeColor(pressed.A);
            }
        }
        /// <summary>
        /// 主题色：普通、鼠标移过、alpha变量
        /// </summary>
        public ColorEXT(Color? normal, byte mouse, byte pressed) : this(normal, PMethod.ThemeColor(mouse), PMethod.ThemeColor(pressed)) { }
        /// <summary>
        /// </summary>
        public ColorEXT(Color? normal, Color? mouse = null, Color? pressed = null, int? alpha = null, ColorEXT value = null) : this()
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
        /// 设置所有颜色，指定Alpha变量
        /// </summary>
        private ColorEXT Reset(Color color, int alpha)
        {
            Normal = color;
            Mouse = PMethod.AlphaColor(color.A - alpha, color);
            Pressed = PMethod.AlphaColor(color.A + alpha, color);
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        private ColorEXT Focused(Color color, int alpha)
        {
            Mouse = color;
            Pressed = PMethod.AlphaColor(color.A + alpha, color);
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
                var result = PMethod.ElementStatu<ColorEXT, Color>(context, culture, str, Parse, ParseValue);
                return new ColorEXT(result.Normal, result.Mouse, result.Pressed, result.Alpha, result.Old);
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
            if (byte.TryParse(str, out byte alpha))
            {
                return PMethod.ThemeColor(alpha);
            }
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
