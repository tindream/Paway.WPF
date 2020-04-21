using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            this.Background = new SolidColorBrush(Color.FromArgb(255, 250, 250, 250));
            this.ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star);
            this.CanUserSortColumns = true;
            this.CanUserResizeColumns = true;
            this.FontSize = 15;
            this.RowHeight = 36;
            this.ColumnHeaderHeight = 35;
        }

        #endregion

        #region 绑定数据
        /// <summary>
        /// 当前绑定的数据类型
        /// </summary>
        private Type type;
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
            this.Columns.Clear();
            var properties = this.type.PropertiesCache();
            foreach (var property in properties)
            {
                var column = new DataGridTextColumn();
                column.Binding = new Binding(property.Name);
                column.Header = property.TextName();
                column.ElementStyle = (Style)FindResource("TextLeftSytle");
                column.FontSize = this.FontSize;
                column.Visibility = property.IShow() ? Visibility.Visible : Visibility.Collapsed;
                this.Columns.Add(column);
            }
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
