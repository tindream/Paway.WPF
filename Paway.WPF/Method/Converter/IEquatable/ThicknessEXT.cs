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
    /// 自定义默认、鼠标划过时、鼠标点击时的Thickness宽度
    /// </summary>
    [TypeConverter(typeof(ThicknessEXTConverter))]
    public class ThicknessEXT : BaseModelInfo, IEquatable<ThicknessEXT>
    {
        private Thickness normal = new Thickness(1);
        /// <summary>
        /// 默认的宽度
        /// </summary>
        public Thickness Normal { get { return normal; } set { normal = value; OnPropertyChanged(); } }

        private Thickness mouse = new Thickness(1);
        /// <summary>
        /// 鼠标划过时的宽度
        /// </summary>
        public Thickness Mouse { get { return mouse; } set { mouse = value; OnPropertyChanged(); } }

        private Thickness pressed = new Thickness(1);
        /// <summary>
        /// 鼠标点击时的宽度
        /// </summary>
        public Thickness Pressed { get { return pressed; } set { pressed = value; OnPropertyChanged(); } }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            if (Mouse != Normal || Pressed != Normal) return $"{Normal};{Mouse};{Pressed}";
            return $"{Normal}";
        }
        /// <summary>
        /// </summary>
        public ThicknessEXT() { }
        /// <summary>
        /// </summary>
        public ThicknessEXT(double value) : this(value, value, value) { }
        /// <summary>
        /// </summary>
        public ThicknessEXT(double? normal, double? mouse, double? pressed = null)
        {
            if (normal != null) Normal = new Thickness(normal.Value);

            if (mouse != null) Mouse = new Thickness(mouse.Value);
            else if (normal != null) Mouse = Normal;

            if (pressed != null) Pressed = new Thickness(pressed.Value);
            else if (mouse != null) Pressed = Mouse;
            else if (normal != null) Pressed = Normal;
        }
        /// <summary>
        /// </summary>
        public ThicknessEXT(Thickness? normal, Thickness? mouse = null, Thickness? pressed = null, ThicknessEXT value = null)
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
        public bool Equals(ThicknessEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转ThicknessEXT
    /// </summary>
    internal class ThicknessEXTConverter : TypeConverter
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
                return new ThicknessEXT(@double);
            }
            if (value is string str)
            {
                var result = PMethod.ElementStatu<ThicknessEXT, Thickness>(context, str, Parse, ParseValue);
                return new ThicknessEXT(result.Normal, result.Mouse, result.Pressed, result.Old);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private Thickness? ParseValue(ThicknessEXT old, string name)
        {
            if (old == null) return null;
            return (Thickness)old.GetValue(name);
        }
        private Thickness Parse(string str)
        {
            if (str.Contains(","))
            {
                var strs = str.Split(',');
                if (strs.Length != 4) throw new WarningException("Parameter error");
                return new Thickness(Convert.ToDouble(strs[0]), Convert.ToDouble(strs[1]), Convert.ToDouble(strs[2]), Convert.ToDouble(strs[3]));
            }
            double.TryParse(str, out double result);
            return new Thickness(result);
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
