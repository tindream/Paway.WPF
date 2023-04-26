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
        #region 原子锁
        private object _syncRoot;
        private object SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        #endregion

        private readonly List<MClientInfo> clientList = new List<MClientInfo>();

        public List<MClientInfo> Client()
        {
            lock (SyncRoot)
            {
                return clientList.FindAll(c => c.Connected);
            }
        }
        public int Count()
        {
            lock (SyncRoot)
            {
                return clientList.FindAll(c => c.Connected).Count;
            }
        }
        public void Add(MClientInfo info)
        {
            lock (SyncRoot)
            {
                var client = clientList.Find(c => c.ClientId == info.ClientId);
                if (client == null) clientList.Add(info);
                else Connect(info.ClientId);
                $"授权连接: {info.Desc}".Log();
            }
        }
        public MClientInfo Connect(string clientId)
        {
            lock (SyncRoot)
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
            lock (SyncRoot)
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
            lock (SyncRoot)
            {
                return clientList.Find(c => c.ClientId == clientId);
            }
        }
        public MClientInfo Client(int userId)
        {
            lock (SyncRoot)
            {
                return clientList.Find(c => c.User?.Id == userId);
            }
        }
    }
}
