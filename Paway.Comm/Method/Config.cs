using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Paway.Comm
{
    public class Config : Paway.Model.Config
    {
        #region 常量
        /// <summary>
        /// 根主题
        /// </summary>
        public const string Topic = "Tinn";
        /// <summary>
        /// 所有人信息
        /// </summary>
        public const string TopicAll = "All";
        /// <summary>
        /// 管理信息
        /// </summary>
        public const string TopicAdmin = "Admin";
        /// <summary>
        /// MQTT端口
        /// </summary>
        public const int MQPort = 9007;
        /// <summary>
        /// HTTP端口
        /// </summary>
        public const int HttpPort = 9008;
        /// <summary>
        /// 上传文件路径
        /// </summary>
        public static string UploadPath = "UploadFile";
        public static string Upload
        {
            get
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UploadPath);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }

        #endregion

        public static MQTTClientPlus MQClient { get; set; }
        /// <summary>
        /// 当前用户
        /// </summary>
        public static IUser User { get; set; }
    }
}
