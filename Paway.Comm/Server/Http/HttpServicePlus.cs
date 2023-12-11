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
using MQTTnet.Server;
using System.Threading.Tasks;
using MQTTnet.Protocol;
using MQTTnet;
using System.ComponentModel;
using System.Net;
using Newtonsoft.Json;
using System.Web;

namespace Paway.Comm
{
    /// <summary>
    /// Http服务(数据库标准操作)
    /// </summary>
    public partial class HttpServicePlus : HttpService
    {
        private readonly IDataService server;
        public event Action<HttpListenerContext, SyncMessage> SyncEvent;
        public event Func<HttpListenerContext, MClientInfo> ClientEvent;
        public event Action<HttpListenerContext> FileEvent;

        public HttpServicePlus(IDataService server, int port) : base(port)
        {
            this.server = server;
        }

        private MClientInfo Client(HttpListenerContext context)
        {
            return ClientEvent?.Invoke(context);
        }
        protected override void MessageHandle(HttpListenerContext context, string url, ref string data, ref string logMsg)
        {
            try
            {
                data = data.Decompress();
                var client = Client(context);
                logMsg = $"[{client?.Desc}] {logMsg}";
                if (MessageHandleFile(context, data, ref logMsg)) return;

                var msg = JsonConvert.DeserializeObject<CommMessage>(data);
                if (msg == null)
                {
                    base.MessageHandle(context, url, ref data, ref logMsg);
                    return;
                }
                switch (msg.Type)
                {
                    case CommType.Sync:
                        MessageHandleSync(context, data, ref logMsg);
                        break;
                    default:
                        logMsg += $">{msg.Type.Description()}";
                        break;
                }
                switch (msg.Type)
                {
                    case CommType.Sync: break;
                    case CommType.SyncExecuteList:
                        var sql = JsonConvert.DeserializeObject<SyncExecuteListMessage>(data);
                        var sqlList = server.ExecuteList(sql.ObjType, sql.Sql);
                        Response(context, sqlList);
                        break;
                    case CommType.SyncExecuteScalar:
                        sql = JsonConvert.DeserializeObject<SyncExecuteListMessage>(data);
                        var obj = server.ExecuteScalar(sql.Sql);
                        Response(context, obj);
                        break;
                }
            }
            finally
            {
                Console.WriteLine(logMsg);
            }
        }
        private void MessageHandleSync(HttpListenerContext context, string data, ref string logMsg)
        {
            var syncBsae = JsonConvert.DeserializeObject<SyncBaseMessage>(data);
            logMsg += $">{syncBsae.OperType}";
            IList syncList = null;
            var typeName = string.Empty;
            var timeNow = DateTime.Now;
            switch (syncBsae.OperType)
            {
                case OperType.Insert:
                    var syncInsert = JsonConvert.DeserializeObject<SyncInsertMessage>(data);
                    typeName = syncInsert.IType.Description();
                    syncList = CMethod.JsonToIList(syncInsert.List, syncInsert.IType);
                    if (typeof(IOperateInfo).IsAssignableFrom(syncInsert.IType))
                    {
                        foreach (IOperateInfo item in syncList)
                        {
                            item.CreateOn = timeNow;
                        }
                    }
                    server.Insert(syncList);
                    Response(context, syncList);
                    break;
                case OperType.Update:
                    var syncUpdate = JsonConvert.DeserializeObject<SyncUpdateMessage>(data);
                    typeName = syncUpdate.IType.Description();
                    syncList = CMethod.JsonToIList(syncUpdate.List, syncUpdate.IType);
                    if (typeof(IOperateInfo).IsAssignableFrom(syncUpdate.IType))
                    {
                        foreach (IOperateInfo item in syncList)
                        {
                            item.UpdateOn = timeNow;
                        }
                    }
                    server.Replace(syncList, syncUpdate.Args);
                    Response(context, syncList);
                    break;
                case OperType.Delete:
                    var syncDelete = JsonConvert.DeserializeObject<SyncDeleteMessage>(data);
                    typeName = syncDelete.IType.Description();
                    syncList = CMethod.JsonToIList(syncDelete.List, syncDelete.IType);
                    server.Delete(syncList);
                    Response(context);
                    break;
                case OperType.Query:
                    var syncQuery = JsonConvert.DeserializeObject<SyncFindMessage>(data);
                    typeName = syncQuery.IType.Description();
                    var findList = server.Find(syncQuery.IType, syncQuery.Find, count: syncQuery.Count, args: syncQuery.Args);
                    Response(context, findList);
                    break;
            }
            logMsg += $">{typeName}";
            if (syncList != null) SyncEvent?.Invoke(context, new SyncMessage(syncList, syncBsae.OperType) { ClientId = syncBsae.ClientId });
        }
    }
}
