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
    public abstract class ParentBasePageModel<P> : OperateItemModel where P : class, IId
    {
        #region 属性
        /// <summary>
        /// 父级列表
        /// </summary>
        public ObservableCollection<P> ParentList { get; private set; } = new ObservableCollection<P>();
        private P _parentInfo;
        public P ParentInfo
        {
            get { return _parentInfo; }
            set { if (_parentInfo != value) { _parentInfo = value; ParentChanged(); RaisePropertyChanged(); } }
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
                    if (ParentInfo == null) throw new WarningException($"请选择 {typeof(P).Description()}");
                    break;
            }
            base.Action(item);
        }
        protected override void Refresh()
        {
            base.Refresh();
            this.PageReload();
        }
        public override void PageReload()
        {
            base.PageReload();
            var last = ParentInfo;
            ParentList.Clear();
            foreach (var item in Cache.List<P>()) ParentList.Add(item);
            ParentInfo = Cache.List<P>().Find(c => c.Id == last?.Id);
            if (ParentInfo == null && ParentList.Count > 0) ParentInfo = ParentList.First();
        }

        #endregion

        public ParentBasePageModel() { }
    }
    /// <summary>
    /// 带父级列表的控件
    /// </summary>
    public abstract class ParentBasePageModel<P, T> : DataGridPageModel<T> where T : class, IBaseInfo, IParent, ICompare<T>, new()
                                                                           where P : class, IId
    {
        #region 属性
        /// <summary>
        /// 父级列表
        /// </summary>
        public ObservableCollection<P> ParentList { get; private set; } = new ObservableCollection<P>();
        private P _parentInfo;
        public P ParentInfo
        {
            get { return _parentInfo; }
            set { if (_parentInfo != value) { _parentInfo = value; ParentChanged(); RaisePropertyChanged(); } }
        }
        protected virtual void ParentChanged()
        {
            if (ParentInfo == null) base.listFilter = c => false;
            else base.listFilter = c => c.ParentId == ParentInfo.Id;
            if (DataGrid != null) base.Reload();
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
                    if (ParentInfo == null) throw new WarningException($"请选择 {typeof(P).Description()}");
                    break;
            }
            base.Action(item);
        }
        protected override void Refresh()
        {
            base.Refresh();
            this.PageReload();
        }
        public override void PageReload()
        {
            base.PageReload();
            var last = ParentInfo;
            ParentList.Clear();
            foreach (var item in Cache.List<P>()) ParentList.Add(item);
            ParentInfo = Cache.List<P>().Find(c => c.Id == last?.Id);
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
    public abstract class ChildBasePageModel<T, C> : DataGridPageModel<T> where T : class, ILoad<T>, IBaseInfo, ICompare<T>, new()
                                                                          where C : class, IParent
    {
        #region 属性
        /// <summary>
        /// 子级列表
        /// </summary>
        public ObservableCollection<C> DetailList { get; private set; } = new ObservableCollection<C>();
        public PagedCollectionView DetailPagedList { get; private set; }

        #endregion
        #region 重载
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