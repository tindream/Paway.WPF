using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
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
    /// 主题背景颜色
    /// </summary>
    [TypeConverter(typeof(ThemeBackgroundConverter))]
    public class ThemeBackground : ModelBase, IEquatable<ThemeBackground>
    {
        private Brush _value;
        /// <summary>
        /// 默认值
        /// </summary>
        public Brush Value { get { return _value; } set { _value = value; OnPropertyChanged(); } }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return $"{Value}";
        }
        /// <summary>
        /// </summary>
        public ThemeBackground()
        {
            PConfig.BackgroundChanged += Config_BackgroundChanged;
        }
        private void Config_BackgroundChanged(Color obj)
        {
            if (this.Value is SolidColorBrush value && value.Color.R == obj.R && value.Color.G == obj.G && value.Color.B == obj.B)
            {
                this.Value = new SolidColorBrush(PConfig.Background);
            }
        }
        /// <summary>
        /// </summary>
        public ThemeBackground(Color value) : this()
        {
            this.Value = new SolidColorBrush(value);
        }
        /// <summary>
        /// </summary>
        public bool Equals(ThemeBackground other)
        {
            return Value.Equals(other.Value);
        }
    }
    /// <summary>
    /// 字符串转ThemeBackground
    /// </summary>
    internal class ThemeBackgroundConverter : TypeConverter
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
                if (byte.TryParse(str, out byte alpha))
                {
                    return new ThemeBackground(PMethod.ThemeColor(alpha));
                }
                return new ThemeBackground((Color)ColorConverter.ConvertFromString(str));
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
