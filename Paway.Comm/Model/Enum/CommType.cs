using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using Paway.Helper;
using Paway.WPF;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯命令
    /// </summary>
    public enum CommType
    {
        None,
        Error,

        /// <summary>
        /// 同步
        /// </summary>
        [Description("同步")]
        Sync,
        /// <summary>
        /// 同步查询列表
        /// </summary>
        [Description("查询表")]
        SyncExecuteList,
        /// <summary>
        /// 同步查询值
        /// </summary>
        [Description("查询值")]
        SyncExecuteScalar,
    }
}
