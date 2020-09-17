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

        #endregion

        /// <summary>
        /// </summary>
        public ComboBoxView()
        {
            DefaultStyleKey = typeof(ComboBoxView);
        }

        #region 关联选择
        private bool isInit;
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template.FindName("PART_DataGrid", this) is DataGridEXT gridView)
            {
                if (!ColumnHeader) gridView.ColumnHeaderHeight = 0;
                gridView.ColumnVisibleEvent += GridView_ColumnVisibleEvent;
                gridView.MouseDoubleClick += GridView_MouseDoubleClick;
                gridView.CurrentCellChanged += GridView_CurrentCellChanged;
                gridView.Loaded += GridView_Loaded;
            }
        }
        private void GridView_ColumnVisibleEvent(DataGridEXT obj)
        {
            ColumnVisibleEvent?.Invoke(obj);
        }
        private void GridView_Loaded(object sender, RoutedEventArgs e)
        {
            var gridView = sender as DataGridEXT;
            var id = this.SelectedValue.ToInt();
            for (int i = 0; i < gridView.Items.Count; i++)
            {
                if (gridView.Items[i] is IId item && item.Id == id)
                {
                    gridView.ScrollIntoView(item);
                    if (gridView.ItemContainerGenerator.ContainerFromIndex(i) is DataGridRow row)
                    {
                        row.IsSelected = true;
                        isInit = true;
                        gridView.Loaded -= GridView_Loaded;
                    }
                }
            }
        }
        private void GridView_CurrentCellChanged(object sender, EventArgs e)
        {
            var gridView = sender as DataGridEXT;
            if (gridView.CurrentCell != null && gridView.CurrentCell.Item is IId item)
            {
                this.SelectedValue = item.GetValue(this.SelectedValuePath);
                if (isInit) this.Dispatcher.BeginInvoke(new Action(() => { this.IsDropDownOpen = false; }));
            }
        }
        private void GridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //this.IsDropDownOpen = false;
        }

        #endregion
    }
}
