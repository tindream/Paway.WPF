using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    /// <summary>
    /// 连接数量消息
    /// </summary>
    public class ConnectCountMessage
    {
        /// <summary>
        /// 连接数量消息
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 连接数量消息
        /// </summary>
        public ConnectCountMessage() { }
        /// <summary>
        /// 连接数量消息
        /// </summary>
        public ConnectCountMessage(int count)
        {
            this.Count = count;
        }
    }
    /// <summary>
    /// 连接数量2消息
    /// </summary>
    public class Connect2CountMessage : ConnectCountMessage
    {
        /// <summary>
        /// 连接数量2消息
        /// </summary>
        public Connect2CountMessage() { }
        /// <summary>
        /// 连接数量2消息
        /// </summary>
        public Connect2CountMessage(int count) : base(count) { }
    }
}
