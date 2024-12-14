using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    /// <summary>
    /// 连接消息
    /// </summary>
    public class ConnectMessage
    {
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool Connectd { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 连接消息
        /// </summary>
        public ConnectMessage() { }
        /// <summary>
        /// 连接消息
        /// </summary>
        public ConnectMessage(bool connectd)
        {
            this.Connectd = connectd;
        }
        /// <summary>
        /// 连接消息
        /// </summary>
        public ConnectMessage(bool connectd, string msg) : this(connectd)
        {
            this.Message = msg;
        }
    }
    /// <summary>
    /// 连接消息2
    /// </summary>
    public class Connect2Message : ConnectMessage
    {
        /// <summary>
        /// 连接消息2
        /// </summary>
        public Connect2Message() { }
        /// <summary>
        /// 连接消息2
        /// </summary>
        public Connect2Message(bool connectd) : base(connectd) { }
        /// <summary>
        /// 连接消息
        /// </summary>
        public Connect2Message(bool connectd, string msg) : base(connectd, msg) { }
    }
}
