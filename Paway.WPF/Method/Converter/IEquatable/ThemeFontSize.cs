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
    /// 主题字体大小
    /// </summary>
    [TypeConverter(typeof(ThemeFontSizeConverter))]
    public class ThemeFontSize : BaseModelInfo, IEquatable<ThemeFontSize>
    {
        private double _value;
        /// <summary>
        /// 默认值
        /// </summary>
        public double Value { get { return _value; } set { _value = value; OnPropertyChanged(); } }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return $"{Value}";
        }
        /// <summary>
        /// </summary>
        public ThemeFontSize()
        {
            PConfig.FontSizeChangedWeak.Subscribe(Config_FontSizeChanged);
        }
        private void Config_FontSizeChanged(double obj)
        {
            if (this.Value == obj)
            {
                this.Value = PConfig.FontSize;
            }
        }
        /// <summary>
        /// </summary>
        public ThemeFontSize(double value) : this()
        {
            this.Value = value;
        }
        /// <summary>
        /// </summary>
        public bool Equals(ThemeFontSize other)
        {
            if (other is null) return false;//x.Equals(null) 会选中强类型重载，原实现直接 NRE
            return Value.Equals(other.Value);
        }
    }
    /// <summary>
    /// 字符串转ThemeFontSize
    /// </summary>
    internal class ThemeFontSizeConverter : TypeConverter
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
                return new ThemeFontSize(@double);
            }
            if (value is string str)
            {
                return new ThemeFontSize(str.ToDouble());
            }
            return base.ConvertFrom(context, culture, value);
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
