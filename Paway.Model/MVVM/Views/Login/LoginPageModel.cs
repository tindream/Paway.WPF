using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    public class LoginPageModel : ViewModelBase
    {
        #region 属性
        protected DependencyObject Root;
        public ObservableCollection<object> ObList { get; private set; } = new ObservableCollection<object>();
        /// <summary>
        /// 登陆锁
        /// </summary>
        private volatile bool iLogining;
        /// <summary>
        /// 过滤锁
        /// </summary>
        private volatile bool iFiltering;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    try
                    {
                        if (!iFiltering)
                        {
                            iFiltering = true;
                            FilterUser(value);
                        }
                    }
                    finally
                    {
                        iFiltering = false;
                    }
                    _userName = value;
                    RaisePropertyChanged();
                }
            }
        }
        protected virtual void FilterUser(string userName) { }

        private string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(); }
        }

        private bool _iUserList;
        public bool IUserList
        {
            get { return _iUserList; }
            set { _iUserList = value; RaisePropertyChanged(); }
        }

        private ImageSource _logoImage;
        public ImageSource LogoImage
        {
            get { return _logoImage; }
            set { _logoImage = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 命令
        public ICommand LoginCommand => new RelayCommand<string>(item =>
        {
            try
            {
                if (UserName.IsEmpty())
                {
                    Method.Hit(Root, "请输入用户名");
                    if (IUserList)
                    {
                        if (Method.Find(Root, out ComboBoxEXT cbxUserName, "cbxUserName")) cbxUserName.Focus();
                    }
                    else
                    {
                        if (Method.Find(Root, out TextBoxEXT tbUserName, "tbUserName")) tbUserName.Focus();
                    }
                    return;
                }
                if (Password.IsEmpty())
                {
                    Method.Hit(Root, "请输入密码");
                    if (Method.Find(Root, out PasswordBox tbPassword, "tbPassword")) tbPassword.Focus();
                    return;
                }
                if (!iLogining)
                {
                    iLogining = true;
                    Login();
                }
            }
            finally
            {
                iLogining = false;
            }
        });
        public virtual void Login() { }

        #endregion

        public LoginPageModel()
        {
            Messenger.Default.Register<LoginLoadMessage>(this, msg => this.Root = msg.Obj);
        }
    }
}