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
    public enum OperateType
    {
        None = 0,
        Refresh = 1 << 0,
        Add = 1 << 1,
        Edit = 1 << 2,
        Delete = 1 << 3,
        Import = 1 << 4,
        Export = 1 << 5,
        Search = 1 << 6,
    }
}
