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
    public class ThemeForeground : BaseModelInfo, IEquatable<ThemeForeground>
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
        public ThemeForeground()
        {
            PConfig.ForegroundChanged += Config_ForegroundChanged;
        }
        private void Config_ForegroundChanged(Color obj)
        {
            if (this.Value is SolidColorBrush value && value.Color.R == obj.R && value.Color.G == obj.G && value.Color.B == obj.B)
            {
                this.Value = PConfig.Foreground.ToBrush();
            }
        }
        /// <summary>
        /// </summary>
        public ThemeForeground(Color value) : this()
        {
            this.Value = value.ToBrush();
        }
        /// <summary>
        /// </summary>
        public bool Equals(ThemeForeground other)
        {
            return Value.Equals(other.Value);
        }
    }
}
