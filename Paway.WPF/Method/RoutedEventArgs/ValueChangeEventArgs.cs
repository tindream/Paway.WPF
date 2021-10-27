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
}
