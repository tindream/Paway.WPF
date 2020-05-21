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
    public class ColorEXT : IEquatable<ColorEXT>
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
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = 50;

        /// <summary>
        /// </summary>
        public ColorEXT() { }
        /// <summary>
        /// </summary>
        public ColorEXT(Color? normal, Color? mouse = null, Color? pressed = null, int? alpha = 50, ColorEXT value = null)
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
            else if (value != null) Pressed = value.Pressed;
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha差异
        /// </summary>
        public ColorEXT Reset(Color color, int alpha = 50)
        {
            Normal = color;
            var a = color.A - alpha;
            if (a < 0) a = 0;
            Mouse = Color.FromArgb((byte)a, color.R, color.G, color.B);
            a = color.A + alpha;
            if (a > 255) a = 255;
            Pressed = Color.FromArgb((byte)a, color.R, color.G, color.B);
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        public ColorEXT Focused(Color color, int alpha = 50)
        {
            Mouse = color;
            var a = color.A + alpha;
            if (a > 255) a = 255;
            Pressed = Color.FromArgb((byte)a, color.R, color.G, color.B);
            return this;
        }
        /// <summary>
        /// </summary>
        public bool Equals(ColorEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转ColorEXT
    /// </summary>
    public class ColorEXTConverter : TypeConverter
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
                int? alpha = null;
                if (strs.Length > 0 && !string.IsNullOrEmpty(strs[0])) normal = (Color)ColorConverter.ConvertFromString(strs[0]);
                if (strs.Length > 1 && !string.IsNullOrEmpty(strs[1])) mouse = (Color)ColorConverter.ConvertFromString(strs[1]);
                if (strs.Length > 2 && !string.IsNullOrEmpty(strs[2])) pressed = (Color)ColorConverter.ConvertFromString(strs[2]);
                if (strs.Length > 3 && !string.IsNullOrEmpty(strs[3])) alpha = Convert.ToInt32(strs[3], culture);

                ColorEXT old = null;
                if (context != null)
                {
                    var service = (IProvideValueTarget)context.GetService(typeof(IProvideValueTarget));
                    var objType = service.TargetObject.GetType();
                    var obj = (DependencyObject)Activator.CreateInstance(objType);
                    var property = (DependencyProperty)service.TargetProperty;
                    old = (ColorEXT)obj.GetValue(property);
                }
                return new ColorEXT(normal, mouse, pressed, alpha, old);
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
