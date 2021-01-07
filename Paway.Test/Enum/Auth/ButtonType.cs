using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 按钮权限
    /// </summary>
    [Flags]
    public enum ButtonType
    {
        [Description(TConfig.None)]
        None,
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Insert = 1 << 0,
        /// <summary>
        /// 编辑
        /// </summary>
        [Description("编辑")]
        Update = 1 << 1,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 1 << 2,
    }
}
