using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Model
{
    /// <summary>
    /// 状态基础消息
    /// </summary>
    public class LevelMessage
    {
        /// <summary>
        /// 消息体
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 消息等级
        /// </summary>
        public LevelType Level { get; set; }

        /// <summary>
        /// 状态基础消息
        /// </summary>
        public LevelMessage() { }
        /// <summary>
        /// 状态基础消息
        /// </summary>
        public LevelMessage(string msg)
        {
            this.Msg = msg;
            this.Level = LevelType.Debug;
        }
        /// <summary>
        /// 状态基础消息
        /// </summary>
        public LevelMessage(string msg, LevelType level)
        {
            this.Msg = msg;
            this.Level = level;
        }
    }
    /// <summary>
    /// 状态消息
    /// </summary>
    public class StatuMessage : LevelMessage
    {
        /// <summary>
        /// 弹出提示
        /// <para>默认不弹出</para>
        /// </summary>
        public bool IHit { get; set; }
        /// <summary>
        /// 关联控件
        /// </summary>
        public DependencyObject Ower { get; set; }
        /// <summary>
        /// 状态消息，指定消息、弹出标记
        /// </summary>
        public StatuMessage(string msg, bool iHit = false) : base(msg)
        {
            this.IHit = iHit;
        }
        /// <summary>
        /// 状态消息，指定消息、父级控件
        /// </summary>
        public StatuMessage(string msg, DependencyObject ower) : base(msg)
        {
            this.Ower = ower;
            this.IHit = ower != null;
        }
        /// <summary>
        /// 状态消息，指定消息、消息等级、父级控件
        /// </summary>
        public StatuMessage(string msg, LevelType level, DependencyObject ower = null) : base(msg, level)
        {
            this.Ower = ower;
            this.IHit = ower != null || level > LevelType.Debug;
        }
        /// <summary>
        /// 状态消息，指定异常消息、父级控件
        /// </summary>
        public StatuMessage(Exception ex, DependencyObject ower = null) : this(null, ex)
        {
            this.Ower = ower;
        }
        /// <summary>
        /// 状态消息
        /// </summary>
        public StatuMessage(string title, Exception ex, DependencyObject ower = null)
        {
            this.IHit = true;
            ex.Log();
            var msg = $"{ex.Message().Replace(Environment.NewLine, "。")}";
            if (!title.IsEmpty()) msg = $"{title}: {msg}";
            this.Msg = msg;
            this.Level = ex.IExist(typeof(WarningException)) ? LevelType.Warn : LevelType.Error;
            this.Ower = ower;
        }
    }
}
