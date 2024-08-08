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
    /// DataGrid节点拖拽路由事件参数
    /// </summary>
    public class DataGridDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 从某节点
        /// </summary>
        public DataGridRow FromItem { get; set; }
        /// <summary>
        /// 到某节点
        /// </summary>
        public DataGridRow ToItem { get; set; }
        /// <summary>
        /// 拖拽状态
        /// </summary>
        public DragType DragType { get; set; }
        /// <summary>
        /// 比较结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// DataGrid节点拖拽路由事件参数
        /// </summary>
        public DataGridDragEventArgs() { }
        /// <summary>
        /// DataGrid节点拖拽路由事件参数
        /// </summary>
        public DataGridDragEventArgs(DataGridRow fromItem, DataGridRow toItem, DragType type, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.FromItem = fromItem;
            this.ToItem = toItem;
            this.DragType = type;
        }
    }
}
