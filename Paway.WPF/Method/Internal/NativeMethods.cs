using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paway.WPF
{
    internal static class NativeMethods
    {
        /// <summary>
        /// 通过设置不同的标识符就可以获取系统分辨率、窗体显示区域的宽度和高度、滚动条的宽度和高度。
        /// </summary>
        /// <param name="nIndex">标识符</param>
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(SystemMessage nIndex);

        /// <summary>
        /// 检索当前双击鼠标的时间
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern int GetDoubleClickTime();
    }
}
