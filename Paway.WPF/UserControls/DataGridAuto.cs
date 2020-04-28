using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Paway.Helper;

namespace Paway.WPF
{
    public partial class DataGridAuto : DataGrid
    {
        #region 基本样式
        public DataGridAuto()
        {
            //AutoGenerateColumns = true;
            this.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
            this.ColumnHeaderHeight = 42;
            this.Initialized += DataGridAuto_Initialized;
        }
        private void DataGridAuto_Initialized(object sender, EventArgs e)
        {
            columnsReady.AddRange(this.Columns);
        }

        #endregion

        #region 绑定数据
        /// <summary>
        /// 当前绑定的数据类型
        /// </summary>
        private Type type;
        /// <summary>
        /// 外部自定义列
        /// </summary>
        private readonly List<DataGridColumn> columnsReady = new List<DataGridColumn>();
        /// <summary>
        /// 获取或设置用于生成 System.Windows.Controls.ItemsControl 的内容的集合。
        /// <para>重载数据绑定</para>
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new IEnumerable ItemsSource
        {
            get { return base.ItemsSource; }
            set
            {
                this.type = value.GetType().GenericType();
                LoadColumns();
                base.ItemsSource = value;
            }
        }
        private void LoadColumns()
        {
            var columns = new List<DataGridColumn>();
            var properties = this.type.PropertiesCache();
            foreach (var property in properties)
            {
                var column = columnsReady.Find(c => (c.ClipboardContentBinding is Binding binding && binding.Path.Path == property.Name) || c.Header.ToStrs() == property.TextName());
                if (column != null)
                {
                    columns.Add(column);
                }
                else
                {
                    column = new DataGridTextColumn();
                    (column as DataGridTextColumn).Binding = new Binding(property.Name);
                    columns.Add(column);
                }
                column.Header = property.TextName();
                if (column is DataGridTextColumn text && text.ElementStyle.Setters.Count == 1 && (text.ElementStyle.Setters[0] as Setter).Property.Name == "Margin")
                {
                    text.ElementStyle = (Style)FindResource("TextLeftSytle");
                }
                column.Visibility = property.IShow() ? Visibility.Visible : Visibility.Collapsed;
            }
            this.Columns.Clear();
            foreach (var column in columns) this.Columns.Add(column);
        }

        #endregion

        #region 扩展公共方法
        public DataGridColumn GetColumn(string name)
        {
            foreach (var item in Columns)
            {
                if (item.ClipboardContentBinding is Binding binding && binding.Path.Path == name)
                {
                    return item;
                }
            }
            return new DataGridTextColumn();
        }

        #endregion
    }
}
