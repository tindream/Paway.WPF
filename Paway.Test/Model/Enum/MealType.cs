using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    public enum MealType
    {
        #region 本地消息
        None,

        #endregion

        #region 通讯消息
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
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

        #endregion
    }
}
