using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Model
{
    /// <summary>
    /// 已加载消息
    /// </summary>
    public class LoadMessage
    {
        /// <summary>
        /// 控件
        /// </summary>
        public DependencyObject Obj { get; set; }
    }
    /// <summary>
    /// 已加载消息
    /// </summary>
    public class OperateLoadMessage : LoadMessage { }
    /// <summary>
    /// 已加载消息
    /// </summary>
    public class LoginLoadMessage : LoadMessage { }
}
