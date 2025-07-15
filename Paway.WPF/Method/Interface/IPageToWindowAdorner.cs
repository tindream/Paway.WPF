using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.WPF
{
    /// <summary>
    /// Page页装饰到Window接口
    /// </summary>
    public interface IPageToWindowAdorner : IFrameworkInputElement
    {
        /// <summary>
        /// 关闭路由事件
        /// </summary>
        event EventHandler<RoutedEventArgs> CloseEvent;
    }
}
