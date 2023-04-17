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
using System.Web;
using System.Text.RegularExpressions;

namespace Paway.Comm
{
    /// <summary>
    /// 文件上传下载
    /// </summary>
    public partial class HttpServicePlus
    {
        private bool MessageHandleFile(HttpListenerContext context, string data, ref string logMsg)
        {
            if (context.Request.RawUrl.StartsWith("/" + Config.UploadPath, StringComparison.OrdinalIgnoreCase))
            {
                var url = HttpUtility.UrlDecode(context.Request.Url.LocalPath);
                var fileName = url.Substring(Config.UploadPath.Length + 2);
                var file = Path.Combine(Config.Upload, fileName);
                var path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                logMsg += $">{context.Request.HttpMethod}>{Path.GetFileName(fileName)}";
                switch (context.Request.HttpMethod)
                {
                    case "POST":
                        Method.SaveFile(file, data);
                        base.Response(context, "上传成功");
                        FileEvent?.Invoke(context);
                        break;
                    case "GET":
                        if (!File.Exists(file))
                        {
                            throw new WarningException($"文件不存在: {Path.GetFileName(fileName)}");
                        }
                        else
                        {
                            base.Response(context, Method.ReadFile(file, out _));
                            FileEvent?.Invoke(context);
                        }
                        break;
                }
                return true;
            }
            return false;
        }
    }
}
