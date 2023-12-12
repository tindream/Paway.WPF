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
using System.Data.SqlClient;
using System.Data.Common;
using System.Web;

namespace Paway.Comm
{
    /// <summary>
    /// Http标准服务
    /// </summary>
    public partial class HttpService
    {
        private HttpListener _listener;
        /// <summary>
        /// 通讯端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 初始化端口
        /// </summary>
        public HttpService(int port)
        {
            Port = port;
        }
        /// <summary>
        /// 启动服务
        /// <para>windows系统下需添加端口</para>
        /// <para>netsh http delete urlacl url=http://+:8088/</para>
        /// <para>netsh http add urlacl url=http://+:8088/ user=Everyone</para>
        /// </summary>
        public void Start()
        {
            _listener = new HttpListener();
            string host = $"http://+:{Port}/";
            _listener.Prefixes.Add(host);
            _listener.Start();
            _listener.BeginGetContext(ProcessRequest, null);
            CConfig.AddStatuLog($"{host} 已启动");
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        protected virtual void MessageHandle(HttpListenerContext context, ref string data, ref string logMsg)
        {
            Response(context, "Hello,World");
        }
        /// <summary>
        /// Http回复消息
        /// </summary>
        protected void Response(HttpListenerContext context, string msg, bool result = true)
        {
            Response(context, new HttpResponseMessage(result, msg));
        }
        /// <summary>
        /// Http回复数据
        /// </summary>
        protected void Response(HttpListenerContext context, object data = null, int total = 0, string msg = "success")
        {
            Response(context, new HttpResponseMessage(true, msg, data, total));
        }
        /// <summary>
        /// Http回复
        /// </summary>
        private void Response(HttpListenerContext context, HttpResponseMessage data)
        {
            var zip = JsonConvert.SerializeObject(data).CompressBase64();
            var buffer = Encoding.UTF8.GetBytes(zip);
            if (context.Response.ContentLength64 == -1) CConfig.AddStatuLog($"连接已关闭：{data.msg}", LeveType.Error);
            else Response(context, buffer);
        }
        /// <summary>
        /// Http回复
        /// </summary>
        protected void Response(HttpListenerContext context, byte[] buffer)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentLength64 = buffer.Length;
            if (buffer.Length < 128 * 1024)
            {
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                var length = 16 * 1024;
                var count = buffer.Length / length;
                if (buffer.Length % length > 0) count++;
                for (var i = 0; i < count; i++)
                {
                    context.Response.OutputStream.Write(buffer, i * length, i == count - 1 ? buffer.Length - i * length : length);
                }
            }
        }
        private void ProcessRequest(IAsyncResult result)
        {
            HttpListenerContext context = null;
            string logMsg = null;
            string data = null;
            try
            {
                _listener.BeginGetContext(ProcessRequest, null);
                context = _listener.EndGetContext(result);
                if (context.Request.HttpMethod == "OPTIONS")
                {
                    Response(context, "ok");
                    return;
                }
                logMsg = HttpUtility.UrlDecode(context.Request.Url.PathAndQuery);
                // 获取请求内容
                data = new StreamReader(context.Request.InputStream, Encoding.UTF8).ReadToEnd();
                MessageHandle(context, ref data, ref logMsg);
            }
            catch (HttpListenerException ex)
            {
                var error = ex.Message();
                if (!data.IsEmpty()) error = $"{error}\n[data]{data}";
                CConfig.AddStatuLog($"[HTTP]网络异常-{ex.ErrorCode}>{logMsg}>{error}", LeveType.Warn);
            }
            catch (Exception ex)
            {
                if (context == null) return;
                var error = ex.Message();
                if (!data.IsEmpty()) error = $"{error}\n[data]{data}";
                if (ex.IExist(typeof(WarningException)))
                {
                    CConfig.AddStatuLog($"[HTTP]{logMsg}>{error}", LeveType.Warn);
                }
                else if (ex.IExist(typeof(DbException)))
                {
                    CConfig.AddStatuLog($"[HTTP]{logMsg}>{error}", LeveType.Error);
                }
                else
                {
                    error = $"{ex.NullReferenceMessage()}{ex}";
                    if (!data.IsEmpty()) error = $"{error}\n[data]{data}";
                    CConfig.AddStatuLog($"[HTTP]{logMsg}>{error}", LeveType.Error);
                }
                try
                {
                    Response(context, ex.Message(), false);
                }
                catch (ProtocolViolationException) { }
                catch (HttpListenerException) { }
                catch (Exception e)
                {
                    e.Log();
                }
            }
            finally
            {
                Close(context);
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        protected void Close(HttpListenerContext context)
        {
            try
            {
                context?.Response.Close();
            }
            catch (HttpListenerException) { }
            catch (Exception e)
            {
                e.Log();
            }
        }
    }
}
