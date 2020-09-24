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
    /// DataGridEXT双击路由事件参数
    /// </summary>
    public class RowDoubleEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 当前项
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// </summary>
        public RowDoubleEventArgs() { }
        /// <summary>
        /// </summary>
        public RowDoubleEventArgs(object item, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.Item = item;
        }
    }
}
