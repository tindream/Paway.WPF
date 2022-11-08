using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    public abstract partial class OperateItemModel : ViewModelBase
    {
        #region 权限控制
        public abstract string Menu { get; }
        private MenuAuthType _auth;
        public MenuAuthType Auth
        {
            get { return _auth; }
            set { _auth = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 菜单
        internal virtual void ActionInternal(string item)
        {
            try
            {
                Action(item);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new StatuMessage(ex));
            }
        }
        protected virtual void Action(string item)
        {
            switch (item)
            {
                case "刷新":
                    Refresh();
                    break;
                case "保存":
                    Save();
                    break;
            }
        }
        protected virtual void Refresh() { }
        protected virtual void Save() { }
        public ICommand ItemClickCommand => new RelayCommand<string>(item => ActionInternal(item));

        #endregion

        #region 搜索
        protected virtual void Search() { }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged(); Search(); }
        }
        protected void ClearSearch() { this._searchText = null; RaisePropertyChanged(nameof(SearchText)); }

        #endregion

        /// <summary>
        /// 默认权限
        /// <para>MenuAuthType.Refresh | MenuAuthType.Save</para>
        /// <para>刷新、保存</para>
        /// </summary>
        protected virtual void AuthNormal()
        {
            this.Auth = MenuAuthType.Refresh | MenuAuthType.Save;
        }

        public OperateItemModel()
        {
            AuthNormal();
        }
    }
}