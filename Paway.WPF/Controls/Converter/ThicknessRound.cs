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
    /// 自定义默认、鼠标划过时、鼠标点击时的Thickness宽度
    /// </summary>
    [TypeConverter(typeof(ThicknessRoundConverter))]
    public class ThicknessRound : IEquatable<ThicknessRound>
    {
        /// <summary>
        /// 默认的宽度
        /// </summary>
        public Thickness Normal { get; set; } = new Thickness(1);
        /// <summary>
        /// 鼠标划过时的宽度
        /// </summary>
        public Thickness Mouse { get; set; } = new Thickness(1);
        /// <summary>
        /// 鼠标点击时的宽度
        /// </summary>
        public Thickness Pressed { get; set; } = new Thickness(1);

        public ThicknessRound() { }
        public ThicknessRound(double value) : this(value, value, value) { }
        public ThicknessRound(double? normal, double? mouse, double? pressed)
        {
            if (normal != null) Normal = new Thickness(normal.Value);
            if (mouse != null) Mouse = new Thickness(mouse.Value);
            else Mouse = Normal;
            if (pressed != null) Pressed = new Thickness(pressed.Value);
            else Pressed = Mouse;
        }
        public ThicknessRound(Thickness? normal, Thickness? mouse, Thickness? pressed)
        {
            if (normal != null) Normal = normal.Value;
            if (mouse != null) Mouse = mouse.Value;
            else Mouse = Normal;
            if (pressed != null) Pressed = pressed.Value;
            else Pressed = Mouse;
        }
        public bool Equals(ThicknessRound other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转ThicknessRound
    /// </summary>
    public class ThicknessRoundConverter : TypeConverter
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
                return new ThicknessRound((double)value);
            }
            if (value is string str)
            {
                var strs = str.Split(';');
                Thickness? normal = null;
                Thickness? mouse = null;
                Thickness? pressed = null;
                if (strs.Length > 0) Parse(strs[0], out normal);
                if (strs.Length > 1) Parse(strs[1], out mouse);
                if (strs.Length > 2) Parse(strs[2], out pressed);
                return new ThicknessRound(normal, mouse, pressed);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private void Parse(string str, out Thickness? value)
        {
            if (string.IsNullOrEmpty(str)) value = null;
            if (str.Contains(","))
            {
                var strs = str.Split(',');
                if (strs.Length != 4) throw new WarningException("参数错误");
                value = new Thickness(Convert.ToDouble(strs[0]), Convert.ToDouble(strs[1]), Convert.ToDouble(strs[2]), Convert.ToDouble(strs[3]));
            }
            else if (double.TryParse(str, out double result))
            {
                value = new Thickness(result);
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
