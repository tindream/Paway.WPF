using Paway.Helper;
using Paway.WPF;
using Paway.Comm;
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
    /// 带父级列表的控件
    /// </summary>
    public abstract class ParentBasePageModel<Parent> : OperateItemModel where Parent : class, IId
    {
        #region 属性
        /// <summary>
        /// 父级列表
        /// </summary>
        public ObservableCollection<Parent> ParentList { get; private set; } = new ObservableCollection<Parent>();
        private Parent _parentInfo;
        /// <summary>
        /// 父级实体
        /// </summary>
        public Parent ParentInfo
        {
            get { return _parentInfo; }
            set { if (_parentInfo != value) { _parentInfo = value; ParentChanged(); OnPropertyChanged(); } }
        }
        /// <summary>
        /// 父级实体切换
        /// </summary>
        protected virtual void ParentChanged() { }

        #endregion
        #region 重载
        /// <summary>
        /// 通用动作命令
        /// </summary>
        protected override void Action(string item)
        {
            switch (item)
            {
                case "刷新":
                    break;
                default:
                    if (ParentInfo == null) throw new WarningException($"请选择 {typeof(Parent).Description()}");
                    break;
            }
            base.Action(item);
        }
        /// <summary>
        /// 刷榜
        /// </summary>
        protected override void Refresh(Action action = null)
        {
            base.Refresh(() => PMethod.Invoke(() => this.PageReload()));
        }
        /// <summary>
        /// 在Loaded第一次触发或重加载时调用
        /// </summary>
        public override void PageReload()
        {
            base.PageReload();
            var last = ParentInfo;
            ParentList.Clear();
            foreach (var item in Cache.FindAll<Parent>()) ParentList.Add(item);
            ParentInfo = Cache.Find<Parent>(last?.Id ?? 0);
            if (ParentInfo == null && ParentList.Count > 0) ParentInfo = ParentList.First();
        }

        #endregion

        /// <summary>
        /// 带父级列表的控件
        /// </summary>
        public ParentBasePageModel() { }
    }
    /// <summary>
    /// 带父级列表的控件
    /// </summary>
    public abstract class ParentBasePageModel<Parent, T> : DataGridPageModel<T> where T : class, IBaseInfo, IParent, ICompare<T>, new()
                                                                           where Parent : class, IId
    {
        #region 属性
        /// <summary>
        /// 父级列表
        /// </summary>
        public ObservableCollection<Parent> ParentList { get; private set; } = new ObservableCollection<Parent>();
        private Parent _parentInfo;
        /// <summary>
        /// 父级实体
        /// </summary>
        public Parent ParentInfo
        {
            get { return _parentInfo; }
            set { if (_parentInfo != value) { _parentInfo = value; ParentChanged(); OnPropertyChanged(); } }
        }
        /// <summary>
        /// 父级实体切换
        /// </summary>
        protected virtual void ParentChanged()
        {
            if (ParentInfo == null) base.listFilter = c => false;
            else base.listFilter = c => c.ParentId == ParentInfo.Id;
            if (DataGrid != null) this.Reload();
        }

        #endregion
        #region 重载
        /// <summary>
        /// 通用动作命令
        /// </summary>
        protected override void Action(string item)
        {
            switch (item)
            {
                case "刷新":
                    break;
                default:
                    if (ParentInfo == null) throw new WarningException($"请选择 {typeof(Parent).Description()}");
                    break;
            }
            base.Action(item);
        }
        /// <summary>
        /// 重载-自定义刷新操作
        /// </summary>
        protected override void Refresh(Action action = null)
        {
            base.Refresh(action);
            this.PageReload();
        }
        /// <summary>
        /// 在Loaded第一次触发或重加载时调用
        /// </summary>
        public override void PageReload()
        {
            base.PageReload();
            var last = ParentInfo;
            ParentList.Clear();
            foreach (var item in Cache.FindAll<Parent>()) ParentList.Add(item);
            ParentInfo = Cache.Find<Parent>(last?.Id ?? 0);
            if (ParentInfo == null && ParentList.Count > 0) ParentInfo = ParentList.First();
        }
        /// <summary>
        /// 设置添加窗体
        /// </summary>
        protected override Window AddWindow()
        {
            if (ViewModel().Info.Id == 0) ViewModel().Info.ParentId = ParentInfo.Id;
            return base.AddWindow();
        }
        /// <summary>
        /// 重载-自定义导入列表前检查
        /// </summary>
        protected override void ImportChecked(List<T> list)
        {
            base.ImportChecked(list);
            list.ForEach(item => { item.ParentId = ParentInfo.Id; });
        }

        #endregion

        /// <summary>
        /// 带父级列表的控件
        /// </summary>
        public ParentBasePageModel() { }
    }
    /// <summary>
    /// 带子级列表的控件
    /// </summary>
    public abstract class ChildBasePageModel<T, Child> : DataGridPageModel<T> where T : class, ILoad<Child>, IBaseInfo, ICompare<T>, new()
                                                                              where Child : class, IParent
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
            if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow && dataGrid.SelectedCells.Count > 0)
            {
                if (dataGrid.SelectedCells.First().Item is Child t) return t;
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
            base.server.Delete(info.DetailList); Cache.Delete(info.DetailList);
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