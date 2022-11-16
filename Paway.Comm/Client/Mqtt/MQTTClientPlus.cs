using System;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
using Paway.Helper;
using MQTTnet.Client;
using MQTTnet;
using System.Threading;
using MQTTnet.Protocol;
using MQTTnet.Adapter;
using Paway.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;
using Paway.Comm;
using Newtonsoft.Json;
using GalaSoft.MvvmLight.Messaging;
using Paway.Model;

namespace Paway.Comm
{
    public partial class MQTTClientPlus : MQTTClient
    {
        private IUser user;

        public MQTTClientPlus() : base(Config.Topic)
        {
            this.ConnectEvent += TestClient_ConnectEvent;
        }
        private void TestClient_ConnectEvent(bool arg1, Exception arg2)
        {
            Messenger.Default.Send(new ConnectMessage(arg1));
            if (arg1)
            {
                base.SubscribeAsync(Config.TopicAdmin, MqttQualityOfServiceLevel.AtLeastOnce);
                base.SubscribeAsync(this.Topic, MqttQualityOfServiceLevel.AtLeastOnce);
            }
        }
        protected override LoginData Login(bool auto)
        {
            if (auto)
            {
                if (this.user.Id > 0) return new LoginData(true, this.user.Id.ToString());
                return null;
            }
            return new LoginData(true, this.user.UserName, this.user.Password);
        }

        #region 外部方法
        public void Connect(string host, int port, IUser user, Dictionary<string, string> properties = null)
        {
            this.user = user;
            var result = base.Connect(host, port, properties).Result;
            var response = result.UserProperties.Find(c => c.Name == "user");
            if (response != null)
            {
                JsonConvert.DeserializeObject(response.Value, user.GetType()).Clone(this.user);
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public Task Send(IMessage msg)
        {
            if (!IConnected) throw new WarningException("正在连接，请稍候...");
            var buffer = JsonConvert.SerializeObject(msg).Compress();
            return Send(buffer, msg.Level);
        }

        #endregion
    }
}
