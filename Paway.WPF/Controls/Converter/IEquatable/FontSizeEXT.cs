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
    /// 自定义默认、鼠标划过时、鼠标点击时的大小
    /// </summary>
    [TypeConverter(typeof(FontSizeConverter))]
    public class FontSizeEXT : ModelBase, IEquatable<FontSizeEXT>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// </summary>
        public FontSizeEXT()
        {
            Config.FontSizeChanged += Config_FontSizeChanged;
        }
        private void Config_FontSizeChanged(double obj)
        {
            if (this.Value == obj)
            {
                this.Value = Config.FontSize;
                OnPropertyChanged(nameof(Value));
            }
        }
        /// <summary>
        /// </summary>
        public FontSizeEXT(double value) : this()
        {
            this.Value = value;
        }
        /// <summary>
        /// </summary>
        public bool Equals(FontSizeEXT other)
        {
            return Value.Equals(other.Value);
        }
    }
}
