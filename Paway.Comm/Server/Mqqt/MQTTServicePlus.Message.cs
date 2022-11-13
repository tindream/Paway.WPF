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

namespace Paway.Comm
{
    public partial class MQTTServicePlus
    {
        #region 消息处理
        protected virtual bool MessageReceived(IMessage msg, MqttApplicationMessage e, ref string logMsg) { return true; }
        protected override void MessageReceived(ApplicationMessageNotConsumedEventArgs e)
        {
            if (e.SenderId == null) return;
            string logMsg = null;
            var type = CommType.None;
            try
            {
                var client = gClient.Client(e.SenderId);
                if (client == null)
                {
                    $"{e.SenderId}>未授权连接".Log(LeveType.Error);
                    return;
                }
                logMsg = $"{client.Desc}";
                var data = e.ApplicationMessage.Payload.Decompress();
                var msg = JsonConvert.DeserializeObject<IMessage>(data);
                logMsg += $">{msg.Type.Description()}";
                if (!MessageReceived(msg, e.ApplicationMessage, ref logMsg))
                {
                    throw new WarningException("未定义的消息");
                }
                logMsg.Log();
            }
            #region catch
            catch (JsonReaderException)
            {
                $"{logMsg}>未定义消息>{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}".Log(LeveType.Error);
                Publish(e.ApplicationMessage.Topic, new ErrorMessage(CommType.None, "未定义消息"));
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
                    Publish(e.ApplicationMessage.Topic, new ErrorMessage(type, error));
                }
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
