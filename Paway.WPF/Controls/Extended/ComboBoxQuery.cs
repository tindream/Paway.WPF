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
    public class ComboBoxQuery : ComboBoxEXT
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderProperty =
            DependencyProperty.RegisterAttached(nameof(ColumnHeader), typeof(bool), typeof(ComboBoxQuery), new PropertyMetadata(false));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.RegisterAttached(nameof(IsOpen), typeof(bool), typeof(ComboBoxQuery), new PropertyMetadata(false));
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedValueProperty =
            DependencyProperty.RegisterAttached(nameof(ComboBoxTree.SelectedValue), typeof(object), typeof(ComboBoxQuery), new PropertyMetadata(OnSelectedValueChanged));
        /// <summary>
        /// </summary>
        public static readonly new DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached(nameof(ComboBoxTree.SelectedItem), typeof(object), typeof(ComboBoxQuery));

        /// <summary>
        /// 下拉列表是否显示列
        /// </summary>
        [Category("扩展")]
        [Description("下拉列表是否显示列标题")]
        public bool ColumnHeader
        {
            get { return (bool)GetValue(ColumnHeaderProperty); }
            set { SetValue(ColumnHeaderProperty, value); }
        }
        /// <summary>
        /// 下拉列表是否显示列
        /// </summary>
        [Category("扩展")]
        [Description("下拉列表是否显示列标题")]
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
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
            if (d is ComboBoxQuery view)
            {
                var list = view.List;
                if (list == null) list = view.ItemsSource;
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
            this.last = item.GetValue(this.DisplayMemberPath).ToStrs();
            this.Text = this.last;
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
        public ComboBoxQuery()
        {
            DefaultStyleKey = typeof(ComboBoxQuery);
        }

        #region 关联选择
        private DataGridEXT gridView;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.List = this.ItemsSource;
            if (Template.FindName("PART_Popup", this) is Popup popup)
            {
                popup.Opened += Popup_Opened;
            }
            if (Template.FindName("PART_DataGrid", this) is DataGridEXT gridView)
            {
                this.gridView = gridView;
                if (!ColumnHeader) gridView.ColumnHeaderHeight = 0;
                gridView.RefreshEvent += GridView_RefreshEvent;
                gridView.RowDoubleEvent += GridView_RowDoubleEvent;
            }
            if (Template.FindName("PART_EditableTextBox", this) is TextBoxEXT textBox)
            {
                this.textBox = textBox;
                textBox.PreviewMouseLeftButtonDown += TextBox_PreviewMouseLeftButtonDown;
            }
            if (Method.Parent(this, out Window window))
            {
                window.LocationChanged += delegate { this.IsDropDownOpen = false; };
            }
            this.KeyUp += ComboBoxQuery_KeyUp;
        }
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsDropDownOpen = !IsDropDownOpen;
        }
        private void GridView_RowDoubleEvent(object arg1, RowDoubleEventArgs arg2)
        {
            if (arg2.Item is IId item)
            {
                if (this.SelectedValue.Equals(item.Id))
                    this.Text = null;
                this.SelectedValue = item.Id;
            }
            IsDropDownOpen = false;
            textBox.Focus();
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            var result = gridView.Selected(this.SelectedValue.ToInt());
            if (result) textBox.Focus();
        }
        private void GridView_RefreshEvent(DataGridEXT obj)
        {
            RefreshEvent?.Invoke(obj);
        }

        #endregion

        #region 搜索
        private IEnumerable List;
        private TextBoxEXT textBox;
        private string last;
        private void ComboBoxQuery_KeyUp(object sender, KeyEventArgs e)
        {
            var text = textBox.Text;
            if (last == text) return;
            last = text;
            Trace.WriteLine(text);
            if (FilterEvent != null)
            {
                if (text.IsEmpty())
                {
                    this.ItemsSource = this.List;
                }
                else
                {
                    var args = new CustomFilterEventArgs(text, e.RoutedEvent, this);
                    FilterEvent.Invoke(this, args);
                    if (args.List != null)
                    {
                        if (args.List.Count == 0)
                        {
                            this.ItemsSource = this.List;
                        }
                        else
                        {
                            this.ItemsSource = args.List;
                        }
                    }
                }
                IsDropDownOpen = true;
            }
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
                        GridView_RowDoubleEvent(gridView, new RowDoubleEventArgs(this.Items[index], e.RoutedEvent, gridView));
                    }
                    break;
            }
            base.OnPreviewKeyDown(e);
        }
        private bool Selected()
        {
            if (this.Items.Count == 0) return false;
            var index = gridView.SelectedIndex;
            if (index < 0) index = 0;
            if (index >= this.Items.Count) index = this.Items.Count - 1;
            if (this.Items[index] is IId item)
            {
                gridView.Selected(item.Id);
                return true;
            }
            return false;
        }

        #endregion
    }
}
