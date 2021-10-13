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
    /// 选择项路由事件参数
    /// </summary>
    public class SelectItemEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 当前项
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// 选择项路由事件参数
        /// </summary>
        public SelectItemEventArgs() { }
        /// <summary>
        /// 选择项路由事件参数
        /// </summary>
        public SelectItemEventArgs(object item, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.Item = item;
        }
    }
}
