using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.WPF
{
    /// <summary>
    /// 组件装饰到Window接口
    /// </summary>
    public interface IWindowAdorner : IFrameworkInputElement
    {
        /// <summary>
        /// 关闭路由事件
        /// </summary>
        event EventHandler<RoutedEventArgs> CloseEvent;
    }
}
