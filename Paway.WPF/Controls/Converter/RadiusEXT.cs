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
            else if (normal != null) Mouse = new CornerRadius(normal.Value);
            if (pressed != null) Pressed = new CornerRadius(pressed.Value);
            else if (mouse != null) Pressed = new CornerRadius(mouse.Value);
        }
        /// <summary>
        /// </summary>
        public RadiusEXT(CornerRadius? normal, CornerRadius? mouse = null, CornerRadius? pressed = null, RadiusEXT value = null)
        {
            if (normal != null) Normal = normal.Value;
            else if (value != null) Normal = value.Normal;
            if (mouse != null) Mouse = mouse.Value;
            else if (normal != null) Mouse = normal.Value;
            else if (value != null) Mouse = value.Mouse;
            if (pressed != null) Pressed = pressed.Value;
            else if (mouse != null) Pressed = mouse.Value;
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
    public class RadiusEXTConverter : TypeConverter
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
                var strs = str.Split(';');
                CornerRadius? normal = null;
                CornerRadius? mouse = null;
                CornerRadius? pressed = null;
                if (strs.Length > 0) Parse(strs[0], out normal);
                if (strs.Length > 1) Parse(strs[1], out mouse);
                if (strs.Length > 2) Parse(strs[2], out pressed);

                RadiusEXT old = null;
                if (context != null)
                {
                    var service = (IProvideValueTarget)context.GetService(typeof(IProvideValueTarget));
                    var objType = service.TargetObject.GetType();
                    var obj = (DependencyObject)Activator.CreateInstance(objType);
                    var property = (DependencyProperty)service.TargetProperty;
                    old = (RadiusEXT)obj.GetValue(property);
                }
                return new RadiusEXT(normal, mouse, pressed, old);
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
