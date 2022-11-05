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
        protected virtual void Action(string item) { }
        public ICommand ItemClickCommand => new RelayCommand<string>(item => Action(item));

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
    }
}