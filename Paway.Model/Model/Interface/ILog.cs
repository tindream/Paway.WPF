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
    /// 日志定义
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 用户
        /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        bool IStatu { get; set; }
    }
}
