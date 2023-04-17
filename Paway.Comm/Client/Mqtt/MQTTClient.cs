using System;
using System.Text;
using System.Reflection;
using System.Net.Sockets;
using MQTTnet.Client;
using MQTTnet;
using System.Threading;
using MQTTnet.Protocol;
using MQTTnet.Adapter;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using MQTTnet.Internal;

namespace Paway.Comm
{
    /// <summary>
    /// 封装MQTT客户端
    /// </summary>
    public partial class MQTTClient
    {
        #region 变量
        /// <summary>
        /// 默认订阅根主题
        /// </summary>
        private readonly string _topic;
        /// <summary>
        /// 默认订阅主题
        /// </summary>
        protected string Topic { get { return $"{_topic}/{ClientId}"; } }
        /// <summary>
        /// 保活时长
        /// <para>正常MQTT 服务器端会配置一个超时时间，一般为60s， 在这个时间段内一个连接如果没有数据传输的话，服务端会主动断开连接以释放资源</para>
        /// <para>方式1: 最为简单， 将keepalive的时间设置小于 服务端的超时时间，则客户端每隔 keepalive的时间就会给服务端发一个心跳包</para>
        /// <para>方式2: 在通信协议里增加心跳指令</para>
        /// </summary>
        private readonly int keepAlivePeriod;
        private string host;
        private int port;
        /// <summary>
        /// 客户端
        /// </summary>
        private readonly IMqttClient mqttClient;
        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IConnected { get { return mqttClient?.IsConnected == true; } }
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; private set; }

        #endregion

        #region 事件 
        /// <summary>
        /// 连接(断开)后事件
        /// </summary>
        public event Action<bool, Exception> ConnectEvent;

        #endregion

        #region 公开方法
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="topic">默认订阅根主题</param>
        /// <param name="keepAlivePeriod">保活时长(s)</param>
        public MQTTClient(string topic, int keepAlivePeriod = 15)
        {
            this._topic = topic;
            this.keepAlivePeriod = keepAlivePeriod;
            ClientId = Guid.NewGuid().ToString();
            mqttClient = new MqttFactory().CreateMqttClient();
            // 设置消息接收处理程序
            mqttClient.ApplicationMessageReceivedAsync += MessageReceivedAsync;
            // 重连机制
            mqttClient.DisconnectedAsync += DisconnectedAsync;
            //连接默认订阅
            mqttClient.ConnectedAsync += ConnectedAsync;
        }
        /// <summary>
        /// 断开
        /// </summary>
        public Task Disconnect()
        {
            return mqttClient.DisconnectAsync();
        }
        /// <summary>
        /// 登陆连接
        /// </summary>
        /// <param name="host">主机</param>
        /// <param name="port">端口</param>
        public Task<MqttClientConnectResult> Connect(string host, int port)
        {
            return Connect(host, port, false);
        }
        private Task<MqttClientConnectResult> Connect(string host, int port, bool auto)
        {
            this.host = host;
            this.port = port;
            var data = Login(auto);
            if (data?.Result != true) return Task.FromResult<MqttClientConnectResult>(null);
            // 设置 MQTT 客户端选项
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(host, port)// 设置服务器端地址
                .WithCredentials(data.UserName, data.Password)// 设置鉴权参数
                .WithClientId(ClientId)// 设置客户端序列号
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(this.keepAlivePeriod))// 保活时长
                .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V500)// 5.0版本，可指定用户属性
                .Build();// 创建选项
            if (data.Properties != null)
            {
                options.UserProperties = new List<MqttUserProperty>();
                foreach (var item in data.Properties)
                {
                    options.UserProperties.Add(new MqttUserProperty(item.Key, item.Value));
                }
            }
            return mqttClient.ConnectAsync(options);
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        public virtual Task Send(string data, MqttQualityOfServiceLevel level, string topic = null)
        {
            if (data == null) return CompletedTask.Instance;
            return Send(Encoding.UTF8.GetBytes(data), level, topic);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        public virtual Task Send(byte[] buffer, MqttQualityOfServiceLevel level, string topic = null)
        {
            if (buffer == null) return CompletedTask.Instance;
            var appMsg = new MqttApplicationMessageBuilder()
                .WithTopic(topic ?? this.Topic)     // 主题
                .WithPayload(buffer)                // 消息
                .WithQualityOfServiceLevel(level)   // qos
                .WithRetainFlag(false)              // retain
                .Build();
            return mqttClient.PublishAsync(appMsg);
        }
        /// <summary>
        /// 自定义订阅
        /// <para>客户端接收以订阅主题等级为准</para>
        /// </summary>
        public Task SubscribeAsync(string topic, MqttQualityOfServiceLevel level)
        {
            return SubscribeAsync(new List<string> { topic }, level);
        }
        /// <summary>
        /// 自定义订阅
        /// <para>客户端接收以订阅主题等级为准</para>
        /// </summary>
        public Task SubscribeAsync(List<string> list, MqttQualityOfServiceLevel level)
        {
            var mqttFactory = new MqttFactory();
            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder();
            foreach (var item in list)
            {
                mqttSubscribeOptions.WithTopicFilter(c => c.WithTopic(item).WithQualityOfServiceLevel(level));
            }
            return mqttClient.SubscribeAsync(mqttSubscribeOptions.Build());
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        public Task UnsubscribeAsync(params string[] args)
        {
            var mqttFactory = new MqttFactory();
            var mqttUnSubscribeOptions = mqttFactory.CreateUnsubscribeOptionsBuilder();
            foreach (var item in args)
            {
                mqttUnSubscribeOptions.WithTopicFilter(item);
            }
            return mqttClient.UnsubscribeAsync(mqttUnSubscribeOptions.Build());
        }

        #endregion

        #region 接收处理
        /// <summary>
        /// 连接前授权登陆
        /// </summary>
        protected virtual LoginData Login(bool auto) { return null; }
        /// <summary>
        /// 消息处理
        /// </summary>
        protected virtual Task MessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args) { return CompletedTask.Instance; }
        /// <summary>
        /// 连接订阅
        /// </summary>
        private Task ConnectedAsync(MqttClientConnectedEventArgs args)
        {
            ConnectEvent?.Invoke(true, null);
            return CompletedTask.Instance;
        }
        /// <summary>
        /// 断开重连
        /// </summary>
        private Task DisconnectedAsync(MqttClientDisconnectedEventArgs args)
        {
            ConnectEvent?.Invoke(false, args.Exception);
            if (args.Exception?.InnerException is SocketException error)
            {
                switch (error.SocketErrorCode)
                {
                    case SocketError.HostUnreachable:
                    case SocketError.ConnectionAborted:
                    case SocketError.NetworkUnreachable:
                    case SocketError.ConnectionRefused:
                        Thread.Sleep(3000);
                        break;
                    default:
                        Thread.Sleep(1000);
                        break;
                }
            }
            else Thread.Sleep(125);
            if (args.Exception is MqttProtocolViolationException) { }//违反协议
            else if (args.Exception is MqttProtocolViolationException) { }// 验证失败
            else
            {
                Connect(this.host, this.port, true);
            }
            return CompletedTask.Instance;
        }

        #endregion
    }
}