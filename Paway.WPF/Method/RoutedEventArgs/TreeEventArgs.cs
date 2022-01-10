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
    /// 树节点拖拽完成路由事件参数
    /// </summary>
    public class TreeDragCompletedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 父节点(为空时则为根)
        /// </summary>
        public ITreeViewItem TreeItem { get; set; }

        /// <summary>
        /// 树节点拖拽完成路由事件参数
        /// </summary>
        public TreeDragCompletedEventArgs() { }
        /// <summary>
        /// 树节点拖拽完成路由事件参数
        /// </summary>
        public TreeDragCompletedEventArgs(ITreeViewItem treeItem, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.TreeItem = treeItem;
        }
    }

    /// <summary>
    /// 树节点拖拽过滤路由事件参数
    /// </summary>
    public class TreeFilterEventArgs : RoutedEventArgs
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
        /// 比较结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 树节点拖拽过滤路由事件参数
        /// </summary>
        public TreeFilterEventArgs() { }
        /// <summary>
        /// 树节点拖拽过滤路由事件参数
        /// </summary>
        public TreeFilterEventArgs(ITreeViewItem fromItem, ITreeViewItem toItem, RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
            this.FromItem = fromItem;
            this.ToItem = toItem;
        }
    }
}
