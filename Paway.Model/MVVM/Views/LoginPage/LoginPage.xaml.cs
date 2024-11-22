using GalaSoft.MvvmLight.Messaging;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// <summary>
        /// 登录页
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
            tbPassword.KeyDown += TbPassword_KeyDown;
        }
        private void TbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.DataContext is LoginPageModel login) login.Login();
        }
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Messenger.Default.Send(new LoginLoadMessage() { Obj = Root, MenuItem = menu });
            PMethod.BeginInvoke(() =>
            {
                if (this.DataContext is LoginPageModel login)
                {
                    if (login.IUserList) cbxUserName.Focus();
                    else tbUserName.Focus();
                }
            });
        }
        /// <summary>
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (e.ClickCount == 2)
            {
                var version = $"V{Assembly.GetEntryAssembly().GetName().Version}";
                Messenger.Default.Send(new StatuMessage(version, true));
            }
        }
    }
}
