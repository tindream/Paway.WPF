using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace Paway.WPF
{
    /// <summary>
    /// 值更新路由事件参数
    /// </summary>
    public class ValueChangeEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 值更新路由事件参数
        /// </summary>
        public ValueChangeEventArgs() { }
        /// <summary>
        /// 值更新路由事件参数
        /// </summary>
        public ValueChangeEventArgs(double value, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.Value = value;
        }
    }
    /// <summary>
    /// 值更新显示路由事件参数
    /// </summary>
    public class ValuesChangeEventArgs : ValueChangeEventArgs
    {
        /// <summary>
        /// 显示值
        /// </summary>
        public string Values { get; set; }
        /// <summary>
        /// 小数位
        /// </summary>
        public int AutoToolTipPrecision { get; set; }

        /// <summary>
        /// 值更新显示路由事件参数
        /// </summary>
        public ValuesChangeEventArgs() { }
        /// <summary>
        /// 值更新显示路由事件参数
        /// </summary>
        public ValuesChangeEventArgs(double value, int precision, RoutedEvent routedEvent, object source) : base(value, routedEvent, source)
        {
            this.AutoToolTipPrecision = precision;
        }
    }
}
