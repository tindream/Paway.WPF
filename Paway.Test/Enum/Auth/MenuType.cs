using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    [Flags]
    public enum MenuType
    {
        [Description(Config.None)]
        None,
        /// <summary>
        /// 用户
        /// </summary>
        [Description("用户")]
        User = 1 << 0,
        /// <summary>
        /// 权限
        /// </summary>
        [Description("权限")]
        Auth = 1 << 3,
    }
}
