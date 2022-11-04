using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Test
{
    /// <summary>
    /// 日志
    /// </summary>
    [Serializable]
    [Description("日志")]
    [Table("Logs")]
    public class LogInfo : ParentBaseOnce
    {
        [Text("时间")]
        public override DateTime CreateOn { get => base.CreateOn; set => base.CreateOn = value; }

        /// <summary>
        /// 类别
        /// </summary>
        [Text("类别")]
        public LogType Type { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Text("用户")]
        public string UserName { get; set; }

        /// <summary>
        /// 记录消息
        /// </summary>
        [Text("日志")]
        public string Message { get; set; }

        public LogInfo() { }
        public LogInfo(LogType type, string userName, string msg = null)
        {
            this.Type = type;
            this.UserName = userName;
            this.Message = msg;
        }
    }
}
