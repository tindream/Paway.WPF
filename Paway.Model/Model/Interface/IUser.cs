using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 用户定义
    /// </summary>
    public interface IUser : ICustomName, IId
    {
        /// <summary>
        /// 用户
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        string Display { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        bool IStatu { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        DateTime LoginOn { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        object Tag();
    }
}
