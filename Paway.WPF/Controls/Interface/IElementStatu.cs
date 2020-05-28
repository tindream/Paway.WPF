using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 扩展组件状态接口
    /// </summary>
    public interface IElementStatu<T>
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        T Normal { get; set; }
        /// <summary>
        /// 鼠标划过时的状态
        /// </summary>
        T Mouse { get; set; }
        /// <summary>
        /// 鼠标点击时的状态
        /// </summary>
        T Pressed { get; set; }
        /// <summary>
        /// 颜色Alpha值变量
        /// </summary>
        int Alpha { get; set; }

    }
}
