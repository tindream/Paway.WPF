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
        private Dictionary<string, string> properties;

        public MQTTClientPlus() : base(Config.Topic)
        {
            this.ConnectEvent += TestClient_ConnectEvent;
        }
        private void TestClient_ConnectEvent(bool arg1, Exception arg2)
        {
            Messenger.Default.Send(new ConnectMessage(arg1));
            if (arg1) Subscribes();
        }
        /// <summary>
        /// 连接成功后订阅
        /// </summary>
        protected virtual void Subscribes()
        {
            base.SubscribeAsync(Config.TopicAdmin, MqttQualityOfServiceLevel.AtLeastOnce);
            base.SubscribeAsync(Config.TopicAll, MqttQualityOfServiceLevel.AtLeastOnce);
            base.SubscribeAsync(this.Topic, MqttQualityOfServiceLevel.AtLeastOnce);
        }
        /// <summary>
        /// 注册参数
        /// </summary>
        protected override LoginData Login(bool auto)
        {
            if (auto)
            {
                if (this.user.Id > 0) return new LoginData(true, this.user.Id.ToString(), properties: this.properties);
                return null;
            }
            return new LoginData(true, this.user.UserName, this.user.Password, this.properties);
        }
        /// <summary>
        /// 注册完成
        /// </summary>
        protected virtual void Logined() { }

        #region 外部方法
        public void Connect(string host, int port, IUser user, Dictionary<string, string> properties = null)
        {
            this.user = user;
            this.properties = properties;
            var result = base.Connect(host, port).Result;
            var response = result.UserProperties.Find(c => c.Name == "user");
            if (response != null)
            {
                JsonConvert.DeserializeObject(response.Value, user.GetType()).Clone(this.user);
                Logined();
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public Task Send(IMessage msg, string topic = null)
        {
            if (!IConnected) throw new WarningException("正在连接，请稍候...");
            var buffer = JsonConvert.SerializeObject(msg).Compress();
            return Send(buffer, msg.Level, topic);
        }

        #endregion
    }
}
