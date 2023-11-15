using System;
using System.Collections.Generic;
using System.Linq;
using Paway.Helper;
using MQTTnet;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Data.Common;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using MQTTnet.Server;
using System.Threading.Tasks;
using MQTTnet.Internal;

namespace Paway.Comm
{
    public partial class MQTTServicePlus
    {
        #region 消息处理
        /// <summary>
        /// 默认不处理，会直接转发
        /// <para>中止：e.ApplicationMessage.Topic = string.Empty;</para>
        /// <para>手动处理转发：Publish(e.ApplicationMessage.Topic, e.ApplicationMessage.Payload);</para>
        /// </summary>
        protected virtual void MessageReceivedAsync(InterceptingPublishEventArgs e, string data, IMessage msg, ref string logMsg) { }
        protected override Task MessageReceivedAsync(InterceptingPublishEventArgs e)
        {
            if (e.ClientId == "SenderClientId") return CompletedTask.Instance;
            if (e.ClientId == null)
            {
                e.ApplicationMessage.Topic = string.Empty;
                return CompletedTask.Instance;
            }
            string logMsg = null;
            var type = CommType.None;
            string data = null;
            try
            {
                var client = gClient.Client(e.ClientId);
                if (client == null)
                {
                    $"{e.ClientId}>未授权连接".Log(LeveType.Error);
                    return CompletedTask.Instance;
                }
                logMsg = $"{client.Desc}";
                data = e.ApplicationMessage.Payload.Decompress();
                var msg = JsonConvert.DeserializeObject<CommMessage>(data);
                type = msg.Type;
                MessageReceivedAsync(e, data, msg, ref logMsg);
                logMsg.Log();
            }
            #region catch
            catch (JsonReaderException)
            {
                e.ApplicationMessage.Topic = string.Empty;
                $"{logMsg}>未定义消息>{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}".Log(LeveType.Error);
                Publish($"{_topic}/{e.ClientId}", new ErrorMessage(CommType.None, "未定义消息"));
            }
            catch (Exception ex)
            {
                e.ApplicationMessage.Topic = string.Empty;
                var error = ex.Message();
                if (!data.IsEmpty()) error = $"{error}\n[data]{data}";
                if (ex.IExist(typeof(WarningException)))
                {
                    $"{logMsg}>{error}".Log(LeveType.Warn);
                }
                else if (ex.IExist(typeof(DbException)))
                {
                    $"{logMsg}>{error}".Log(LeveType.Error);
                }
                else
                {
                    error = $"{ex.NullReferenceMessage()}{ex}";
                    if (!data.IsEmpty()) error = $"{error}\n[data]{data}";
                    $"{logMsg}>{error}".Log(LeveType.Error);
                }
                if (type != CommType.None)
                {
                    Publish($"{_topic}/{e.ClientId}", new ErrorMessage(type, ex.Message()));
                }
            }
            return CompletedTask.Instance;

            #endregion
        }

        #endregion
    }
}
