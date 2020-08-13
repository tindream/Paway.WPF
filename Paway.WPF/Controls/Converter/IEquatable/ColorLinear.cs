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
    /// 自定义线性渐变的Color颜色
    /// </summary>
    [TypeConverter(typeof(ColorLinearConverter))]
    public class ColorLinear : IEquatable<ColorLinear>
    {
        /// <summary>
        /// 起始颜色
        /// </summary>
        public Color Start { get; set; } = Color.FromArgb(85, Config.Color.R, Config.Color.G, Config.Color.B);
        /// <summary>
        /// 终点颜色
        /// </summary>
        public Color End { get; set; } = Color.FromArgb(250, Config.Color.R, Config.Color.G, Config.Color.B);
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = 165;

        /// <summary>
        /// </summary>
        public ColorLinear()
        {
            Config.ColorChanged += Config_ColorChanged;
        }
        private void Config_ColorChanged(Color obj)
        {
            if (this.Start is Color start && start.R == obj.R && start.G == obj.G && start.B == obj.B)
            {
                this.Start = Color.FromArgb(start.A, Config.Color.R, Config.Color.G, Config.Color.B);
            }
            if (this.End is Color end && end.R == obj.R && end.G == obj.G && end.B == obj.B)
            {
                this.End = Color.FromArgb(end.A, Config.Color.R, Config.Color.G, Config.Color.B);
            }
        }
        /// <summary>
        /// </summary>
        public ColorLinear(Color? start, Color? end = null, int? alpha = null, ColorLinear value = null) : this()
        {
            if (alpha != null) Alpha = alpha.Value;
            else if (value != null) Alpha = value.Alpha;

            if (start != null) Start = start.Value;
            else if (value != null) Start = value.Start;

            if (end != null) End = end.Value;
            else if (start != null) Reset(start.Value, Alpha);
            else if (value != null) End = value.End;
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha差异
        /// </summary>
        public ColorLinear Reset(Color color, int alpha = 165)
        {
            Start = color;
            var a = color.A + alpha;
            if (a > 255) a = 255;
            End = Color.FromArgb((byte)a, color.R, color.G, color.B);
            return this;
        }
        /// <summary>
        /// </summary>
        public bool Equals(ColorLinear other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End) && Alpha.Equals(other.Alpha);
        }
    }
    /// <summary>
    /// 字符串转ColorLinear
    /// </summary>
    internal class ColorLinearConverter : TypeConverter
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
                var old = Method.GetValue<ColorLinear>(context);

                var strs = str.Split(';');
                Color? start = null;
                Color? end = null;
                int? alpha = null;
                if (strs.Length > 0)
                {
                    if (!string.IsNullOrEmpty(strs[0])) start = Parse(strs[0]);
                    else if (old != null) start = old.Start;
                }
                if (strs.Length > 1)
                {
                    if (!string.IsNullOrEmpty(strs[1])) end = Parse(strs[1]);
                    else if (old != null) end = old.End;
                }
                if (strs.Length > 2)
                {
                    if (!string.IsNullOrEmpty(strs[2])) alpha = Convert.ToInt32(strs[3], culture);
                    else if (old != null) alpha = old.Alpha;
                }
                return new ColorLinear(start, end, alpha, old);
            }
            return base.ConvertFrom(context, culture, value);
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
