using System;
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
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 刷新
        /// </summary>
        Refresh = 1 << 0,

        /// <summary>
        /// 添加
        /// </summary>
        Add = 1 << 1,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 1 << 2,
        /// <summary>
        /// 山峡好人
        /// </summary>
        Delete = 1 << 3,
        /// <summary>
        /// 保存
        /// </summary>
        Save = 1 << 4,

        /// <summary>
        /// 导入
        /// </summary>
        Import = 1 << 5,
        /// <summary>
        /// 导出
        /// </summary>
        Export = 1 << 6,

        /// <summary>
        /// 搜索
        /// </summary>
        Search = 1 << 10,
    }
}
