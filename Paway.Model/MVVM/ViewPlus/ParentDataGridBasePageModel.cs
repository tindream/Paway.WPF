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
    /// 带父级列表的DataGrid控件
    /// </summary>
    public abstract class ParentDataGridBasePageModel<Parent, T> : DataGridPageModel<T> where T : class, IOperateOnceInfo, IParent, ICompare<T>, new()
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
        public override bool Action(string item)
        {
            switch (item)
            {
                case "刷新":
                    break;
                default:
                    if (ParentInfo == null) throw new WarningException($"{PConfig.LanguageBase.PleaseSelect} {typeof(Parent).Description()}");
                    break;
            }
            return base.Action(item);
        }
        /// <summary>
        /// 重载-自定义刷新操作
        /// </summary>
        protected override void Refresh(Action action = null)
        {
            //base 是异步刷新（ProgressAsync + Task.Run），原先紧跟的 PageReload 会与后台加载并发执行；
            //改为作为回调执行，保证数据加载完成后再重建父级列表
            base.Refresh(() =>
            {
                action?.Invoke();
                this.Invoke(() => this.PageReload());
            });
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
        protected override bool AddWindow()
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
        /// 带父级列表的DataGrid控件
        /// </summary>
        public ParentDataGridBasePageModel() { }
    }
}