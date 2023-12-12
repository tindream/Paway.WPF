using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯相关的一些常量，与状态日志事件
    /// </summary>
    public class CConfig : TConfig
    {
        #region 常量
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
        /// <summary>
        /// 上传文件完整路径
        /// </summary>
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
        #region MQTT通讯常量
        /// <summary>
        /// 根主题
        /// </summary>
        public const string Topic = Name;
        /// <summary>
        /// 所有人信息
        /// </summary>
        public const string TopicAll = All;
        /// <summary>
        /// 管理信息
        /// </summary>
        public const string TopicAdmin = "Admin";

        #endregion

        /// <summary>
        /// MQTT全局客户端
        /// </summary>
        public static MQTTClientPlus MQClient { get; set; }
        /// <summary>
        /// 当前用户
        /// </summary>
        public static IUser User { get; set; }

        #region 状态日志
        /// <summary>
        /// 状态日志事件
        /// </summary>
        public static event Action<string, LeveType> StatuLogEvent;
        /// <summary>
        /// 添加状态日志
        /// </summary>
        public static void AddStatuLog(string msg, LeveType level = LeveType.Debug)
        {
            StatuLogEvent?.Invoke(msg, level);
        }

        #endregion
    }
}
