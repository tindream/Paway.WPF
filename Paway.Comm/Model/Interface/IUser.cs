using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 用户定义
    /// </summary>
    public interface IUser : ICustomName, IName, ITag, IId
    {
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
        EnableType Enable { get; set; }
        /// <summary>
        /// 登陆时间
        /// </summary>
        DateTime LoginOn { get; set; }

        /// <summary>
        /// 唯一标记
        /// <para>授权标记</para>
        /// </summary>
        string VerCode { get; }
    }
}
