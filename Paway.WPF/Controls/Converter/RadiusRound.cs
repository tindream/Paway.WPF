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
    /// 自定义默认、鼠标划过时、鼠标点击时的CornerRadius圆角
    /// </summary>
    [TypeConverter(typeof(RadiusRoundConverter))]
    public class RadiusRound : IEquatable<RadiusRound>
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

        public RadiusRound() { }
        public RadiusRound(double value) : this(value, value, value) { }
        public RadiusRound(double? normal, double? mouse, double? pressed)
        {
            if (normal != null) Normal = new CornerRadius(normal.Value);
            if (mouse != null) Mouse = new CornerRadius(mouse.Value);
            else Mouse = Normal;
            if (pressed != null) Pressed = new CornerRadius(pressed.Value);
            else Pressed = Mouse;
        }
        public RadiusRound(CornerRadius? normal, CornerRadius? mouse, CornerRadius? pressed)
        {
            if (normal != null) Normal = normal.Value;
            if (mouse != null) Mouse = mouse.Value;
            else Mouse = Normal;
            if (pressed != null) Pressed = pressed.Value;
            else Pressed = Mouse;
        }
        public bool Equals(RadiusRound other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转RadiusRound
    /// </summary>
    public class RadiusRoundConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw base.GetConvertFromException(value);
            }
            if (value is double)
            {
                return new RadiusRound((double)value);
            }
            if (value is string str)
            {
                var strs = str.Split(';');
                CornerRadius? normal = null;
                CornerRadius? mouse = null;
                CornerRadius? pressed = null;
                if (strs.Length > 0) Parse(strs[0], out normal);
                if (strs.Length > 1) Parse(strs[1], out mouse);
                if (strs.Length > 2) Parse(strs[2], out pressed);
                return new RadiusRound(normal, mouse, pressed);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private void Parse(string str, out CornerRadius? value)
        {
            if (string.IsNullOrEmpty(str)) value = null;
            if (str.Contains(","))
            {
                var strs = str.Split(',');
                if (strs.Length != 4) throw new WarningException("参数错误");
                value = new CornerRadius(Convert.ToDouble(strs[0]), Convert.ToDouble(strs[1]), Convert.ToDouble(strs[2]), Convert.ToDouble(strs[3]));
            }
            else if (double.TryParse(str, out double result))
            {
                value = new CornerRadius(result);
            }
            else value = null;
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);
            return value.ToString();
        }
    }
}
