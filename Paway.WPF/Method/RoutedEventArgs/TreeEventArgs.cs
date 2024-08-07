﻿using Paway.Helper;
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
    /// 树节点拖拽路由事件参数
    /// </summary>
    public class TreeDragEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 从某节点
        /// </summary>
        public ITreeViewItem FromItem { get; set; }
        /// <summary>
        /// 到某节点
        /// </summary>
        public ITreeViewItem ToItem { get; set; }
        /// <summary>
        /// 拖拽状态
        /// </summary>
        public DragType DragType { get; set; }
        /// <summary>
        /// 比较结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 树节点拖拽路由事件参数
        /// </summary>
        public TreeDragEventArgs() { }
        /// <summary>
        /// 树节点拖拽路由事件参数
        /// </summary>
        public TreeDragEventArgs(ITreeViewItem fromItem, ITreeViewItem toItem, DragType type, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.FromItem = fromItem;
            this.ToItem = toItem;
            this.DragType = type;
        }
    }
}
