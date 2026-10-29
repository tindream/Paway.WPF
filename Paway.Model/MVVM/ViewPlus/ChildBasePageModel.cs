using Paway.Helper;
using Paway.WPF;
using Paway.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Paway.Model
{
    /// <summary>
    /// 带子级列表的控件
    /// </summary>
    public abstract class ChildBasePageModel<T, Child> : DataGridPageModel<T> where T : class, ILoad<Child>, IOperateOnceInfo, ICompare<T>, new()
                                                                              where Child : class, IParent, new()
    {
        #region 属性
        /// <summary>
        /// 子级列表
        /// </summary>
        public ObservableCollection<Child> DetailList { get; private set; } = new ObservableCollection<Child>();
        /// <summary>
        /// 子级分页列表
        /// </summary>
        public PagedCollectionView DetailPagedList { get; private set; }
        private Child _childItem;
        /// <summary>
        /// 子级实体
        /// </summary>
        public Child ChildItem
        {
            get { return _childItem; }
            set { if (_childItem != value) { _childItem = value; ChildChanged(); OnPropertyChanged(); } }
        }
        /// <summary>
        /// 获取副DataGrid选中项
        /// <para>选择单元格时，ChildItem为空</para>
        /// </summary>
        protected Child ChildInfo(DataGrid dataGrid)
        {
            if (ChildItem != null) return ChildItem;
            if (dataGrid.SelectedItem is Child t) return t;
            if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow && dataGrid.SelectedCells.Count > 0)
            {
                if (dataGrid.SelectedCells.First().Item is Child t2) return t2;
            }
            return default;
        }
        /// <summary>
        /// 子级实体切换
        /// </summary>
        protected virtual void ChildChanged() { }

        #endregion
        #region 重载
        /// <summary>
        /// 选中项后处理
        /// </summary>
        protected override void SelectedChanged()
        {
            base.SelectedChanged();
            DetailList.Clear();
            if (SelectedItem != null)
            {
                SelectedItem.Load();
                foreach (var item in SelectedItem.DetailList) DetailList.Add(item);
            }
        }
        /// <summary>
        /// 重载-自定义删除实体
        /// </summary>
        protected override void Deleted(T info)
        {
            info.Load();
            base.Deleted(info);
            base.server.Delete(info.DetailList); Cache.Delete(info.DetailList); if (!this.iCache) Cache.UpdateList(OperType.Delete, this.List, info.DetailList);
        }

        #endregion

        /// <summary>
        /// 带子级列表的控件
        /// </summary>
        public ChildBasePageModel()
        {
            this.DetailPagedList = new PagedCollectionView(DetailList) { PageSize = 20 };
        }
    }
}