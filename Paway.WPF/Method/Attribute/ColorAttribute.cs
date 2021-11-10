using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 特性.枚举颜色
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ColorAttribute : Attribute
    {
        /// <summary>
        /// 特性.枚举颜色
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// 特性.枚举颜色
        /// </summary>
        public ColorAttribute(byte r, byte g, byte b)
        {
            this.Color = Color.FromArgb(255, r, g, b);
        }
        /// <summary>
        /// 特性.枚举颜色
        /// </summary>
        public ColorAttribute(byte a, byte r, byte g, byte b)
        {
            this.Color = Color.FromArgb(a, r, g, b);
        }
    }
}
