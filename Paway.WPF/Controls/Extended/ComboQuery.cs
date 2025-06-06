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
    /// ComboBox扩展搜索(DataGrid)
    /// </summary>
    public class ComboQuery : ComboBoxEXT
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderProperty =
            DependencyProperty.RegisterAttached(nameof(ColumnHeader), typeof(bool), typeof(ComboQuery), new PropertyMetadata(false));
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedValueProperty =
            DependencyProperty.RegisterAttached(nameof(SelectedValue), typeof(object), typeof(ComboQuery), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedValueChanged));
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached(nameof(SelectedItem), typeof(object), typeof(ComboQuery));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ColumnWidth), typeof(DataGridLength), typeof(ComboQuery), new PropertyMetadata(new DataGridLength(1, DataGridLengthUnitType.Star)));

        /// <summary>
        /// 下拉列表是否显示列
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("下拉列表是否显示列标题")]
        public bool ColumnHeader
        {
            get { return (bool)GetValue(ColumnHeaderProperty); }
            set { SetValue(ColumnHeaderProperty, value); }
        }
        /// <summary>
        /// 下拉列表列宽度
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("下拉列表列宽度")]
        public DataGridLength ColumnWidth
        {
            get { return (DataGridLength)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }
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
        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboQuery view)
            {
                IEnumerable list = view.List ?? view.ItemsSource;
                if (list != null)
                {
                    var id = view.SelectedValue.ToInt();
                    if (list.Find(nameof(IId.Id), id) is IId item)
                    {
                        view.InitText(item);
                    }
                }
            }
        }
        private void InitText(IId item)
        {
            this.SelectedItem = item;
            this.last = this.DisplayMemberPath.IsEmpty() ? item.ToString() : item.GetValue(this.DisplayMemberPath).ToStrings();
            this.Text = this.last;
            if (this.List != null) this.ItemsSource = this.List;
        }

        #endregion

        #region 事件
        /// <summary>
        /// DataGrid数据绑定刷新事件
        /// </summary>
        public event Action<DataGridEXT> RefreshEvent;
        /// <summary>
        /// 搜索过滤事件
        /// </summary>
        public event EventHandler<CustomFilterEventArgs> FilterEvent;

        #endregion

        /// <summary>
        /// </summary>
        public ComboQuery()
        {
            DefaultStyleKey = typeof(ComboQuery);
        }

        #region 关联选择
        private DataGridEXT gridView;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            IsEditable = false;
            if (Template.FindName("PART_Popup", this) is Popup popup)
            {
                popup.Opened -= Popup_Opened;
                popup.Opened += Popup_Opened;
            }
            if (Template.FindName("PART_DataGrid", this) is DataGridEXT gridView)
            {
                this.gridView = gridView;
                if (!ColumnHeader) gridView.ColumnHeaderHeight = 0;
                gridView.RefreshEvent -= GridView_RefreshEvent;
                gridView.RefreshEvent += GridView_RefreshEvent;
                gridView.RowDoubleEvent -= GridView_RowDoubleEvent;
                gridView.RowDoubleEvent += GridView_RowDoubleEvent;
            }
            if (PMethod.Parent(this, out Window window))
            {
                window.LocationChanged -= Window_LocationChanged;
                window.LocationChanged += Window_LocationChanged;
            }
            this.KeyUp -= ComboQuery_KeyUp;
            this.KeyUp += ComboQuery_KeyUp;
        }
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            this.IsDropDownOpen = false;
        }
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }
        private void GridView_RowDoubleEvent(object arg1, SelectItemEventArgs arg2)
        {
            {
                var value = this.SelectedValuePath.IsEmpty() ? arg2.Item : arg2.Item.GetValue(this.SelectedValuePath);
                if (this.SelectedValue?.Equals(value) == true) this.Text = null;
                this.SelectedValue = value;
            }
            IsDropDownOpen = false;
            textBox.Focus();
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            var result = gridView.Select(this.SelectedValuePath, this.SelectedValue);
            if (result) textBox.Focus();
        }
        private void GridView_RefreshEvent(DataGridEXT obj)
        {
            RefreshEvent?.Invoke(obj);
        }

        #endregion

        #region 搜索
        private List<dynamic> List;
        private string last;
        private void ComboQuery_KeyUp(object sender, KeyEventArgs e)
        {
            var text = textBox.Text;
            if (last == text) return;
            last = text;
            var index = textBox.SelectionStart;
            if (text.IsEmpty())
            {
                this.ItemsSource = this.List;
            }
            else
            {
                if (this.ItemsSource != null && this.List == null)
                {
                    this.List = new List<dynamic>();
                    foreach (var item in this.ItemsSource) this.List.Add(item);
                }
                if (FilterEvent != null)
                {
                    var args = new CustomFilterEventArgs(text, e.RoutedEvent, gridView);
                    FilterEvent.Invoke(this, args);
                    if (args.List != null)
                    {
                        this.ItemsSource = args.List;
                    }
                }
                else if (this.List != null)
                {
                    var type = this.ItemsSource.GenericType();
                    var p = type.Predicate<dynamic>(gridView.Columns, text);
                    this.ItemsSource = this.List.AsParallel().Where(p).ToList();
                }
                if (textBox.Text != text)
                {//第一次搜索绑定出现输入框被清空
                    textBox.Text = text;
                    textBox.SelectionStart = index;
                }
            }
            IsDropDownOpen = true;
        }
        /// <summary>
        /// 按键快捷响应
        /// </summary>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
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
                        if (gridView.SelectedIndex == 0)
                        {
                            textBox.Focus();
                            return;
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
                        var index = gridView.SelectedIndex;
                        if (index < 0) index = 0;
                        if (index >= this.Items.Count) index = this.Items.Count - 1;
                        GridView_RowDoubleEvent(gridView, new SelectItemEventArgs(this.Items[index], e.RoutedEvent, gridView));
                    }
                    break;
            }
            base.OnPreviewKeyDown(e);
        }
        private bool Selected()
        {
            if (gridView.SelectedItem is IId item)
            {
                return gridView.Select(item.Id, true);
            }
            if (this.Items.Count > 0 && this.Items[0] is IId item2)
            {
                return gridView.Select(item2.Id, true);
            }
            return false;
        }

        #endregion
    }
}
