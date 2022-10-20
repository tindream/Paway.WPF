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
        /// N1
        /// </summary>
        [Description("D")]
        N1 = 1 << 0,
        /// <summary>
        /// N2
        /// </summary>
        [Description("C")]
        N2 = 1 << 1,
        /// <summary>
        /// N3
        /// </summary>
        [Description("B")]
        N3 = 1 << 2,
        /// <summary>
        /// N4
        /// </summary>
        [Description("A")]
        N4 = 1 << 3,
    }
}
