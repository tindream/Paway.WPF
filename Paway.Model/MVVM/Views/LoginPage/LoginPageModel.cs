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
    /// <summary>
    /// 登陆基类，必须实现登陆方法
    /// </summary>
    public abstract class LoginPageModel : ViewModelBasePlus
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
                    if (iFiltering) return;
                    try
                    {
                        iFiltering = true;
                        FilterUser(value);
                    }
                    finally
                    {
                        iFiltering = false;
                    }
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }
        protected virtual void FilterUser(string userName) { }

        private string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        private bool _iUserList;
        public bool IUserList
        {
            get { return _iUserList; }
            set { _iUserList = value; OnPropertyChanged(); }
        }

        private bool _iAuto;
        public bool IAuto
        {
            get { return _iAuto; }
            set { _iAuto = value; OnPropertyChanged(); }
        }

        private string welcome = "欢迎使用";
        public string Welcome
        {
            get { return welcome; }
            set { welcome = value; OnPropertyChanged(); }
        }
        private ImageSource _logoImage;
        public ImageSource LogoImage
        {
            get { return _logoImage; }
            set { _logoImage = value; OnPropertyChanged(); }
        }

        private bool _iSetting;
        public bool ISetting
        {
            get { return _iSetting; }
            set { _iSetting = value; OnPropertyChanged(); }
        }

        private bool _iClose;
        public bool IClose
        {
            get { return _iClose; }
            set { _iClose = value; OnPropertyChanged(); }
        }

        #endregion

        #region 命令
        protected bool CheckPad()
        {
            if (Password.IsEmpty())
            {
                Method.Hit(Root, "请输入密码");
                if (Method.Find(Root, out PasswordBox tbPassword, "tbPassword")) tbPassword.Focus();
                return false;
            }
            return true;
        }
        public abstract void Login();
        protected virtual Window SetWindow() { return null; }
        protected virtual void OnSet(DependencyObject obj) { }
        protected virtual void OnClose(DependencyObject obj) { }
        protected override void Action(string item)
        {
            base.Action(item);
            switch (item)
            {
                case "登陆":
                    if (iLogining) break;
                    try
                    {
                        iLogining = true;
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
                        Login();
                    }
                    finally
                    {
                        iLogining = false;
                    }
                    break;
                case "设置":
                    var window = SetWindow();
                    if (window != null && Method.ShowWindow(Root, window) == true)
                    {
                        OnSet(Root);
                    }
                    break;
                case "关闭": OnClose(Root); break;
            }
        }

        #endregion

        public LoginPageModel()
        {
            Messenger.Default.Register<LoginLoadMessage>(this, msg => this.Root = msg.Obj);
        }
    }
}