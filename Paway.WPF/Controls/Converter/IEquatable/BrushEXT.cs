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
    /// </summary>
    [TypeConverter(typeof(BrushEXTConverter))]
    public class BrushEXT : IEquatable<BrushEXT>, IElementStatu<Brush>, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// </summary>
        public void OnPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Method.GetLastModelName()));
        }

        #endregion

        private Brush normal = new SolidColorBrush(Colors.LightGray);
        /// <summary>
        /// 默认的颜色
        /// </summary>
        public Brush Normal { get { return normal; } set { normal = value; OnPropertyChanged(); } }
        private Brush mouse = new SolidColorBrush(Method.ThemeColor(210));
        /// <summary>
        /// 鼠标划过时的颜色
        /// </summary>
        public Brush Mouse { get { return mouse; } set { mouse = value; OnPropertyChanged(); } }
        private Brush pressed = new SolidColorBrush(Method.ThemeColor(210));
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
                var alpha = pressed.Color.A - 20;
                if (alpha < 0) alpha = 0;
                return new SolidColorBrush(Color.FromArgb((byte)alpha, pressed.Color.R, pressed.Color.G, pressed.Color.B));
            }
        }
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        public int Alpha { get; set; } = 50;
        internal bool IHigh { get; set; }

        /// <summary>
        /// </summary>
        public BrushEXT()
        {
            Config.ColorChanged += Config_ColorChanged;
        }
        private void Config_ColorChanged(Color obj)
        {
            if (obj != Colors.LightGray && obj != Colors.Gray && this.Normal is SolidColorBrush normal && normal.Color.R == obj.R && normal.Color.G == obj.G && normal.Color.B == obj.B)
            {
                this.Normal = new SolidColorBrush(Method.ThemeColor(normal.Color.A));
            }
            if (this.Mouse is SolidColorBrush mouse && mouse.Color.R == obj.R && mouse.Color.G == obj.G && mouse.Color.B == obj.B)
            {
                this.Mouse = new SolidColorBrush(Method.ThemeColor(mouse.Color.A));
            }
            if (this.Pressed is SolidColorBrush pressed && pressed.Color.R == obj.R && pressed.Color.G == obj.G && pressed.Color.B == obj.B)
            {
                this.Pressed = new SolidColorBrush(Method.ThemeColor(pressed.Color.A));
            }
            High();
        }
        /// <summary>
        /// 主题深色
        /// </summary>
        public BrushEXT(bool high) : this()
        {
            this.IHigh = high;
            High();
        }
        /// <summary>
        /// 主题深色
        /// </summary>
        private void High()
        {
            if (IHigh)
            {
                this.Normal = new SolidColorBrush(Config.Color.AddLight(0.985));
                this.Mouse = new SolidColorBrush(Config.Color.AddLight(0.95));
                this.Pressed = new SolidColorBrush(Config.Color.AddLight(-60));
            }
        }
        /// <summary>
        /// 主题色：设置所有颜色，自动Alpha变量
        /// </summary>
        public BrushEXT(byte normal) : this(Method.ThemeColor(normal)) { }
        /// <summary>
        /// 主题色：鼠标移过、按下
        /// </summary>
        public BrushEXT(byte mouse, byte pressed) : this(null, mouse, pressed) { }
        /// <summary>
        /// 主题色：设置所有颜色，普通、鼠标移过、按下
        /// </summary>
        public BrushEXT(byte normal, byte mouse, byte pressed, bool iHigh = false) : this(Method.ThemeColor(normal), mouse, pressed) { }
        /// <summary>
        /// 主题色：普通、鼠标移过、按下
        /// </summary>
        public BrushEXT(Color? normal, byte mouse, byte pressed) : this(normal, Method.ThemeColor(mouse), Method.ThemeColor(pressed)) { }
        /// <summary>
        /// </summary>
        public BrushEXT(Color? normal, Color? mouse = null, Color? pressed = null, int? alpha = null, BrushEXT value = null) : this()
        {
            if (alpha != null) Alpha = alpha.Value;
            else if (value != null) Alpha = value.Alpha;

            if (normal != null) Normal = new SolidColorBrush(normal.Value);
            else if (value != null) Normal = value.Normal;

            if (mouse != null) Mouse = new SolidColorBrush(mouse.Value);
            else if (normal != null) Reset(normal.Value, Alpha);
            else if (value != null) Mouse = value.Mouse;

            if (pressed != null) Pressed = new SolidColorBrush(pressed.Value);
            else if (mouse != null) Focused(mouse.Value, Alpha);
            else if (normal == null && value != null) Pressed = value.Pressed;
        }
        /// <summary>
        /// 设置所有颜色，指定Alpha变量
        /// </summary>
        private BrushEXT Reset(Color color, int alpha)
        {
            Normal = new SolidColorBrush(color);
            var a = color.A - alpha;
            if (a < 0) a = 0;
            Mouse = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
            a = color.A + alpha;
            if (color == Colors.Transparent) a = 0;
            else if (a > 255) a = 255;
            Pressed = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
            return this;
        }
        /// <summary>
        /// 设置鼠标划过、点击时的颜色
        /// </summary>
        private BrushEXT Focused(Color color, int alpha)
        {
            Mouse = new SolidColorBrush(color);
            var a = color.A + alpha;
            if (color == Colors.Transparent) a = 0;
            else if (a > 255) a = 255;
            Pressed = new SolidColorBrush(Color.FromArgb((byte)a, color.R, color.G, color.B));
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
                var result = Method.ElementStatu<BrushEXT, Color>(context, culture, str, Parse, ParseValue);
                return new BrushEXT(result.Item2, result.Item3, result.Item4, result.Item5, result.Item1);
            }
            return base.ConvertFrom(context, culture, value);
        }
        private Color? ParseValue(BrushEXT old, string name)
        {
            if (old == null) return null;
            return (old.GetValue(name) as SolidColorBrush).Color;
        }
        private Color Parse(string str)
        {
            if (byte.TryParse(str, out byte alpha))
            {
                return Method.ThemeColor(alpha);
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
