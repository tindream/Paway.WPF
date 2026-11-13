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
    public class RadiusEXT : BaseModelInfo, IEquatable<RadiusEXT>
    {
        private CornerRadius normal = new CornerRadius(3);
        /// <summary>
        /// 默认的圆角
        /// </summary>
        public CornerRadius Normal { get { return normal; } set { normal = value; OnPropertyChanged(); } }

        private CornerRadius mouse = new CornerRadius(3);
        /// <summary>
        /// 鼠标划过时的圆角
        /// </summary>
        public CornerRadius Mouse { get { return mouse; } set { mouse = value; OnPropertyChanged(); } }

        private CornerRadius pressed = new CornerRadius(3);
        /// <summary>
        /// 鼠标点击时的圆角
        /// </summary>
        public CornerRadius Pressed { get { return pressed; } set { pressed = value; OnPropertyChanged(); } }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            if (Mouse != Normal || Pressed != Normal) return $"{Normal};{Mouse};{Pressed}";
            return $"{Normal}";
        }
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
            if (other is null) return false;//x.Equals(null) 会选中强类型重载，原实现直接 NRE
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
        /// <summary>
        /// 与非泛型调用保持一致
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is RadiusEXT other && Equals(other);
        }
        /// <summary>
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = Normal.GetHashCode();
                hash = (hash * 397) ^ Mouse.GetHashCode();
                return (hash * 397) ^ Pressed.GetHashCode();
            }
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
            if (value is double @double)
            {
                return new RadiusEXT(@double);
            }
            if (value is string str)
            {
                var result = PMethod.ElementStatu<RadiusEXT, CornerRadius>(context, str, Parse, ParseValue);
                return new RadiusEXT(result.Normal, result.Mouse, result.Pressed, result.Old);
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
            if (str.Contains(','))
            {
                var strs = str.Split(',');
                if (strs.Length != 4) throw new WarningException("Parameter error");
                return new CornerRadius(ParseDouble(strs[0]), ParseDouble(strs[1]), ParseDouble(strs[2]), ParseDouble(strs[3]));
            }
            return new CornerRadius(ParseDouble(str));
        }
        /// <summary>
        /// XAML 数值固定按不变文化解析：逗号作小数点的区域设置下，默认解析会失败并把值静默当成 0
        /// </summary>
        private static double ParseDouble(string str)
        {
            if (!double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)) return 0;
            return result;
        }
        /// <summary>
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            //原实现两个分支写反：非 string 目标返回了字符串。目标为 string 时取 ToString，其余交给基类
            if (destinationType == typeof(string)) return value?.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
