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
    /// 主题前景色
    /// </summary> 
    [TypeConverter(typeof(ThemeForegroundConverter))]
    public class ThemeForeground : BaseModelInfo, IEquatable<ThemeForeground>
    {
        private Brush _result;
        /// <summary>
        /// 显示值
        /// </summary>
        public Brush Result { get { return _result; } set { _result = value; OnPropertyChanged(); } }

        private SolidColorBrush _value;
        /// <summary>
        /// 设置值
        /// </summary>
        public SolidColorBrush Value { get { return _value; } set { _value = value; Result = value.Color == Colors.Transparent ? value.Color.ToBrush() : value.Color.AddLight(Light).ToBrush(); } }
        private int _light;
        /// <summary>
        /// 浅色比例
        /// </summary>
        public int Light { get { return _light; } set { _light = value; Result = Value.Color == Colors.Transparent ? Value.Color.ToBrush() : Value.Color.AddLight(Light).ToBrush(); } }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return $"{Result}";
        }
        /// <summary>
        /// </summary>
        public ThemeForeground()
        {
            if (System.Threading.Thread.CurrentThread.ManagedThreadId != 1)
            {
                throw new WarningException("Resources cannot be called in threads");
            }
        }
        private void Config_ForegroundChanged(Color obj)
        {
            if (this.Value is SolidColorBrush value && value.Color.R == obj.R && value.Color.G == obj.G && value.Color.B == obj.B)
            {
                this.Value = PConfig.Foreground.ToBrush();
            }
        }
        private void Config_BackgroundChanged(Color obj)
        {
            if (this.Value is SolidColorBrush value && value.Color.R == obj.R && value.Color.G == obj.G && value.Color.B == obj.B)
            {
                this.Value = PConfig.Background.ToBrush();
            }
        }
        /// <summary>
        /// </summary>
        public ThemeForeground(Color value, int light = 0, bool iBack = false) : this()
        {
            this._light = light;
            this.Value = value.ToBrush();
            if (iBack)
            {
                PConfig.BackgroundChanged += Config_BackgroundChanged;
            }
            else
            {
                PConfig.ForegroundChanged += Config_ForegroundChanged;
            }
        }
        /// <summary>
        /// </summary>
        public bool Equals(ThemeForeground other)
        {
            return Result.Equals(other.Result);
        }
    }
    /// <summary>
    /// 字符串转ThemeBackground
    /// </summary>
    internal class ThemeForegroundConverter : TypeConverter
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
                    return new ThemeForeground(PMethod.ThemeColor(alpha));
                }
                return new ThemeForeground(str.ToColor());
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
