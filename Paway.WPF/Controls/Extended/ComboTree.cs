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
    /// ComboBox扩展树
    /// </summary>
    public class ComboTree : ComboBoxEXT
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IQueryProperty =
            DependencyProperty.RegisterAttached(nameof(IQuery), typeof(bool), typeof(ComboTree), new PropertyMetadata(false));
        /// <summary>
        /// 启用搜索框
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("启用搜索框")]
        public bool IQuery
        {
            get { return (bool)GetValue(IQueryProperty); }
            set { SetValue(IQueryProperty, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// 搜索过滤事件
        /// </summary>
        public event EventHandler<CustomFilterEventArgs> FilterEvent;

        #endregion

        #region 数据
        /// <summary>
        /// 选中项列表
        /// </summary>
        [Browsable(false)]
        public List<ITreeViewItem> CheckedItems { get { return treeView.CheckedItems; } }
        /// <summary>
        /// 普通项列表
        /// </summary>
        [Browsable(false)]
        public List<ITreeViewItem> NormalItems { get { return treeView.NormalItems; } }

        #endregion

        /// <summary>
        /// </summary>
        public ComboTree()
        {
            DefaultStyleKey = typeof(ComboTree);
            DependencyPropertyDescriptor.FromProperty(SelectedValueProperty, typeof(ComboTree)).AddValueChanged(this, OnSelectedValueChanged);
        }
        private void OnSelectedValueChanged(object sender, EventArgs e)
        {
            IEnumerable list = this.List ?? this.ItemsSource;
            if (list != null)
            {
                var id = this.SelectedValue.ToInt();
                foreach (ITreeViewItem item in list)
                {
                    if (this.InitText(item, id)) break;
                }
            }
        }
        private bool InitText(ITreeViewItem item, int id)
        {
            if (!item.IsGroup && item.Id == id)
            {
                this.SelectedItem = item;
                this.last = this.DisplayMemberPath.IsEmpty() ? item.ToString() : item.GetValue(this.DisplayMemberPath).ToStrings();
                this.Text = this.last;
                if (this.List != null) this.ItemsSource = this.List;
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

        #region 关联选择
        private bool isInit;
        private TreeViewEXT treeView;
        private Type type;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            IsEditable = false;
            if (this.ItemsSource != null) this.type = this.ItemsSource.GenericType();
            if (typeof(ITreeViewItem).IsAssignableFrom(type))
            {
                this.List = new List<ITreeViewItem>();
                foreach (ITreeViewItem item in this.ItemsSource) this.List.Add(item);
                LoadChilds(this.List);
            }
            if (Template.FindName("PART_Popup", this) is Popup popup)
            {
                popup.Opened -= Popup_Opened;
                popup.Opened += Popup_Opened;
            }
            if (Template.FindName("PART_TreeView", this) is TreeViewEXT treeView)
            {
                this.treeView = treeView;
                treeView.MouseDoubleClick -= TreeView_MouseDoubleClick;
                treeView.MouseDoubleClick += TreeView_MouseDoubleClick;
                treeView.SelectedItemChanged -= TreeView_SelectedItemChanged;
                treeView.SelectedItemChanged += TreeView_SelectedItemChanged;
            }
            if (PMethod.Parent(this, out Window window))
            {
                window.LocationChanged -= Window_LocationChanged;
                window.LocationChanged += Window_LocationChanged;
            }
            this.KeyUp -= ComboTree_KeyUp;
            this.KeyUp += ComboTree_KeyUp;
        }
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            this.IsDropDownOpen = false;
        }
        private void LoadChilds(IEnumerable<ITreeViewItem> list)
        {
            foreach (var item in list)
            {
                if (item.IsGroup) LoadChilds(item.Children);
                else this.Childs.Add(item);
            }
        }
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }
        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var treeView = sender as TreeViewEXT;
            if (treeView.SelectedItem is ITreeViewItem item && !item.IsGroup)
            {
                var id = this.SelectedValuePath.IsEmpty() ? item : item.GetValue(this.SelectedValuePath);
                if (this.SelectedValue.Equals(id))
                    this.Text = null;
                this.SelectedValue = id;
                this.IsDropDownOpen = false;
                textBox.Focus();
            }
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeViewEXT;
            if (e.NewValue is ITreeViewItem item)
            {
                if (!item.IsGroup)
                {
                    if (isInit && !this.IQuery)
                    {
                        this.SelectedValue = this.SelectedValuePath.IsEmpty() ? item : item.GetValue(this.SelectedValuePath);
                        PMethod.BeginInvoke(() => { this.IsDropDownOpen = false; });
                    }
                }
                else
                {
                    treeView.Expand(item);
                }
            }
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            isInit = treeView.Selected(this.SelectedValue.ToInt());
            if (isInit) textBox.Focus();
        }

        #endregion

        #region 搜索
        private IList<ITreeViewItem> List;
        private readonly IList<ITreeViewItem> Childs = new List<ITreeViewItem>();
        private string last;
        private void ComboTree_KeyUp(object sender, KeyEventArgs e)
        {
            var text = textBox.Text;
            if (last == text) return;
            last = text;
            if (text.IsEmpty())
            {
                this.ItemsSource = this.List;
            }
            else
            {
                if (FilterEvent != null)
                {
                    var args = new CustomFilterEventArgs(text, e.RoutedEvent, treeView);
                    FilterEvent.Invoke(this, args);
                    if (args.List != null)
                    {
                        this.ItemsSource = args.List;
                    }
                }
                else if (this.List != null)
                {
                    var list = this.Childs.FindLabbda(text, c => c.IsGroup);
                    var id = list.Count > 0 ? list[0].Id : 0;
                    list = LoadQuery(list);
                    this.ItemsSource = list;
                    if (id > 0)
                    {
                        PMethod.BeginInvoke(() =>
                        {
                            treeView.Selected(id);
                            textBox.Focus();
                        });
                    }
                }
            }
            IsDropDownOpen = true;
        }
        private List<ITreeViewItem> LoadQuery(List<ITreeViewItem> list)
        {
            var result = false;
            var resultList = new List<ITreeViewItem>();
            foreach (var item in list)
            {
                if (item.Parent != null)
                {
                    result = true;
                    var parent = resultList.Find(c => c.Id == item.Parent.Id);
                    if (parent == null)
                    {
                        parent = item.Parent.Clone();
                        parent.Parent = item.Parent.Parent.Clone();
                        resultList.Add(parent);
                    }
                    parent.Add(item);
                }
            }
            if (result) return LoadQuery(resultList);
            return list;
        }
        /// <summary>
        /// 按键快捷响应
        /// </summary>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!IQuery)
            {
                e.Handled = true;
                base.OnPreviewKeyDown(e);
                return;
            }
            switch (e.Key)
            {
                case Key.Right:
                    if (textBox.IsFocused && textBox.SelectionStart == textBox.Text.Length)
                    {
                        IsDropDownOpen = true;
                    }
                    break;
                case Key.Up:
                    if (IsDropDownOpen)
                    {
                        if (treeView.SelectedItem is IId item)
                        {
                            if (this.Items.Count > 0 && this.Items[0] is IId item2 && item.Id == item2.Id)
                            {
                                textBox.Focus();
                                return;
                            }
                        }
                        if (textBox.IsFocused && Selected())
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                    break;
                case Key.Down:
                    if (textBox.IsFocused)
                    {
                        if (!IsDropDownOpen)
                        {
                            IsDropDownOpen = true;
                            e.Handled = true;
                            return;
                        }
                        if (Selected())
                        {
                            e.Handled = true;
                            return;
                        }
                    }
                    break;
                case Key.Enter:
                    if (this.Items.Count > 0)
                    {
                        if (treeView.SelectedItem is IId)
                        {
                            TreeView_MouseDoubleClick(treeView, null);
                        }
                        else if (this.Items[0] is IId item2)
                        {
                            treeView.Selected(item2.Id);
                        }
                        TreeView_MouseDoubleClick(treeView, null);
                    }
                    break;
            }
            base.OnPreviewKeyDown(e);
        }
        private bool Selected()
        {
            if (treeView.SelectedItem is IId item)
            {
                return treeView.Selected(item.Id);
            }
            if (this.Items.Count > 0 && this.Items[0] is IId item2)
            {
                return treeView.Selected(item2.Id);
            }
            return false;
        }

        #endregion
    }
}
