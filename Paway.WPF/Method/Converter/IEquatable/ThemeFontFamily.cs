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
    /// 主题文本字体
    /// </summary>
    public class ThemeFontFamily : ModelBase, IEquatable<ThemeFontFamily>
    {
        private string _value;
        /// <summary>
        /// 默认值
        /// </summary>
        public string Value { get { return _value; } set { _value = value; OnPropertyChanged(); } }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
        /// <summary>
        /// </summary>
        public ThemeFontFamily()
        {
            PConfig.FontFamilyChanged += Config_FontFamilyChanged;
        }
        private void Config_FontFamilyChanged(string obj)
        {
            if (this.Value == obj)
            {
                this.Value = PConfig.FontFamily;
            }
        }
        /// <summary>
        /// </summary>
        public ThemeFontFamily(string value) : this()
        {
            this.Value = value;
        }
        /// <summary>
        /// </summary>
        public bool Equals(ThemeFontFamily other)
        {
            return Value.Equals(other.Value);
        }
    }
}
