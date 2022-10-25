using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Paway.Test
{
    public class LoginPageModel : ViewModelBase
    {
        #region 属性
        private DependencyObject Root;
        /// <summary>
        /// 登陆限制，防止重复提交
        /// </summary>
        private volatile bool aloneLogin;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged(); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged(); }
        }

        #endregion

        #region 命令
        public ICommand MenuCommand => new RelayCommand<string>(item => Action(item));
        public void Action(string item)
        {
            switch (item)
            {
                case "登陆":
                    if (UserName.IsEmpty())
                    {
                        Method.Hit(Config.Window, "请输入用户名");
                        if (Method.Find(Root, out TextBoxEXT tbUserName, "tbUserName")) tbUserName.Focus();
                        return;
                    }
                    //if (Password.IsEmpty())
                    //{
                    //    Method.Hit(Config.Window, "请输入密码"); return;
                    //}
                    try
                    {
                        if (aloneLogin) return;
                        aloneLogin = true;
                        //if (Config.MQClient == null) Config.MQClient = new MeetClient();
                        //Messenger.Default.Send(new StatuMessage($"{UserName} 正在登陆", false));
                        //Method.Progress(Config.Window, "正在登陆...", ad =>
                        //{
                        //    Config.User.Name = UserName;
                        //    Config.User.Pad = EncryptHelper.MD5(Password + Config.Suffix);
                        //    Config.MQClient.Connect(Config.LocationClient.Host, Config.LocationClient.MQPort, Config.User).Wait();
                        //}, () =>
                        //{
                        //    Config.User = new UserInfo { Name = UserName };
                        //    Messenger.Default.Send(new StatuMessage($"{Config.UserID} 登陆成功", false));
                        //    Messenger.Default.Send(new LoginMessage());
                        //}, ex =>
                        //{
                        //    if (ex.InnerException() is MqttConnectingFailedException mqEx)
                        //    {
                        //        switch (mqEx.ResultCode)
                        //        {
                        //            case MqttClientConnectResultCode.NotAuthorized:
                        //                Messenger.Default.Send(new StatuMessage("未授权", LeveType.Error));
                        //                break;
                        //            case MqttClientConnectResultCode.Banned:
                        //                Messenger.Default.Send(new StatuMessage("用户已登陆", LeveType.Error));
                        //                break;
                        //            case MqttClientConnectResultCode.BadUserNameOrPassword:
                        //                Messenger.Default.Send(new StatuMessage("用户名或密码错误", LeveType.Error));
                        //                break;
                        //        }
                        //    }
                        //    else if (ex.InnerException() is SocketException skEx)
                        //    {
                        //        Messenger.Default.Send(new StatuMessage(skEx.Message, LeveType.Error));
                        //    }
                        //    else
                        //    {
                        //        Messenger.Default.Send(new StatuMessage(ex));
                        //    }
                        //}, () =>
                        //{
                        //    aloneLogin = false;
                        //});
                    }
                    catch (Exception ex)
                    {
                        Messenger.Default.Send(new StatuMessage(ex));
                        return;
                    }
                    break;
            }
        }

        #endregion

        public LoginPageModel()
        {
            Messenger.Default.Register<LoginLoadMessage>(this, msg => this.Root = msg.Obj);
        }
    }
}