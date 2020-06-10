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
    /// 自定义默认、鼠标划过时、鼠标点击时的大小
    /// </summary>
    [TypeConverter(typeof(DoubleEXTConverter))]
    public class DoubleEXT : IEquatable<DoubleEXT>
    {
        /// <summary>
        /// 默认的大小
        /// </summary>
        public double Normal { get; set; }
        /// <summary>
        /// 鼠标划过时的大小
        /// </summary>
        public double Mouse { get; set; }
        /// <summary>
        /// 鼠标点击时的大小
        /// </summary>
        public double Pressed { get; set; }

        /// <summary>
        /// </summary>
        public DoubleEXT() { }
        /// <summary>
        /// </summary>
        public DoubleEXT(double value) : this(value, value, value) { }
        /// <summary>
        /// </summary>
        public DoubleEXT(double? normal, double? mouse = null, double? pressed = null, DoubleEXT value = null)
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
        public bool Equals(DoubleEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转DoubleEXT
    /// </summary>
    internal class DoubleEXTConverter : TypeConverter
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
            if (value is double @double)
            {
                return new DoubleEXT(@double);
            }
            if (value is string str)
            {
                var result = Method.ElementStatu<DoubleEXT, double>(context, culture, str, Parse, ParseValue);
                return new DoubleEXT(result.Item2, result.Item3, result.Item4, result.Item1);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private double? ParseValue(DoubleEXT old, string name)
        {
            if (old == null) return null;
            return (double)old.GetValue(name);
        }
        private double Parse(string str)
        {
            double.TryParse(str, out double result);
            return result;
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
