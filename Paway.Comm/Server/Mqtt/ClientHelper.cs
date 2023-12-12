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
        private readonly List<MClientInfo> clientList = new List<MClientInfo>();

        /// <summary>
        /// 客户端在线列表
        /// </summary>
        public List<MClientInfo> Clients()
        {
            lock (clientList)
            {
                return clientList.FindAll(c => c.Connected);
            }
        }
        /// <summary>
        /// 客户端在线列表数量
        /// </summary>
        public int Count()
        {
            lock (clientList)
            {
                return clientList.FindAll(c => c.Connected).Count;
            }
        }
        /// <summary>
        /// 查询客户端
        /// </summary>
        public MClientInfo Client(string clientId)
        {
            lock (clientList)
            {
                return clientList.Find(c => c.ClientId == clientId);
            }
        }
        /// <summary>
        /// 查询客户端
        /// </summary>
        public MClientInfo Client(int userId)
        {
            lock (clientList)
            {
                return clientList.Find(c => c.User?.Id == userId);
            }
        }

        /// <summary>
        /// 尝试添加客户端，已存在时，重置时间和在线状态
        /// </summary>
        public void Add(MClientInfo info)
        {
            lock (clientList)
            {
                var client = clientList.Find(c => c.ClientId == info.ClientId);
                if (client == null) clientList.Add(info);
                else Connect(info.ClientId);
                $"授权连接: {info.Desc}".Log();
            }
        }
        /// <summary>
        /// 按用户、设备 添加客户端
        /// </summary>
        public MClientInfo Add(string ip, IUser user)
        {
            lock (clientList)
            {
                var client = clientList.Find(c => c.ClientId == user.VerCode);
                if (client == null)
                {
                    client = new MClientInfo(user.VerCode, ip, user);
                    clientList.Add(client);
                }
                return client;
            }
        }
        /// <summary>
        /// 客户端上线
        /// </summary>
        public MClientInfo Connect(string clientId)
        {
            lock (clientList)
            {
                var client = clientList.Find(c => c.ClientId == clientId);
                if (client != null)
                {
                    client.Connected = true;
                    client.ConnectTime = DateTime.Now;
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
            lock (clientList)
            {
                var client = clientList.Find(c => c.ClientId == clientId);
                if (client != null)
                {
                    //断开重连时可能不再验证连接
                    client.Connected = false;
                    client.ConnectTime = DateTime.Now;
                }
                return client;
            }
        }
        /// <summary>
        /// 移除客户端
        /// </summary>
        public void Remove(string clientId)
        {
            lock (clientList)
            {
                clientList.RemoveAll(c => c.ClientId == clientId);
            }
        }
    }
}
