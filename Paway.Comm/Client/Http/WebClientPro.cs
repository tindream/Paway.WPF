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

namespace Paway.Comm
{
    public class WebClientPro : WebClient
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 默认60s
        /// </summary>
        public WebClientPro(int timeout = 30)
        {
            Timeout = timeout * 1000;
            Encoding = Encoding.UTF8;
            Headers[HttpRequestHeader.ContentType] = "application/json";
        }
        public WebClientPro(string user, string pad, int timeout = 30) : this(timeout)
        {
            var buffer = Encoding.UTF8.GetBytes($"{user}:{pad}");
            var result = Convert.ToBase64String(buffer);
            Headers[HttpRequestHeader.Authorization] = $"Basic {result}";
        }

        /// <summary>
        /// 重写GetWebRequest,添加WebRequest对象超时时间
        /// </summary>
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }

        #region 扩展方法
        /// <summary>
        /// 下载用户附件
        /// </summary>
        public void DownFile(string httpUrl, string toPath, string file)
        {
            var url = $"{httpUrl}/{Config.UploadPath}/{toPath}?id={Config.User?.Id}";
            var path = Path.GetDirectoryName(file);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            var str = this.DownloadString(url).Decompress();
            Method.SaveFile(file, str);
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        public string UpFile(string httpUrl, string toPath, string file, double max)
        {
            var url = $"{httpUrl}/{Config.UploadPath}/{toPath}?id={Config.User?.Id}";
            var str = Method.ReadFile(file, out int length);
            if (length > max * 1024 * 1024)
            {
                var desc = max >= 1 ? $"{max:0.#}M" : $"{max * 1024:F0}K";
                throw new WarningException($"上传附件不得大于{desc}");
            }
            var result = this.UploadString(url, str.CompressBase64()).Decompress();
            return result;
        }

        #endregion
    }
}
