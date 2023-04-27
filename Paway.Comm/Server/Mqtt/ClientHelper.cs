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
    public class ClientHelper
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object syncRoot = new object();

        private readonly List<MClientInfo> clientList = new List<MClientInfo>();

        public List<MClientInfo> Client()
        {
            lock (syncRoot)
            {
                return clientList.FindAll(c => c.Connected);
            }
        }
        public int Count()
        {
            lock (syncRoot)
            {
                return clientList.FindAll(c => c.Connected).Count;
            }
        }
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
        public MClientInfo Client(string clientId)
        {
            lock (syncRoot)
            {
                return clientList.Find(c => c.ClientId == clientId);
            }
        }
        public MClientInfo Client(int userId)
        {
            lock (syncRoot)
            {
                return clientList.Find(c => c.User?.Id == userId);
            }
        }
    }
}
