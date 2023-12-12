using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paway.Comm
{
    /// <summary>
    /// MQTT客户端列表管理
    /// </summary>
    public class ClientHelper
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        private readonly List<MClientInfo> clientList = new List<MClientInfo>();

        /// <summary>
        /// 客户端在线列表
        /// </summary>
        public List<MClientInfo> Clients()
        {
            lock (syncRoot)
            {
                return clientList.FindAll(c => c.Connected);
            }
        }
        /// <summary>
        /// 客户端在线列表数量
        /// </summary>
        public int Count()
        {
            lock (syncRoot)
            {
                return clientList.FindAll(c => c.Connected).Count;
            }
        }
        /// <summary>
        /// 尝试添加客户端，已存在时，重置时间和在线状态
        /// </summary>
        public void Add(MClientInfo info)
        {
            lock (syncRoot)
            {
                var client = clientList.Find(c => c.ClientId == info.ClientId);
                if (client == null) clientList.Add(info);
                else Connect(info.ClientId);
                $"授权连接: {info.Desc}".Log();
            }
        }
        /// <summary>
        /// 客户端上线
        /// </summary>
        public MClientInfo Connect(string clientId)
        {
            lock (syncRoot)
            {
                var client = clientList.Find(c => c.ClientId == clientId);
                if (client != null)
                {
                    client.Connected = true;
                    client.DateTime = DateTime.Now;
                }
                else
                {
                    $"未授权连接: {clientId}".Log();
                }
                return client;
            }
        }
        /// <summary>
        /// 客户端掉线
        /// </summary>
        public MClientInfo DisConnect(string clientId)
        {
            lock (syncRoot)
            {
                var client = clientList.Find(c => c.ClientId == clientId);
                if (client != null)
                {
                    //断开重连时可能不再验证连接
                    client.Connected = false;
                    client.DateTime = DateTime.Now;
                }
                return client;
            }
        }
        /// <summary>
        /// 查询客户端
        /// </summary>
        public MClientInfo Client(string clientId)
        {
            lock (syncRoot)
            {
                return clientList.Find(c => c.ClientId == clientId);
            }
        }
        /// <summary>
        /// 查询客户端
        /// </summary>
        public MClientInfo Client(int userId)
        {
            lock (syncRoot)
            {
                return clientList.Find(c => c.User?.Id == userId);
            }
        }
    }
}
