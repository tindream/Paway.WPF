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
        private Point? _lastMouseDown;
        public event Func<ITreeViewItem, ITreeViewItem, bool> DragChecked;
        private bool OnDragChecked(ITreeViewItem fromItem, ITreeViewItem toItem)
        {
            return DragChecked?.Invoke(fromItem, toItem) == true;
        }
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
            if (PMethod.Parent(e.OriginalSource, out TreeViewItem item) && item.DataContext is ITreeViewItem toItem)
            {
                if (e.Data.GetData(typeof(TreeViewItemModel)) is ITreeViewItem fromItem)
                {
                    if (IsExist(fromItem, toItem))
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                    }
                }
            }
        }
        private bool IsExist(ITreeViewItem fromItem, ITreeViewItem toItem)
        {
            if (fromItem.IsGroup == toItem.IsGroup && fromItem.Id == toItem.Id) return true;
            if (OnDragChecked(fromItem, toItem)) return true;
            foreach (var item in fromItem.Children.ToList())
            {
                if (IsExist(item, toItem)) return true;
            }
            return false;
        }
        /// <summary>
        /// 开始拖动-完成
        /// </summary>
        protected override void OnDrop(DragEventArgs e)
        {
            if (PMethod.Parent(e.OriginalSource, out TreeViewItem item) && item.DataContext is ITreeViewItem toItem)
            {
                if (e.Data.GetData(typeof(TreeViewItemModel)) is TreeViewItemModel fromItem)
                {
                    if (toItem.IsGroup)
                    {
                        MoveItem(fromItem, toItem);
                    }
                    else
                    {
                        toItem = toItem.Parent;
                        if (toItem != null) MoveItem(fromItem, toItem);
                        else if (this.ItemsSource is ObservableCollection<TreeViewItemModel> list) list.Add(fromItem);
                    }
                }
            }
            base.OnDrop(e);
        }
        private void MoveItem(TreeViewItemModel fromItem, ITreeViewItem toItem)
        {
            if (toItem.IsGroup)
            {
                if (fromItem.Parent != null) fromItem.Parent.Remove(fromItem);
                else if (this.ItemsSource is ObservableCollection<TreeViewItemModel> list) list.Remove(fromItem);
                toItem.Add(fromItem);
            }
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
