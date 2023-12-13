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
    /// 按键消息
    /// </summary>
    public class KeyMessage
    {
        /// <summary>
        /// 按键
        /// </summary>
        public Key Key { get; }
        /// <summary>
        /// 已处理标记
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// 按键消息
        /// </summary>
        public KeyMessage() { }
        /// <summary>
        /// 按键消息
        /// </summary>
        public KeyMessage(Key key)
        {
            this.Key = key;
        }
    }
}
