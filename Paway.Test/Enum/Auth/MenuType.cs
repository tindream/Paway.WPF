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
    public enum MenuType : byte
    {
        [Description(Config.None)]
        None,
        /// <summary>
        /// N
        /// </summary>
        N1 = 1 << 0,
        /// <summary>
        /// N
        /// </summary>
        N2 = 1 << 1,
        /// <summary>
        /// N
        /// </summary>
        N3 = 1 << 2,
    }
}
