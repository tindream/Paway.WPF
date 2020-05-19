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
    /// 自定义默认、鼠标划过时、鼠标点击时的Brush颜色
    /// </summary>
    [TypeConverter(typeof(BrushRoundConverter))]
    public class BrushRound : IEquatable<BrushRound>
    {
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Brush Normal { get; set; } = new SolidColorBrush(Colors.LightGray);
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Brush Mouse { get; set; } = new SolidColorBrush(Color.FromArgb(210, 35, 175, 255));
        /// <summary>
        /// 鼠标点击时的颜色
        /// </summary>
        public Brush Pressed { get; set; } = new SolidColorBrush(Color.FromArgb(250, 35, 175, 255));

        public BrushRound() { }
        public BrushRound(Color? normal, Color? mouse = null, Color? pressed = null, int add = 40)
        {
            if (normal != null) Normal = new SolidColorBrush(normal.Value);
            if (mouse != null) Mouse = new SolidColorBrush(mouse.Value);
            else if (normal != null) Reset(normal.Value, add);
            if (pressed != null) Pressed = new SolidColorBrush(pressed.Value);
            else if (mouse != null) Focused(mouse.Value, add);
        }
        public BrushRound Reset(Color color, int add = 40)
        {
            Normal = new SolidColorBrush(color);
            Mouse = new SolidColorBrush(Color.FromArgb((byte)(color.A - add), color.R, color.G, color.B));
            var a = color.A + add;
            if (a > 255) a = 255;
            Pressed = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
            return this;
        }
        public BrushRound Focused(Color color, int add = 40)
        {
            Mouse = new SolidColorBrush(color);
            var a = color.A + add;
            if (a > 255) a = 255;
            Pressed = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
            return this;
        }
        public bool Equals(BrushRound other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed);
        }
    }
    /// <summary>
    /// 字符串转BrushRound
    /// </summary>
    public class BrushRoundConverter : TypeConverter
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
            if (value is string str)
            {
                var strs = str.Split(';');
                Color? normal = null;
                Color? mouse = null;
                Color? pressed = null;
                int add = 40;
                if (strs.Length > 0 && !string.IsNullOrEmpty(strs[0])) normal = (Color)ColorConverter.ConvertFromString(strs[0]);
                if (strs.Length > 1 && !string.IsNullOrEmpty(strs[1])) mouse = (Color)ColorConverter.ConvertFromString(strs[1]);
                if (strs.Length > 2 && !string.IsNullOrEmpty(strs[2])) pressed = (Color)ColorConverter.ConvertFromString(strs[2]);
                if (strs.Length > 3 && !string.IsNullOrEmpty(strs[3])) add = Convert.ToInt32(strs[3], culture);
                return new BrushRound(normal, mouse, pressed, add);
            }
            return base.ConvertFrom(context, culture, value);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);
            return value.ToString();
        }
    }
}
