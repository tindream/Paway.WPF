using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Model
{
    /// <summary>
    /// 管理数据结构
    /// </summary>
    [Serializable]
    [Table("Sys_Admins")]
    public class AdminBaseInfo : BaseInfo, IInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
