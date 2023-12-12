using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 启用状态
    /// </summary>
    public enum EnableType
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        None,
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable,
    }
}
