using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// HTTP返回消息实体定义
    /// </summary>
    [Serializable]
    public class HttpResponseMessage
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 列表总数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// HTTP返回消息实体定义
        /// </summary>
        public HttpResponseMessage() { }
        /// <summary>
        /// HTTP返回消息实体定义
        /// </summary>
        public HttpResponseMessage(bool result, string msg, object data = null, int total = 0) : this()
        {
            this.code = result ? 200 : 400;
            this.msg = msg;
            this.data = data;
            this.total = total;
        }
    }
}
