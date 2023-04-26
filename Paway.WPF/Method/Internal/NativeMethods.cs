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
        /// 获取当前线程的一个伪句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetCurrentThread();

        /// <summary>
        /// 获取线程执行的周期个数。
        /// </summary>
        [DllImport("kernel32.dll")]
        internal static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        /// <summary>
        /// 该函数删除一个逻辑笔、画笔、字体、位图、区域或者调色板，释放所有与该对象有关的系统资源，在对象被删除之后，指定的句柄也就失效了。
        /// </summary>
        [DllImport("gdi32.dll", SetLastError = true)]
        internal static extern bool DeleteObject(IntPtr hObject);

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
