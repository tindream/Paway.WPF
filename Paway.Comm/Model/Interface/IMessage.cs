using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯命令接口定义
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// 通讯命令类型
        /// </summary>
        CommType Type { get; set; }
        /// <summary>
        /// 通讯消息等级
        /// </summary>
        MqttQualityOfServiceLevel Level { get; set; }

        /// <summary>
        /// 通讯消息转byte[]后处理
        /// </summary>
        byte[] Buffer();
        /// <summary>
        /// byte[]转实体后处理
        /// </summary>
        void Parse(byte[] data);
    }
}
