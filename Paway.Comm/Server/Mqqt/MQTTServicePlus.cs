using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
using Paway.Utils;
using Paway.Helper;
using System.Threading;
using System.IO;
using System.Collections;
using System.Xml;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Protocol;
using GalaSoft.MvvmLight.Messaging;
using Paway.Model;

namespace Paway.Comm
{
    public partial class MQTTServicePlus : MQTTService
    {
        protected ClientHelper gClient;

        public MQTTServicePlus() { }
        public new void Start(int port)
        {
            gClient = new ClientHelper();
            base.Start(port).Wait();
            Messenger.Default.Send(new StatuMessage($"mq://+:{port} 已启动"));
        }
        public MClientInfo Client(int userId)
        {
            return gClient.Client(userId);
        }

        #region 内部事件
        protected override void ClientConnected(ClientConnectedEventArgs args)
        {
            var client = gClient.Connect(args.ClientId);
            Messenger.Default.Send(new StatuMessage($"{client?.Desc}上线"));
        }
        protected override void ClientDisConnected(ClientDisconnectedEventArgs args)
        {
            var client = gClient.DisConnect(args.ClientId);
            if (client == null) return;
            Messenger.Default.Send(new StatuMessage($"{client?.Desc}下线"));
        }

        #endregion

        #region 公开方法
        /// <summary>
        /// 发布消息
        /// </summary>
        public Task Publish(string topic, IMessage msg)
        {
            if (topic == null) return Task.Delay(0);
            return Publish(topic, JsonConvert.SerializeObject(msg).Compress(), msg.Level);
        }

        #endregion
    }
}
