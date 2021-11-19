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
    public class ComboView : ComboBoxEXT
    {
        #region 扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColumnHeaderProperty =
            DependencyProperty.RegisterAttached(nameof(ColumnHeader), typeof(bool), typeof(ComboView), new PropertyMetadata(false));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ColumnWidth), typeof(DataGridLength), typeof(ComboView));

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
        /// 下拉列表列宽度
        /// </summary>
        [Category("扩展")]
        [Description("下拉列表列宽度")]
        public DataGridLength ColumnWidth
        {
            get { return (DataGridLength)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        #endregion

        #region 事件
        /// <summary>
        /// DataGrid数据绑定刷新事件
        /// </summary>
        public event Action<DataGridEXT> RefreshEvent;

        #endregion

        /// <summary>
        /// </summary>
        public ComboView()
        {
            DefaultStyleKey = typeof(ComboView);
        }

        #region 关联选择
        private bool selecting;
        private DataGridEXT gridView;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
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
                gridView.CurrentCellChanged -= GridView_CurrentCellChanged;
                gridView.CurrentCellChanged += GridView_CurrentCellChanged;
            }
        }
        private void GridView_CurrentCellChanged(object sender, EventArgs e)
        {
            if (selecting) return;
            var gridView = sender as DataGridEXT;
            if (gridView.CurrentCell != null && gridView.CurrentCell.Item is IId item)
            {
                this.SelectedValue = item.GetValue(this.SelectedValuePath);
                this.IsDropDownOpen = false;
            }
        }
        private void Popup_Opened(object sender, EventArgs e)
        {
            try
            {
                selecting = true;
                gridView.Select(this.SelectedValue.ToInt());
            }
            finally
            {
                selecting = false;
            }
        }
        private void GridView_RefreshEvent(DataGridEXT obj)
        {
            RefreshEvent?.Invoke(obj);
        }

        #endregion
    }
}
