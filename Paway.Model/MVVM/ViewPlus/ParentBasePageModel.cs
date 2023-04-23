using Paway.Helper;
using Paway.WPF;
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
        public Parent ParentInfo
        {
            get { return _parentInfo; }
            set { if (_parentInfo != value) { _parentInfo = value; ParentChanged(); OnPropertyChanged(); } }
        }
        protected virtual void ParentChanged() { }

        #endregion
        #region 重载
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
        protected override void Refresh(Action action = null)
        {
            base.Refresh(() => Method.Invoke(() => this.PageReload()));
        }
        public override void PageReload()
        {
            base.PageReload();
            var last = ParentInfo;
            ParentList.Clear();
            foreach (var item in Cache.List<Parent>()) ParentList.Add(item);
            ParentInfo = Cache.List<Parent>().Find(c => c.Id == last?.Id);
            if (ParentInfo == null && ParentList.Count > 0) ParentInfo = ParentList.First();
        }

        #endregion

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
        public Parent ParentInfo
        {
            get { return _parentInfo; }
            set { if (_parentInfo != value) { _parentInfo = value; ParentChanged(); OnPropertyChanged(); } }
        }
        protected virtual void ParentChanged()
        {
            if (ParentInfo == null) base.listFilter = c => false;
            else base.listFilter = c => c.ParentId == ParentInfo.Id;
            if (DataGrid != null) this.Reload();
        }

        #endregion
        #region 重载
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
        protected override void Refresh(Action action = null)
        {
            base.Refresh(() => Method.Invoke(() => this.PageReload()));
        }
        public override void PageReload()
        {
            base.PageReload();
            var last = ParentInfo;
            ParentList.Clear();
            foreach (var item in Cache.List<Parent>()) ParentList.Add(item);
            ParentInfo = Cache.List<Parent>().Find(c => c.Id == last?.Id);
            if (ParentInfo == null && ParentList.Count > 0) ParentInfo = ParentList.First();
        }
        protected override Window AddWindow()
        {
            if (ViewModel().Info.Id == 0) ViewModel().Info.ParentId = ParentInfo.Id;
            return base.AddWindow();
        }
        protected override void ImportChecked(List<T> list)
        {
            base.ImportChecked(list);
            list.ForEach(item => { item.ParentId = ParentInfo.Id; });
        }

        #endregion

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
        public PagedCollectionView DetailPagedList { get; private set; }

        #endregion
        #region 重载
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
        protected override void Deleted(T info)
        {
            info.Load();
            base.Deleted(info);
            base.server.Delete(info.DetailList);
            Method.Delete(info.DetailList);
        }

        #endregion

        public ChildBasePageModel()
        {
            this.DetailPagedList = new PagedCollectionView(DetailList) { PageSize = 20 };
        }
    }
}