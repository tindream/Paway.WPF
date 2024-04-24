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
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// TreeView扩展
    /// </summary>
    public partial class TreeViewEXT : TreeView
    {
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
            new PropertyMetadata(new BrushEXT(null, PConfig.Foreground, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty UserModeProperty =
            DependencyProperty.RegisterAttached(nameof(UserMode), typeof(bool), typeof(TreeViewEXT));

        #endregion

        #region 扩展
        /// <summary>
        /// 选择框
        /// <para>默认值：Collapsed</para>
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
        /// <para>默认值：默认</para>
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
        /// <para>默认值：主题前景, 120, 200</para>
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
        /// <para>默认值：主题前景, 主题前景, White</para>
        /// </summary>
        [Category("扩展")]
        [Description("项字体颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }
        /// <summary>
        /// 用户模式
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("用户模式")]
        public bool UserMode
        {
            get { return (bool)GetValue(UserModeProperty); }
            set { SetValue(UserModeProperty, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 行双击路由事件
        /// </summary>
        public event EventHandler<SelectItemEventArgs> RowDoubleEvent;
        #region 节点拖动检查过滤路由事件
        /// <summary>
        /// 节点拖动检查过滤路由事件
        /// </summary>
        public event EventHandler<TreeDragEventArgs> DragFilter;
        /// <summary>
        /// 节点拖动检查过滤路由事件
        /// </summary>
        private bool? OnDragFilter(ITreeViewItem fromItem, ITreeViewItem toItem, RoutedEvent routed)
        {
            var args = new TreeDragEventArgs(fromItem, toItem, routed, this);
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
        public event EventHandler<TreeDragEventArgs> DragCompleted;
        /// <summary>
        /// 节点拖动完成路由事件
        /// </summary>
        private void OnDragCompleted(ITreeViewItem fromItem, ITreeViewItem toItem, RoutedEvent routed)
        {
            var args = new TreeDragEventArgs(fromItem, toItem, routed, this);
            DragCompleted?.Invoke(this, args);
        }

        #endregion
        #region 节点拖动外部路由事件
        /// <summary>
        /// 节点拖动外部路由事件
        /// </summary>
        public event EventHandler<DragEventArgs> DragExternal;
        /// <summary>
        /// 节点拖动外部路由事件
        /// </summary>
        private void OnDragExternal(DragEventArgs e)
        {
            DragExternal?.Invoke(this, e);
        }

        #endregion

        #endregion

        #region 数据
        /// <summary>
        /// 选中项列表
        /// </summary>
        [Browsable(false)]
        public List<ITreeViewItem> CheckedItems
        {
            get
            {
                var checkList = new List<ITreeViewItem>();
                if (this.ItemsSource is ObservableCollection<ITreeViewItem> list)
                {
                    GetCheckedItems(list, checkList, true);
                }
                return checkList;
            }
        }
        /// <summary>
        /// 普通项列表
        /// </summary>
        [Browsable(false)]
        public List<ITreeViewItem> NormalItems
        {
            get
            {
                var checkList = new List<ITreeViewItem>();
                if (this.ItemsSource is ObservableCollection<ITreeViewItem> list)
                {
                    GetCheckedItems(list, checkList);
                }
                return checkList;
            }
        }
        /// <summary>
        /// 获取列表下选中项
        /// </summary>
        public void GetCheckedItems(ObservableCollection<ITreeViewItem> list, List<ITreeViewItem> checkList, bool? isChecked = null)
        {
            foreach (var item in list)
            {
                if (item.IsGroup) GetCheckedItems(item.Children, checkList, isChecked);
                else if (isChecked == null || item.IsChecked == isChecked) checkList.Add(item);
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TreeViewEXT()
        {
            DefaultStyleKey = typeof(TreeViewEXT);
            this.MouseDoubleClick += TreeViewEXT_MouseDoubleClick;
        }
        private void TreeViewEXT_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PMethod.Parent(e.OriginalSource, out TreeViewItem item) && item.DataContext is ITreeViewItem treeItem && !treeItem.IsGroup)
            {
                RowDoubleEvent?.Invoke(this, new SelectItemEventArgs(item.DataContext, e.RoutedEvent, this));
            }
        }

        #region 拖拽节点
        /// <summary>
        /// 拖拽起点
        /// </summary>
        private Point? _lastMouseDown;
        private ITreeViewItem fromItem;
        /// <summary>
        /// 存在分组标记
        /// </summary>
        private bool IsGroup;
        /// <summary>
        /// 滚动条
        /// </summary>
        private ScrollViewerEXT scrollViewer;
        /// <summary>
        /// 滚动标记
        /// </summary>
        private string scrollFlag;
        /// <summary>
        /// 获取滚动条
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            scrollViewer = Template.FindName("Part_ScrollViewer", this) as ScrollViewerEXT;
        }
        /// <summary>
        /// 按下记录位置
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && this.AllowDrop && e.ClickCount == 1)
            {
                if (!(this.ItemsSource is ObservableCollection<ITreeViewItem> list))
                {
                    PMethod.Hit(this, "数据源需定义指定格式: ObservableCollection<ITreeViewItem>", ColorType.Error);
                    return;
                }
                _lastMouseDown = e.GetPosition(this);
                IsGroup = list.Any(c => c.IsGroup);
                if (scrollViewer != null) scrollFlag = $"V{scrollViewer.VerticalOffset}H{scrollViewer.HorizontalOffset}";
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
                if (scrollViewer != null && scrollFlag != $"V{scrollViewer.VerticalOffset}H{scrollViewer.HorizontalOffset}")
                {
                    _lastMouseDown = null;
                    return;
                }
                Point currentPosition = e.GetPosition(this);
                if ((Math.Abs(currentPosition.X - _lastMouseDown.Value.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Value.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    if (this.SelectedItem is ITreeViewItem item)
                    {
                        this.fromItem = item;
                        try
                        {
                            DragDrop.DoDragDrop(this, item, DragDropEffects.Move);
                        }
                        finally
                        {
                            fromItem = null;
                        }
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
            if (this.fromItem != null)
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
            if (toItem == null) return IsGroup && !fromItem.IsGroup;
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
            if (this.fromItem != null)
            {
                if (PMethod.Parent(e.OriginalSource, out TreeViewItem item) && item.DataContext is ITreeViewItem toItem)
                {
                    var iSame = fromItem.Parent == toItem.Parent;
                    RemoveItem(fromItem);
                    if (toItem.IsGroup)
                    {
                        if (fromItem.IsGroup)
                        {
                            var index = toItem.Children.ToList().FindLastIndex(c => c.IsGroup);
                            toItem.Insert(index + 1, fromItem);
                            OnDragCompleted(fromItem, null, e.RoutedEvent);
                        }
                        else
                        {
                            toItem.Add(fromItem);
                            OnDragCompleted(fromItem, null, e.RoutedEvent);
                        }
                    }
                    else if (toItem.Parent != null && toItem.Parent.IsGroup)
                    {
                        if (fromItem.IsGroup)
                        {
                            var index = toItem.Parent.Children.ToList().FindLastIndex(c => c.IsGroup);
                            toItem.Parent.Insert(index + 1, fromItem);
                            OnDragCompleted(fromItem, null, e.RoutedEvent);
                        }
                        else
                        {
                            var index = toItem.Parent.Children.IndexOf(toItem);
                            toItem.Parent.Insert(index, fromItem);
                            OnDragCompleted(fromItem, iSame ? toItem : null, e.RoutedEvent);
                        }
                    }
                    else if (!IsGroup && this.ItemsSource is ObservableCollection<ITreeViewItem> list)
                    {
                        var index = list.IndexOf(toItem);
                        list.Insert(index, fromItem);
                        OnDragCompleted(fromItem, toItem, e.RoutedEvent);
                    }
                }
                else if (this.ItemsSource is ObservableCollection<ITreeViewItem> list)
                {
                    RemoveItem(fromItem);
                    list.Add(fromItem);
                    OnDragCompleted(fromItem, null, e.RoutedEvent);
                }
            }
            else OnDragExternal(e);
            base.OnDrop(e);
        }
        private void RemoveItem(ITreeViewItem fromItem)
        {
            if (fromItem.Parent != null) fromItem.Parent.Remove(fromItem);
            else if (this.ItemsSource is ObservableCollection<ITreeViewItem> list) list.Remove(fromItem);
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
