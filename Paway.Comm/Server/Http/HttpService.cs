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
using MQTTnet.Server;
using System.Threading.Tasks;
using MQTTnet.Protocol;
using MQTTnet;
using System.ComponentModel;
using System.Net;
using Newtonsoft.Json;
using System.Data.SqlClient;
using GalaSoft.MvvmLight.Messaging;
using System.Data.Common;
using Paway.Model;

namespace Paway.Comm
{
    /// <summary>
    /// Http标准服务
    /// </summary>
    public partial class HttpService
    {
        private HttpListener _listener;
        public int _port;

        public HttpService(int port)
        {
            _port = port;
        }
        public void Start()
        {
            _listener = new HttpListener();
            string host = $"http://+:{_port}/";
            _listener.Prefixes.Add(host);
            _listener.Start();
            _listener.BeginGetContext(ProcessRequest, null);
            Messenger.Default.Send(new StatuMessage($"{host} 已启动"));
        }
        public void Stop()
        {
            _listener.Stop();
        }
        /// <summary>
        /// 消息处理
        /// </summary>
        protected virtual void MessageHandle(HttpListenerContext context, string data, ref string logMsg)
        {
            Response(context, "Hello,World");
        }
        /// <summary>
        /// Http回复消息
        /// </summary>
        protected void Response(HttpListenerContext context, string msg, bool result = true)
        {
            Response(context, new ResponseMessage(result, msg));
        }
        /// <summary>
        /// Http回复数据
        /// </summary>
        protected void Response(HttpListenerContext context, object data = null, int total = 0, string msg = "success")
        {
            Response(context, new ResponseMessage(true, msg, data, total));
        }
        /// <summary>
        /// Http回复
        /// </summary>
        private void Response(HttpListenerContext context, ResponseMessage data)
        {
            var zip = JsonConvert.SerializeObject(data).CompressBase64();
            var buffer = Encoding.UTF8.GetBytes(zip);
            Response(context, buffer);
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

                // 获取请求内容
                data = new StreamReader(context.Request.InputStream, Encoding.UTF8).ReadToEnd().Decompress();
                MessageHandle(context, data, ref logMsg);
                if (logMsg != null) Console.WriteLine(logMsg);
            }
            catch (HttpListenerException ex)
            {
                var error = ex.Message();
                if (!data.IsEmpty()) error = $"[data]{data}\n{error}";
                if (!logMsg.IsEmpty()) error = $"{logMsg}>{error}";
                Messenger.Default.Send(new StatuMessage($"网络异常-{ex.ErrorCode}>{error}", LeveType.Warn));
            }
            catch (Exception ex)
            {
                if (context == null) return;
                string error = ex.Message();
                if (ex.InnerException() is DbException || ex.InnerException() is WarningException)
                {
                    if (!data.IsEmpty()) error = $"[data]{data}\n{error}";
                    if (!logMsg.IsEmpty()) error = $"{logMsg}>{error}";
                    Messenger.Default.Send(new StatuMessage(error, LeveType.Warn));
                }
                else
                {
                    error = $"{ex}";
                    if (!data.IsEmpty()) error = $"[data]{data}\n{error}";
                    Messenger.Default.Send(new StatuMessage(error, LeveType.Error));
                }
                try
                {
                    error = ex.Message();
                    Response(context, error, false);
                }
                catch (HttpListenerException) { }
                catch (Exception e)
                {
                    e.Log();
                }
            }
            finally
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
}
