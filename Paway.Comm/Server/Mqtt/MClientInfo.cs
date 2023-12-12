using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// MQTT客户端属性
    /// </summary>
    [Serializable]
    public class MClientInfo
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 远程连接口
        /// </summary>
        public string Endpoint { get; set; }
        /// <summary>
        /// 连接时间
        /// </summary>
        public DateTime ConnectTime { get; set; }
        /// <summary>
        /// 心跳时间
        /// </summary>
        public DateTime HeartTime { get; set; }

        /// <summary>
        /// 连接用户
        /// </summary>
        public IUser User { get; set; }
        /// <summary>
        /// 状况标识
        /// </summary>
        public string Heard { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get
            {
                var desc = User.CustomName;
                if (Heard != null) desc = $"[{Heard}]{desc}";
                return desc;
            }
        }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Connected { get; set; }

        /// <summary>
        /// MQTT客户端属性
        /// </summary>
        public MClientInfo(IUser user)
        {
            this.User = user;
            this.ClientId = user.ClientId;
        }
        /// <summary>
        /// MQTT客户端属性
        /// </summary>
        public MClientInfo(string clientId, string endpoint, IUser user)
        {
            this.ClientId = clientId;
            this.Endpoint = endpoint;
            this.User = user;
        }
    }
}
