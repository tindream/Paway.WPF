﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Model
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            tbPassword.KeyDown += TbPassword_KeyDown;
        }
        private void TbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.DataContext is LoginPageModel login) login.Login();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Messenger.Default.Send(new LoginLoadMessage() { Obj = Root });
            Method.BeginInvoke(() =>
            {
                if (this.DataContext is LoginPageModel login)
                {
                    if (login.IUserList) cbxUserName.Focus();
                    else tbUserName.Focus();
                }
            });
        }
    }
}