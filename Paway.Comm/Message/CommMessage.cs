using MQTTnet.Protocol;
using Newtonsoft.Json;
using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯基类实体定义
    /// </summary>
    [Serializable]
    public class CommMessage : IMessage
    {
        /// <summary>
        /// 通讯命令类型
        /// </summary>
        public CommType Type { get; set; }
        /// <summary>
        /// 通讯消息等级
        /// </summary>
        [JsonIgnore]
        public MqttQualityOfServiceLevel Level { get; set; }

        /// <summary>
        /// </summary>
        public CommMessage() { }
        /// <summary>
        /// </summary>
        public CommMessage(CommType type)
        {
            this.Type = type;
            this.Level = MqttQualityOfServiceLevel.AtLeastOnce;
        }
        /// <summary>
        /// 通讯消息转byte[]后处理
        /// </summary>
        public virtual byte[] Buffer() { return null; }
        /// <summary>
        /// byte[]转实体后处理
        /// </summary>
        public virtual void Parse(byte[] data) { }
    }
}
