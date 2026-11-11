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
    /// ComboBox扩展树
    /// </summary>
    public class ComboTree : ComboBoxEXT
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedValueProperty =
            DependencyProperty.RegisterAttached(nameof(SelectedValue), typeof(object), typeof(ComboTree), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached(nameof(SelectedItem), typeof(object), typeof(ComboTree),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IQueryProperty =
            DependencyProperty.RegisterAttached(nameof(IQuery), typeof(bool), typeof(ComboTree), new PropertyMetadata(false));

        /// <summary>
        /// 获取或设置的值 System.Windows.Controls.Primitives.Selector.SelectedItem, ，获得通过 System.Windows.Controls.Primitives.Selector.SelectedValuePath。
        /// <para>重写</para>
        /// </summary>
        public new object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }
        /// <summary>
        /// 获取或设置当前所选内容中的第一项或如果所选内容为空则返回 null
        /// <para>重写</para>
        /// </summary>
        public new object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboTree tree && e.NewValue is ITreeViewItem item)
            {
                //外部只赋 SelectedItem 时同步 SelectedValue 与文本：Popup 打开时按 SelectedValue 定位节点，
                //不同步的话单独绑定 SelectedItem 不生效
                tree.SelectedValue = tree.SelectedValuePath.IsEmpty() ? item : item.GetValue(tree.SelectedValuePath);
                tree.InitText2(item);
            }
        }
        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboTree tree)
            {
                IEnumerable list = tree.List ?? tree.ItemsSource;
                if (list != null)
                {
                    if (!tree.SelectedValuePath.IsEmpty())
                    {
                        foreach (ITreeViewItem item2 in list)
                        {
                            if (tree.InitText(item2)) break;
                        }
                    }
                    else if (tree.SelectedItem is ITreeViewItem item) tree.InitText2(item);
                }
            }
        }
        private bool InitText(ITreeViewItem item)
        {
            if (!item.IsGroup && item.GetValue(this.SelectedValuePath)?.Equals(this.SelectedValue) == true)
            {
                this.SelectedItem = item;
                this.InitText2(item);
                return true;
            }
            else if (item.Children.Count > 0)
            {
                foreach (var temp in item.Children)
                {
                    if (InitText(temp)) return true;
                }
            }
            return false;
        }
        private void InitText2(ITreeViewItem item)
        {
            this.last = this.DisplayMemberPath.IsEmpty() ? item.ToString() : item.GetValue(this.DisplayMemberPath).ToStrings();
            this.Text = this.last;
            if (this.List != null) this.ItemsSource = this.List;
        }
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
            //ItemsSource 被置空后 type 仍是旧值，会导致下面 foreach 遍历 null
            this.type = this.ItemsSource?.GenericType();
            if (typeof(ITreeViewItem).IsAssignableFrom(type))
            {
                this.List = new List<ITreeViewItem>();
                foreach (ITreeViewItem item in this.ItemsSource) this.List.Add(item);
                //模板重建(换主题/重设样式)时本方法会被再次调用，Childs 不清空会累积重复项，
                //表现为搜索下拉里出现重复节点、且越用越多
                this.Childs.Clear();
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
            if (this.Parent(out Window window))
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
                //SelectedItem 的 setter 会立刻同步 SelectedValue，必须先按旧值判断是否重复选中，
                //否则判定恒为 true；重复选中时 SelectedItem 不变、DP 回调不触发，需手动恢复显示文本
                var value = this.SelectedValuePath.IsEmpty() ? item : item.GetValue(this.SelectedValuePath);
                if (this.SelectedValue?.Equals(value) == true) this.InitText2(item);
                this.SelectedItem = item;
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
                        this.SelectedItem = item;
                        this.BeginInvoke(() => { this.IsDropDownOpen = false; });
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
            //自定义模板可能不含 PART_TreeView
            if (treeView == null) return;
            isInit = treeView.Selected(this.SelectedValue.ToInt());
            if (isInit) textBox?.Focus();
        }

        #endregion

        #region 搜索
        private IList<ITreeViewItem> List;
        private readonly IList<ITreeViewItem> Childs = new List<ITreeViewItem>();
        private string last;
        private void ComboTree_KeyUp(object sender, KeyEventArgs e)
        {
            if (textBox == null || treeView == null) return;
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
                        this.BeginInvoke(() =>
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
            var resultList = new List<ITreeViewItem>();
            //父节点按 Id 建索引：原先在循环里 resultList.Find 查找父级，匹配项多时是 O(n²)
            var index = new Dictionary<int, ITreeViewItem>();
            foreach (var item in list)
            {
                if (item.Parent == null) continue;
                if (!index.TryGetValue(item.Parent.Id, out var parent))
                {
                    parent = item.Parent.Clone();
                    //顶层节点的爷爷节点为 null，原先直接解引用会抛 NullReferenceException
                    parent.Parent = item.Parent.Parent?.Clone();
                    index[item.Parent.Id] = parent;
                    resultList.Add(parent);
                }
                parent.Add(item);
            }
            if (resultList.Count > 0) return LoadQuery(resultList);
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
                        //未选中时取第一项，最后统一确认一次；原先分支内已调用一次、下面又无条件调用一次，会重复赋值
                        if (!(treeView.SelectedItem is IId) && this.Items[0] is IId item2)
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
