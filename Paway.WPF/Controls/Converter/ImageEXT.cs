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
using System.Windows.Media.Imaging;

namespace Paway.WPF
{
    /// <summary>
    /// 自定义默认、鼠标划过时、鼠标点击时的图片
    /// </summary>
    [TypeConverter(typeof(ImageEXTConverter))]
    public class ImageEXT : IEquatable<ImageEXT>
    {
        /// <summary>
        /// 默认的图片
        /// </summary>
        public ImageSource Normal { get; set; }
        /// <summary>
        /// 鼠标划过时的图片
        /// </summary>
        public ImageSource Mouse { get; set; }
        /// <summary>
        /// 鼠标点击时的图片
        /// </summary>
        public ImageSource Pressed { get; set; }

        /// <summary>
        /// </summary>
        public ImageEXT() { }
        /// <summary>
        /// </summary>
        public ImageEXT(string uri) : this(uri, uri, uri) { }
        /// <summary>
        /// </summary>
        public ImageEXT(string normal, string mouse = null, string pressed = null)
        {
            if (normal != null) Normal = new BitmapImage(new Uri(normal));
            if (mouse != null) Mouse = new BitmapImage(new Uri(mouse));
            else Mouse = Normal;
            if (pressed != null) Pressed = new BitmapImage(new Uri(pressed));
            else Pressed = Mouse;
        }
        /// <summary>
        /// </summary>
        public ImageEXT(ImageSource normal, ImageSource mouse = null, ImageSource pressed = null)
        {
            if (normal != null) Normal = normal;
            if (mouse != null) Mouse = mouse;
            else Mouse = Normal;
            if (pressed != null) Pressed = pressed;
            else Pressed = Mouse;
        }
        /// <summary>
        /// </summary>
        public bool Equals(ImageEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转ImageEXT
    /// </summary>
    public class ImageEXTConverter : TypeConverter
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
        /// 自定义字符串转换，以位运算符(|)隔开
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null)
            {
                throw base.GetConvertFromException(value);
            }
            if (value is string str)
            {
                var strs = str.Split('|');
                return new ImageEXT(strs.Length > 0 ? strs[0] : null, strs.Length > 1 ? strs[1] : null, strs.Length > 2 ? strs[2] : null);
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
