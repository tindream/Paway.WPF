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
    /// 自定义默认、鼠标划过时、鼠标点击时的CornerRadius圆角
    /// </summary>
    [TypeConverter(typeof(RadiusEXTConverter))]
    public class RadiusEXT : IEquatable<RadiusEXT>
    {
        /// <summary>
        /// 默认的圆角
        /// </summary>
        public CornerRadius Normal { get; set; } = new CornerRadius(5);
        /// <summary>
        /// 鼠标划过时的圆角
        /// </summary>
        public CornerRadius Mouse { get; set; } = new CornerRadius(5);
        /// <summary>
        /// 鼠标点击时的圆角
        /// </summary>
        public CornerRadius Pressed { get; set; } = new CornerRadius(5);

        /// <summary>
        /// </summary>
        public RadiusEXT() { }
        /// <summary>
        /// </summary>
        public RadiusEXT(double value) : this(value, value, value) { }
        /// <summary>
        /// </summary>
        public RadiusEXT(double? normal, double? mouse, double? pressed = null)
        {
            if (normal != null) Normal = new CornerRadius(normal.Value);

            if (mouse != null) Mouse = new CornerRadius(mouse.Value);
            else if (normal != null) Mouse = Normal;

            if (pressed != null) Pressed = new CornerRadius(pressed.Value);
            else if (mouse != null) Pressed = Mouse;
            else if (normal != null) Pressed = Normal;
        }
        /// <summary>
        /// </summary>
        public RadiusEXT(CornerRadius? normal, CornerRadius? mouse = null, CornerRadius? pressed = null, RadiusEXT value = null)
        {
            if (normal != null) Normal = normal.Value;
            else if (value != null) Normal = value.Normal;

            if (mouse != null) Mouse = mouse.Value;
            else if (normal != null) Mouse = Normal;
            else if (value != null) Mouse = value.Mouse;

            if (pressed != null) Pressed = pressed.Value;
            else if (mouse != null) Pressed = Mouse;
            else if (normal != null) Pressed = Normal;
            else if (value != null) Pressed = value.Pressed;
        }
        /// <summary>
        /// </summary>
        public bool Equals(RadiusEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转RadiusEXT
    /// </summary>
    internal class RadiusEXTConverter : TypeConverter
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
            if (value is double)
            {
                return new RadiusEXT((double)value);
            }
            if (value is string str)
            {
                var result = Method.ElementStatu<RadiusEXT, CornerRadius>(context, culture, str, Parse, ParseValue);
                return new RadiusEXT(result.Item2, result.Item3, result.Item4, result.Item1);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private CornerRadius? ParseValue(RadiusEXT old, string name)
        {
            if (old == null) return null;
            return (CornerRadius)old.GetValue(name);
        }
        private CornerRadius Parse(string str)
        {
            if (str.Contains(","))
            {
                var strs = str.Split(',');
                if (strs.Length != 4) throw new WarningException("参数错误");
                return new CornerRadius(Convert.ToDouble(strs[0]), Convert.ToDouble(strs[1]), Convert.ToDouble(strs[2]), Convert.ToDouble(strs[3]));
            }
            double.TryParse(str, out double result);
            return new CornerRadius(result);
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
