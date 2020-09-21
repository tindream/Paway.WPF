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
    /// ComboBox扩展视图(DataGrid)
    /// </summary>
    public class ComboBoxView : ComboBoxEXT
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderProperty =
            DependencyProperty.RegisterAttached(nameof(ColumnHeader), typeof(bool), typeof(ComboBoxEXT), new PropertyMetadata(false));

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

        #endregion

        #region 事件
        /// <summary>
        /// DataGrid列绑定显示事件
        /// </summary>
        public event Action<DataGridEXT> ColumnVisibleEvent;
        /// <summary>
        /// 搜索过滤事件
        /// </summary>
        public event Action<object, ComboBoxViewFilterEventArgs> FilterEvent;

        #endregion

        /// <summary>
        /// </summary>
        public ComboBoxView()
        {
            DefaultStyleKey = typeof(ComboBoxView);
        }

        #region 关联选择
        private DataGridEXT gridView;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template.FindName("PART_Popup", this) is Popup popup)
            {
                popup.Opened += Popup_Opened;
            }
            if (Template.FindName("PART_DataGrid", this) is DataGridEXT gridView)
            {
                this.gridView = gridView;
                if (!ColumnHeader) gridView.ColumnHeaderHeight = 0;
                gridView.ColumnVisibleEvent += GridView_ColumnVisibleEvent;
                gridView.RowDoubleEvent += GridView_RowDoubleEvent;
            }
            if (Template.FindName("PART_EditableTextBox", this) is TextBoxEXT textBox)
            {
                InitQuery(textBox);
            }
        }
        private void GridView_RowDoubleEvent(DataGridEXT arg1, object arg2)
        {
            if (arg2 is IId item)
            {
                this.SelectedValue = item.GetValue(this.SelectedValuePath);
            }
            this.IsDropDownOpen = false;
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            gridView.Selected(this.SelectedValue.ToInt());
        }
        private void GridView_ColumnVisibleEvent(DataGridEXT obj)
        {
            ColumnVisibleEvent?.Invoke(obj);
        }

        #endregion

        #region 搜索
        private IEnumerable List;
        internal TextBoxEXT textBox;
        private void InitQuery(TextBoxEXT textBox)
        {
            this.List = this.ItemsSource;
            this.textBox = textBox;

            textBox.TextChanged += TextBox_TextChanged;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBoxEXT;
            Trace.WriteLine(textBox.Text);
            if (FilterEvent != null)
            {
                this.IsDropDownOpen = true;
                var text = textBox.Text;
                if (text.IsEmpty())
                {
                    this.ItemsSource = this.List;
                }
                else
                {
                    var args = new ComboBoxViewFilterEventArgs() { Filter = text };
                    FilterEvent.Invoke(this, args);
                    if (args.List != null)
                    {
                        this.ItemsSource = args.List;
                        if (args.List.Count > 0)
                        {
                            //gridView.Selected(args.List[0].GetValue(this.SelectedValuePath).ToInt());
                        }
                    }
                }
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
                        this.IsDropDownOpen = true;
                    }
                    break;
                case Key.Up:
                    if (this.IsDropDownOpen)
                    {
                        if (gridView.SelectedIndex == 0)
                        {
                            this.IsDropDownOpen = false;
                            e.Handled = true;
                            return;
                        }
                    }
                    break;
                case Key.Down:
                    if (textBox.IsFocused && !this.IsDropDownOpen)
                    {
                        this.IsDropDownOpen = true;
                        e.Handled = true;
                        return;
                    }
                    break;
            }
            base.OnPreviewKeyDown(e);
        }

        #endregion
    }
    /// <summary>
    /// 搜索过滤路由事件参数
    /// </summary>
    public class ComboBoxViewFilterEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// 过滤输入
        /// </summary>
        public string Filter { get; set; }
        /// <summary>
        /// 结果列表
        /// </summary>
        public IList List { get; set; }
    }
}
