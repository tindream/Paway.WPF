using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.Comm
{
    /// <summary>
    /// 登陆数据
    /// </summary>
    [Serializable]
    public class LoginData
    {
        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// </summary>
        public LoginData() { }
        /// <summary>
        /// 登陆数据
        /// </summary>
        public LoginData(bool result, string userName, string password = null)
        {
            this.Result = result;
            this.UserName = userName;
            this.Password = password;
        }
    }
}
