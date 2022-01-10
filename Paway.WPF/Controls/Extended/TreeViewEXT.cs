using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// TreeView扩展
    /// </summary>
    public partial class TreeViewEXT : TreeView
    {
        #region 属性
        /// <summary>
        /// 选中项列表
        /// </summary>
        [Browsable(false)]
        public IList<ITreeViewItem> ChekedItems
        {
            get
            {
                var list = new List<ITreeViewItem>();
                foreach (ITreeViewItem item in this.ItemsSource)
                {
                    if (item.IsChecked == true) list.Add(item);
                }
                return list;
            }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ICheckBoxProperty =
            DependencyProperty.RegisterAttached(nameof(ICheckBox), typeof(Visibility), typeof(TreeViewEXT),
                new PropertyMetadata(Visibility.Collapsed));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TreeViewEXT),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(TreeViewEXT),
            new PropertyMetadata(new BrushEXT(null, PConfig.Alpha - PConfig.Interval * 2, PConfig.Alpha)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(TreeViewEXT),
            new PropertyMetadata(new BrushEXT(PConfig.TextColor, PConfig.TextColor, Colors.White)));

        #endregion

        #region 扩展
        /// <summary>
        /// 选择框
        /// </summary>
        [Category("扩展")]
        [Description("选择框")]
        public Visibility ICheckBox
        {
            get { return (Visibility)GetValue(ICheckBoxProperty); }
            set { SetValue(ICheckBoxProperty, value); }
        }
        /// <summary>
        /// 项边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("项边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 项背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("项背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }
        /// <summary>
        /// 项字体颜色
        /// </summary>
        [Category("扩展")]
        [Description("项字体颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TreeViewEXT()
        {
            DefaultStyleKey = typeof(TreeViewEXT);
        }

        #region 拖拽节点
        #region 节点拖动检查过滤路由事件
        /// <summary>
        /// 节点拖动检查过滤路由事件
        /// </summary>
        public event EventHandler<TreeFilterEventArgs> DragFilter;
        /// <summary>
        /// 节点拖动检查过滤路由事件
        /// </summary>
        private bool? OnDragFilter(ITreeViewItem fromItem, ITreeViewItem toItem, RoutedEvent routed)
        {
            var args = new TreeFilterEventArgs(fromItem, toItem, routed, this);
            if (DragFilter != null)
            {
                DragFilter.Invoke(this, args);
                return args.Result;
            }
            return null;
        }

        #endregion
        #region 节点拖动完成路由事件
        /// <summary>
        /// 节点拖动完成路由事件
        /// </summary>
        public event EventHandler<TreeDragCompletedEventArgs> DragCompleted;
        /// <summary>
        /// 节点拖动完成路由事件
        /// </summary>
        private void OnDragCompleted(ITreeViewItem treeItem, RoutedEvent routed)
        {
            var args = new TreeDragCompletedEventArgs(treeItem, routed, this);
            DragCompleted?.Invoke(this, args);
        }

        #endregion
        private Point? _lastMouseDown;
        /// <summary>
        /// 按下记录位置
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && this.AllowDrop)
            {
                _lastMouseDown = e.GetPosition(this);
            }
            base.OnPreviewMouseLeftButtonDown(e);
        }
        /// <summary>
        /// 抬起停止拖动
        /// </summary>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_lastMouseDown != null)
            {
                _lastMouseDown = null;
                if (PMethod.Parent(this, out Window window)) window.Cursor = null;
                //if (PMethod.Parent(this, out Window window)) window.Cursor = Cursors.Hand;
            }
            base.OnPreviewMouseLeftButtonUp(e);
        }
        /// <summary>
        /// 启动拖动
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _lastMouseDown != null)
            {
                Point currentPosition = e.GetPosition(this);
                if ((Math.Abs(currentPosition.X - _lastMouseDown.Value.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Value.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    if (this.SelectedItem is ITreeViewItem item)
                    {
                        DragDrop.DoDragDrop(this, item, DragDropEffects.Move);
                    }
                }
            }
            base.OnPreviewMouseMove(e);
        }
        /// <summary>
        /// 拖动进入时检查状态
        /// </summary>
        protected override void OnDragEnter(DragEventArgs e)
        {
            DragCheck(e);
            base.OnDragEnter(e);
        }
        /// <summary>
        /// 拖动离开时检查状态
        /// </summary>
        protected override void OnDragLeave(DragEventArgs e)
        {
            DragCheck(e);
            base.OnDragLeave(e);
        }
        /// <summary>
        /// 拖动过程中检查状态
        /// </summary>
        protected override void OnDragOver(DragEventArgs e)
        {
            DragCheck(e);
            base.OnDragOver(e);
        }
        private void DragCheck(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TreeViewItemModel)) is ITreeViewItem fromItem)
            {
                if (PMethod.Parent(e.OriginalSource, out TreeViewItem item) && item.DataContext is ITreeViewItem toItem)
                {
                    if (IsFilter(fromItem, toItem, e.RoutedEvent))
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                    }
                }
                else if (IsFilter(fromItem, null, e.RoutedEvent))
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
                }
            }
        }
        private bool IsFilter(ITreeViewItem fromItem, ITreeViewItem toItem, RoutedEvent routed)
        {
            var result = OnDragFilter(fromItem, toItem, routed);
            if (result != null) return result.Value;
            if (toItem == null) return !fromItem.IsGroup;
            if (fromItem.IsGroup == toItem.IsGroup && fromItem.Id == toItem.Id) return true;
            foreach (var item in fromItem.Children.ToList())
            {
                if (IsFilter(item, toItem, routed)) return true;
            }
            return false;
        }
        /// <summary>
        /// 开始拖动-完成
        /// </summary>
        protected override void OnDrop(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TreeViewItemModel)) is TreeViewItemModel fromItem)
            {
                if (PMethod.Parent(e.OriginalSource, out TreeViewItem item) && item.DataContext is ITreeViewItem toItem)
                {
                    if (toItem.IsGroup)
                    {
                        RemoveItem(fromItem);
                        toItem.Add(fromItem);
                        OnDragCompleted(toItem, e.RoutedEvent);
                    }
                    else if (toItem.Parent != null && toItem.Parent.IsGroup)
                    {
                        var index = toItem.Parent.Children.IndexOf(toItem);
                        RemoveItem(fromItem);
                        toItem.Parent.Insert(index, fromItem);
                        OnDragCompleted(toItem.Parent, e.RoutedEvent);
                    }
                }
                else if (this.ItemsSource is ObservableCollection<TreeViewItemModel> list)
                {
                    RemoveItem(fromItem);
                    list.Add(fromItem);
                    OnDragCompleted(null, e.RoutedEvent);
                }
            }
            base.OnDrop(e);
        }
        private void RemoveItem(TreeViewItemModel fromItem)
        {
            if (fromItem.Parent != null) fromItem.Parent.Remove(fromItem);
            else if (this.ItemsSource is ObservableCollection<TreeViewItemModel> list) list.Remove(fromItem);
        }

        #endregion

        #region 展开/收缩节点
        /// <summary>
        /// 收缩全部节点
        /// </summary>
        public void CollapseAll()
        {
            CollapseAll(this);
        }
        private void CollapseAll(ItemsControl view)
        {
            foreach (var item in view.Items)
            {
                if (view.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeItem)
                {
                    treeItem.IsExpanded = false;
                    if (treeItem.HasItems)
                    {
                        CollapseAll(treeItem);
                    }
                }
            }
        }
        /// <summary>
        /// 展开全部节点
        /// </summary>
        public void ExpandAll()
        {
            foreach (var item in this.Items)
            {
                if (this.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeItem)
                {
                    treeItem.ExpandSubtree();
                }
            }
        }
        /// <summary>
        /// 展开指定节点
        /// </summary>
        public bool Expand(ITreeViewItem model)
        {
            return Selected(model.Id, false, model.IsGroup);
        }
        /// <summary>
        /// 选中指定节点
        /// </summary>
        public bool Selected(int id, bool iSelected = true, bool? iGroup = null)
        {
            return Expand(this, id, iSelected, iGroup);
        }
        private bool Expand(ItemsControl view, int id, bool iSelected, bool? iGroup = null)
        {
            foreach (var item in view.Items)
            {
                if (item is ITreeViewItem temp)
                {
                    if (Expand(view, temp, id, iSelected, iGroup)) return true;
                }
            }
            return false;
        }
        private bool Expand(ItemsControl view, ITreeViewItem item, int id, bool iSelected, bool? iGroup = null)
        {
            if (view.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeItem)
            {
                if (item.Id == id && (iGroup == null || iGroup.Value == item.IsGroup))
                {
                    treeItem.IsExpanded = true;
                    if (iSelected)
                    {
                        treeItem.IsSelected = true;
                        treeItem.BringIntoView();//滚动条滚动到选中的子元素
                        treeItem.Focus();
                    }
                    return true;
                }
                else if (item.Children.Count > 0)
                {
                    var oldExpanded = treeItem.IsExpanded;
                    if (iSelected)
                    {
                        treeItem.IsExpanded = true;
                        if (treeItem.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                        {
                            treeItem.UpdateLayout();
                        }
                        //treeItem.ExpandSubtree();
                    }
                    if (Expand(treeItem, id, iSelected, iGroup)) return true;
                    else treeItem.IsExpanded = oldExpanded;
                }
            }
            return false;
        }

        #endregion
    }
}
