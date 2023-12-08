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
        public string ClientId { get; set; }
        public string Endpoint { get; set; }
        public DateTime DateTime { get; set; }
        public IUser User { get; set; }
        public string Heard { get; set; }
        public string Desc
        {
            get
            {
                var desc = User.CustomName;
                if (Heard != null) desc = $"({Heard}){desc}";
                return desc;
            }
        }

        public bool Connected { get; set; }

        public MClientInfo(string clientId, string endpoint, IUser user)
        {
            this.ClientId = clientId;
            this.Endpoint = endpoint;
            this.User = user;
        }
    }
}
