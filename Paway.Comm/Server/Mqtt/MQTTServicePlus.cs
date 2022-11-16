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
using MQTTnet.Internal;

namespace Paway.Comm
{
    public partial class MQTTServicePlus : MQTTService
    {
        protected ClientHelper gClient;

        public MQTTServicePlus() { }
        public void Start(int port)
        {
            gClient = new ClientHelper();
            base.StartAsync(port).Wait();
            Messenger.Default.Send(new StatuMessage($"mq://+:{port} 已启动"));
        }
        public void Stop()
        {
            base.StopAsync().Wait();
            Messenger.Default.Send(new StatuMessage($"mqtt 已关闭"));
        }
        public MClientInfo Client(int userId)
        {
            return gClient.Client(userId);
        }

        #region 内部事件
        protected override Task ClientConnectedAsync(ClientConnectedEventArgs args)
        {
            var client = gClient.Connect(args.ClientId);
            Messenger.Default.Send(new StatuMessage($"{client?.Desc}上线"));
            return CompletedTask.Instance;
        }
        protected override Task ClientDisConnectedAsync(ClientDisconnectedEventArgs args)
        {
            var client = gClient.DisConnect(args.ClientId);
            if (client != null)
            {
                Messenger.Default.Send(new StatuMessage($"{client?.Desc}下线"));
            }
            return CompletedTask.Instance;
        }

        #endregion

        #region 公开方法
        /// <summary>
        /// 发布消息
        /// </summary>
        public Task Publish(string topic, IMessage msg)
        {
            if (topic == null) return CompletedTask.Instance;
            return Publish(topic, JsonConvert.SerializeObject(msg).Compress());
        }

        #endregion
    }
}
