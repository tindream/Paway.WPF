using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 操作记录接口
    /// </summary>
    public interface IOperateInfo : IId
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        int CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateOn { get; set; }
        /// <summary>
        /// 更新用户
        /// </summary>
        int UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime UpdateOn { get; set; }
    }
}
