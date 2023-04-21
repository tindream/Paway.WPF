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
using Paway.Model;

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
            //this.Proxy = WebRequest.DefaultWebProxy;
        }
        public WebClientPro(string user, string pad, int timeout = 30) : this(timeout)
        {
            var buffer = Encoding.UTF8.GetBytes($"{user}:{pad}");
            var result = Convert.ToBase64String(buffer);
            Headers[HttpRequestHeader.Authorization] = $"Basic {result}";
        }
        public WebClientPro(IUser user, int timeout = 30) : this(timeout)
        {
            if (user != null)
            {
                Headers[HttpRequestHeader.Authorization] = $"{user.Id}";
                Headers["Tag"] = $"{user.Tag()}";
            }
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

        #region 下载文件
        private Action<double> downPercentage;
        /// <summary>
        /// 下载文件
        /// </summary>
        public string DownFileAsync(string httpUrl, string toFile, string file, Action<double> percentage = null, Action completed = null)
        {
            if (percentage != null)
            {
                this.downPercentage = percentage;
                this.DownloadProgressChanged += WebClientPro_DownloadProgressChanged;
            }

            var url = $"{httpUrl}/{Config.UploadPath}/{toFile}?id={Config.User?.Id}";
            var path = Path.GetDirectoryName(file);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            string result = this.DownloadStringTaskAsync(url).Result.Decompress();
            completed();
            return result;
        }
        private void WebClientPro_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downPercentage(e.ProgressPercentage);
        }

        #endregion

        #region 上传文件
        private Action<double> upPercentage;
        /// <summary>
        /// 异步上传文件
        /// </summary>
        public string UpFileAsync(string httpUrl, string toFile, string file, double max = 0, Action<double> percentage = null)
        {
            if (percentage != null)
            {
                this.upPercentage = percentage;
                this.UploadProgressChanged += WebClientPro_UploadProgressChanged;
            }

            var url = $"{httpUrl}/{Config.UploadPath}/{toFile}?id={Config.User?.Id}";
            var str = Method.ReadFile(file, out int length);
            if (max != 0 && length > max * 1024 * 1024)
            {
                var desc = max >= 1 ? $"{max:0.#}M" : $"{max * 1024:F0}K";
                throw new WarningException($"上传附件不得大于{desc}");
            }
            return this.UploadStringTaskAsync(url, str.CompressBase64()).Result.Decompress();
        }
        private void WebClientPro_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            upPercentage(e.ProgressPercentage);
        }

        #endregion
    }
}
