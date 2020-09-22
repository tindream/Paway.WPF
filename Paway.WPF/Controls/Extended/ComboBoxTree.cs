using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// ComboBox扩展树
    /// </summary>
    public class ComboBoxTree : ComboBoxEXT
    {
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedValueProperty =
            DependencyProperty.RegisterAttached(nameof(ComboBoxTree.SelectedValue), typeof(object), typeof(ComboBoxTree), new PropertyMetadata(OnSelectedValueChanged));
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached(nameof(ComboBoxTree.SelectedItem), typeof(object), typeof(ComboBoxTree));

        /// <summary>
        /// </summary>
        public ComboBoxTree()
        {
            DefaultStyleKey = typeof(ComboBoxTree);
        }
        /// <summary>
        /// 重写
        /// </summary>
        public new object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        /// <summary>
        /// 重写
        /// </summary>
        public new object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBoxTree tree)
            {
                if (tree.ItemsSource != null)
                {
                    var id = tree.SelectedValue.ToInt();
                    foreach (ITreeView item in tree.ItemsSource)
                    {
                        if (tree.InitText(item, id)) break;
                    }
                }
            }
        }

        #region 关联选择
        private bool isInit;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template.FindName("PART_TreeView", this) is TreeViewEXT treeView)
            {
                treeView.MouseDoubleClick += TreeView_MouseDoubleClick;
                treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
                treeView.Loaded += TreeView_Loaded;
            }
        }
        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var treeView = sender as TreeViewEXT;
            if (treeView.SelectedItem is ITreeView treeItem && !treeItem.IsGrouping)
                this.IsDropDownOpen = false;
        }
        private bool InitText(ITreeView item, int id)
        {
            if (!item.IsGrouping && item.Id == id)
            {
                this.SelectedItem = item;
                this.Text = item.GetValue(this.DisplayMemberPath).ToStrs();
                return true;
            }
            else if (item.Children.Count > 0)
            {
                foreach (var temp in item.Children)
                {
                    if (InitText(temp, id)) return true;
                }
            }
            return false;
        }
        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            var treeView = sender as TreeViewEXT;
            isInit = treeView.Selected(this.SelectedValue.ToInt());
            if (isInit) treeView.Loaded -= TreeView_Loaded;
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeViewEXT;
            if (e.NewValue is ITreeView item)
            {
                if (!item.IsGrouping)
                {
                    if (isInit)
                    {
                        this.SelectedValue = item.GetValue(this.SelectedValuePath);
                        this.Dispatcher.BeginInvoke(new Action(() => { this.IsDropDownOpen = false; }));
                    }
                }
                else
                {
                    treeView.Expand(item);
                }
            }
        }

        #endregion
    }
}
