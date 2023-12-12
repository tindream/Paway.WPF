using MQTTnet.Protocol;
using Newtonsoft.Json;
using Paway.Comm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 回复消息
    /// </summary>
    [Serializable]
    public class ResponseMessage : CommMessage
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public ResponseMessage(CommType type) : base(type) { }
        /// <summary>
        /// 消息
        /// </summary>
        public ResponseMessage(CommType type, string msg) : base(type)
        {
            this.Msg = msg;
        }
    }
    /// <summary>
    /// 查询结果
    /// </summary>
    [Serializable]
    public class ResponseInfoMessage : ResponseMessage
    {
        /// <summary>
        /// 查询结果
        /// </summary>
        public object Info { get; set; }

        /// <summary>
        /// 查询结果
        /// </summary>
        public ResponseInfoMessage(CommType type, object info) : base(type)
        {
            this.Info = info;
        }
    }
    /// <summary>
    /// 查询结果列表
    /// </summary>
    [Serializable]
    public class ResponseListMessage : ResponseMessage
    {
        /// <summary>
        /// 查询结果列表
        /// </summary>
        public IList List { get; set; }

        /// <summary>
        /// 查询结果列表
        /// </summary>
        public ResponseListMessage(CommType type, IList list) : base(type)
        {
            this.List = list;
        }
    }
}
