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
    /// 自定义搜索过滤器路由事件参数
    /// </summary>
    public class CustomFilterEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 过滤输入
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 结果列表
        /// </summary>
        public IList List { get; set; }

        /// <summary>
        /// 自定义搜索过滤器路由事件参数
        /// </summary>
        public CustomFilterEventArgs() { }
        /// <summary>
        /// 自定义搜索过滤器路由事件参数
        /// </summary>
        public CustomFilterEventArgs(string filter, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.Filter = filter;
        }
    }
}
