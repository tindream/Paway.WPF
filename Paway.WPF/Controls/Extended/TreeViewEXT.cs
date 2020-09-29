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
        public IList<ITreeView> ChekedItems
        {
            get
            {
                var list = new List<ITreeView>();
                foreach (ITreeView item in this.ItemsSource)
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
        public static readonly DependencyProperty CheckBoxProperty =
            DependencyProperty.RegisterAttached(nameof(CheckBox), typeof(Visibility), typeof(TreeViewEXT),
                new PropertyMetadata(Visibility.Collapsed));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TreeViewEXT),
                new PropertyMetadata(new BrushEXT(170, 250)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(TreeViewEXT),
            new PropertyMetadata(new BrushEXT(85, 170)));

        #endregion

        #region 扩展
        /// <summary>
        /// 选择框
        /// </summary>
        [Category("扩展")]
        [Description("选择框")]
        public Visibility CheckBox
        {
            get { return (Visibility)GetValue(CheckBoxProperty); }
            set { SetValue(CheckBoxProperty, value); }
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

        #endregion

        /// <summary>
        /// </summary>
        public TreeViewEXT()
        {
            DefaultStyleKey = typeof(TreeViewEXT);
        }
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
        public bool Expand(ITreeView model)
        {
            return Selected(model.Id, false);
        }
        /// <summary>
        /// 选中指定节点
        /// </summary>
        public bool Selected(int id, bool iSelected = true)
        {
            return Expand(this, id, iSelected);
        }
        private bool Expand(ItemsControl view, int id, bool iSelected)
        {
            foreach (var item in view.Items)
            {
                if (item is ITreeView temp)
                {
                    if (Expand(view, temp, id, iSelected)) return true;
                }
            }
            return false;
        }
        private bool Expand(ItemsControl view, ITreeView item, int id, bool iSelected)
        {
            if (view.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeItem)
            {
                if (item.Id == id)
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
                    if (Expand(treeItem, id, iSelected)) return true;
                    else treeItem.IsExpanded = oldExpanded;
                }
            }
            return false;
        }
    }
}
