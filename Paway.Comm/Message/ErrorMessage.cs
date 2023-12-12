using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯错误实体定义
    /// </summary>
    [Serializable]
    public class ErrorMessage : CommMessage
    {
        /// <summary>
        /// 原通讯命令类型
        /// </summary>
        public CommType FromType { get; set; }
        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 通讯错误实体定义
        /// </summary>
        public ErrorMessage() : base(CommType.Error) { }
        /// <summary>
        /// 通讯错误实体定义
        /// </summary>
        public ErrorMessage(CommType type, string msg) : this()
        {
            this.FromType = type;
            this.Message = msg;
        }
    }
}
