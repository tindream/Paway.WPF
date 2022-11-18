using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    public class ConnectMessage
    {
        public bool Connectd { get; set; }

        public ConnectMessage() { }
        public ConnectMessage(bool connectd)
        {
            this.Connectd = connectd;
        }
    }
    /// <summary>
    /// 连接状态2
    /// </summary>
    public class Connect2Message
    {
        public bool Connectd { get; set; }

        public Connect2Message() { }
        /// <summary>
        /// 连接状态2
        /// </summary>
        public Connect2Message(bool connectd)
        {
            this.Connectd = connectd;
        }
    }
}
