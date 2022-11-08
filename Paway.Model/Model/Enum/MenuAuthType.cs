﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Model
{
    /// <summary>
    /// 菜单权限
    /// </summary>
    [Flags]
    public enum MenuAuthType
    {
        None = 0,
        Refresh = 1 << 0,

        Add = 1 << 1,
        Edit = 1 << 2,
        Delete = 1 << 3,
        Save = 1 << 4,

        Import = 1 << 5,
        Export = 1 << 6,

        Search = 1 << 10,
    }
}
