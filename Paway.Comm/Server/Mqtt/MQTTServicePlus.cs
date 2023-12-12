using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
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
using MQTTnet.Internal;

namespace Paway.Comm
{
    /// <summary>
    /// MQTT服务扩展
    /// </summary>
    public partial class MQTTServicePlus : MQTTService
    {
        /// <summary>
        /// 客户端处理
        /// </summary>
        protected ClientHelper gClient;

        /// <summary>
        /// </summary>
        public MQTTServicePlus() { }
        /// <summary>
        /// 启动服务
        /// </summary>
        public void Start(int port)
        {
            gClient = new ClientHelper();
            base.StartAsync(port, CConfig.Topic).Wait();
            CConfig.AddStatuLog($"mq://+:{port} 已启动");
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            base.StopAsync().Wait();
            CConfig.AddStatuLog($"mqtt 已关闭");
        }
        /// <summary>
        /// 查询指定客户端
        /// </summary>
        public MClientInfo Client(int userId)
        {
            return gClient.Client(userId);
        }
        /// <summary>
        /// 查询客户端在线列表
        /// </summary>
        public List<MClientInfo> Clients()
        {
            return gClient.Clients();
        }

        #region 内部事件
        /// <summary>
        /// 客户端连接后处理
        /// </summary>
        protected override Task ClientConnectedAsync(ClientConnectedEventArgs args)
        {
            var client = gClient.Connect(args.ClientId);
            CConfig.AddStatuLog($"{client?.Desc}上线");
            return base.ClientConnectedAsync(args);
        }
        /// <summary>
        /// 客户端断开后处理
        /// </summary>
        protected override Task ClientDisConnectedAsync(ClientDisconnectedEventArgs args)
        {
            var client = gClient.DisConnect(args.ClientId);
            if (client != null)
            {
                CConfig.AddStatuLog($"{client?.Desc}下线");
            }
            return base.ClientDisConnectedAsync(args);
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
