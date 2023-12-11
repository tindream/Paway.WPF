using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Paway.Helper;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯命令
    /// <para>预留1-10</para>
    /// </summary>
    public enum CommType
    {
        None,
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,
        /// <summary>
        /// 通知
        /// </summary>
        [Description("通知")]
        Notice,

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
