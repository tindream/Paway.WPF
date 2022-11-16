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
        protected virtual Task MessageReceivedAsync(InterceptingPublishEventArgs e, string data, IMessage msg, ref string logMsg) { return CompletedTask.Instance; }
        protected override Task MessageReceivedAsync(InterceptingPublishEventArgs e)
        {
            if (e.ClientId == null) return CompletedTask.Instance;
            string logMsg = null;
            var type = CommType.None;
            try
            {
                var client = gClient.Client(e.ClientId);
                if (client == null)
                {
                    $"{e.ClientId}>未授权连接".Log(LeveType.Error);
                    return CompletedTask.Instance;
                }
                logMsg = $"{client.Desc}";
                var data = e.ApplicationMessage.Payload.Decompress();
                var msg = JsonConvert.DeserializeObject<IMessage>(data);
                logMsg += $">{msg.Type.Description()}";
                var task = MessageReceivedAsync(e, data, msg, ref logMsg);
                logMsg.Log();
                return task;
            }
            #region catch
            catch (JsonReaderException)
            {
                $"{logMsg}>未定义消息>{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}".Log(LeveType.Error);
                return Publish(e.ApplicationMessage.Topic, new ErrorMessage(CommType.None, "未定义消息"));
            }
            catch (Exception ex)
            {
                string error = ex.Message();
                if (ex.InnerException() is DbException || ex.InnerException() is WarningException)
                {
                    $"{logMsg}>{error}".Log(LeveType.Warn);
                }
                else
                {
                    $"{logMsg}>{ex}".Log(LeveType.Error);
                }
                if (type != CommType.None)
                {
                    return Publish(e.ApplicationMessage.Topic, new ErrorMessage(type, error));
                }
                return CompletedTask.Instance;
            }
            finally
            {//不回复
                e.ApplicationMessage.Topic = "";
            }

            #endregion
        }

        #endregion
    }
}
