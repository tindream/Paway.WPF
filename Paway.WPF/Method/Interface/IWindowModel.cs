using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// Window模型接口
    /// </summary>
    public interface IWindowModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// 关闭处理
        /// </summary>
        bool? OnCancel(Window wd);
        /// <summary>
        /// 提交处理
        /// </summary>
        bool? OnCommit(Window wd);
    }
}
