using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    /// <summary>
    /// 登录消息
    /// </summary>
    public class LoginMessage
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录消息
        /// </summary>
        public LoginMessage() { }
        /// <summary>
        /// 登录消息
        /// </summary>
        public LoginMessage(string userName)
        {
            this.UserName = userName;
        }
    }
}
