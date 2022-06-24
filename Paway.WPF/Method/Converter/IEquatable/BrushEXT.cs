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
    /// 自定义默认、鼠标划过时、鼠标点击时的Brush颜色
    /// <para>需要反射实体对象，获取默认值，故注意无限循环，不可引用自身</para>
    /// </summary>
    [TypeConverter(typeof(BrushEXTConverter))]
    public class BrushEXT : ModelBase, IEquatable<BrushEXT>, IElementStatu<Brush>
    {
        private Brush normal = Colors.LightGray.ToBrush();
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Brush Normal { get { return normal; } set { normal = value; OnPropertyChanged(); } }
        private Brush mouse = PMethod.ThemeColor(PConfig.Alpha - PConfig.Interval).ToBrush();
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Brush Mouse { get { return mouse; } set { mouse = value; OnPropertyChanged(); } }
        /// <summary>
        /// 鼠标划过时的未选中颜色
        /// </summary>
        public Brush MousePressed
        {
            get
            {
                var mouse = Mouse as SolidColorBrush;
                return PMethod.AlphaColor(mouse.Color.A + Alpha / 2, mouse.Color).ToBrush();
            }
        }

        private Brush pressed = PMethod.ThemeColor(PConfig.Alpha + PConfig.Interval).ToBrush();
        /// <summary>
        /// 鼠标点击时的颜色
        /// </summary>
        public Brush Pressed { get { return pressed; } set { pressed = value; OnPropertyChanged(); } }
        /// <summary>
        /// 鼠标划过时的选中颜色
        /// </summary>
        public Brush PressedMouse
        {
            get
            {
                var pressed = Pressed as SolidColorBrush;
                return PMethod.AlphaColor(pressed.Color.A - Alpha / 2, pressed.Color).ToBrush();
            }
        }
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = PConfig.Interval;
        internal bool IHigh { get; set; }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            if (Mouse.ToColor() != Normal.ToColor() || Pressed.ToColor() != Normal.ToColor()) return $"{Normal};{Mouse};{Pressed};{Alpha}";
            return $"{Normal}";
        }
        /// <summary>
        /// </summary>
        public BrushEXT()
        {
            PConfig.ColorChanged += Config_ColorChanged;
        }
        private void Config_ColorChanged(Color obj)
        {
            if (this.Normal is SolidColorBrush normal && normal.Color.R == obj.R && normal.Color.G == obj.G && normal.Color.B == obj.B)
            {
                if (normal.Color != Colors.Transparent && normal.Color != Colors.LightGray && normal.Color != Colors.DarkGray &&
                    normal.Color != Colors.White && normal.Color != PConfig.TextColor && normal.Color != PConfig.TextSub)
                {
                    this.Normal = PMethod.ThemeColor(normal.Color.A).ToBrush();
                }
            }
            if (this.Mouse is SolidColorBrush mouse && mouse.Color.R == obj.R && mouse.Color.G == obj.G && mouse.Color.B == obj.B)
            {
                if (mouse.Color != Colors.Transparent && mouse.Color != Colors.White && mouse.Color != PConfig.TextColor)
                {
                    this.Mouse = PMethod.ThemeColor(mouse.Color.A).ToBrush();
                }
            }
            if (this.Pressed is SolidColorBrush pressed && pressed.Color.R == obj.R && pressed.Color.G == obj.G && pressed.Color.B == obj.B)
            {
                if (pressed.Color != Colors.Transparent && pressed.Color != Colors.White)
                {
                    this.Pressed = PMethod.ThemeColor(pressed.Color.A).ToBrush();
                }
            }
            High();
        }
        /// <summary>
        /// 主题深色
        /// </summary>
        private void High()
        {
            if (IHigh)
            {
                this.Normal = PConfig.Background.AddLight(-30).ToBrush();
                this.Mouse = PConfig.Color.AddLight(0.96).ToBrush();
                this.Pressed = PConfig.Color.AddLight(-90).ToBrush();
            }
        }
        /// <summary>
        /// 主题背景
        /// </summary>
        public BrushEXT(bool iHigh) : this()
        {
            this.IHigh = iHigh;
            High();
        }
        /// <summary>
        /// 主题色：设置所有颜色，根据Alpha自动设置
        /// </summary>
        public BrushEXT(byte normal) : this(PMethod.ThemeColor(normal)) { }
        /// <summary>
        /// 主题色：普通、鼠标移过、按下
        /// </summary>
        public BrushEXT(Color? normal, byte mouse, byte pressed) : this(normal, PMethod.ThemeColor(mouse), PMethod.ThemeColor(pressed)) { }
        /// <summary>
        /// </summary>
        public BrushEXT(Color? normal, Color? mouse = null, Color? pressed = null, int? alpha = null, BrushEXT value = null) : this()
        {
            if (alpha != null) Alpha = alpha.Value;
            else if (value != null) Alpha = value.Alpha;

            if (normal != null) Normal = normal.Value.ToBrush();
            else if (value != null) Normal = value.Normal;

            if (mouse != null) Mouse = mouse.Value.ToBrush();
            else if (normal != null) Reset(normal.Value, Alpha);
            else if (value != null) Mouse = value.Mouse;

            if (pressed != null) Pressed = pressed.Value.ToBrush();
            else if (mouse != null) Focused(mouse.Value, Alpha);
            else if (normal == null && value != null) Pressed = value.Pressed;
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha变量
        /// </summary>
        private BrushEXT Reset(Color color, int alpha)
        {
            Normal = color.ToBrush();
            Mouse = PMethod.AlphaColor(color.A - alpha, color).ToBrush();
            Pressed = PMethod.AlphaColor(color.A + alpha, color).ToBrush();
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        private BrushEXT Focused(Color color, int alpha)
        {
            Mouse = color.ToBrush();
            Pressed = PMethod.AlphaColor(color.A + alpha * 2, color).ToBrush();
            return this;
        }
        /// <summary>
        /// </summary>
        public bool Equals(BrushEXT other)
        {
            return Normal.Equals(other.Normal) && Mouse.Equals(other.Mouse) && Pressed.Equals(other.Pressed) && Alpha.Equals(other.Alpha);
        }
    }
    /// <summary>
    /// 字符串转BrushEXT
    /// </summary>
    internal class BrushEXTConverter : TypeConverter
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
                var result = PMethod.ElementStatu<BrushEXT, Color>(context, culture, str, Parse, ParseValue);
                return new BrushEXT(result.Normal, result.Mouse, result.Pressed, result.Alpha, result.Old);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private Color? ParseValue(BrushEXT old, string name)
        {
            if (old == null) return null;
            return (old.GetValue(name) as SolidColorBrush)?.Color;
        }
        private Color Parse(string str)
        {
            if (byte.TryParse(str, out byte alpha))
            {
                return PMethod.ThemeColor(alpha);
            }
            return (Color)ColorConverter.ConvertFromString(str);
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
