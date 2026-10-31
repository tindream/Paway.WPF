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
            //原先用“托管线程Id是否为1”判断UI线程：副UI线程(每个窗口独立Dispatcher)、单元测试、
            //宿主从非主线程启动时线程Id都不是1，会误判并抛异常，且异常点远离调用栈。
            //改为真正的UI线程判断：Application未创建时无法判断，放行。
            var app = Application.Current;
            if (app != null && !app.Dispatcher.CheckAccess() && !InDesignMode)
            {
                throw new WarningException("Resources cannot be called in threads");
            }
        }
        /// <summary>
        /// 是否处于设计器模式（避免每次构造都新建DependencyObject探测）
        /// </summary>
        private static readonly bool InDesignMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
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
                PConfig.BackgroundChangedWeak.Subscribe(Config_BackgroundChanged);
            }
            else
            {
                PConfig.ForegroundChangedWeak.Subscribe(Config_ForegroundChanged);
            }
        }
        /// <summary>
        /// </summary>
        public bool Equals(ThemeForeground other)
        {
            //x.Equals(null) 会优先选中强类型重载，原实现直接 NRE
            if (other is null) return false;
            return Equals(Result, other.Result);
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
            //原实现两个分支写反：非 string 目标返回了字符串。目标为 string 时取 ToString，其余交给基类
            if (destinationType == typeof(string)) return value?.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
