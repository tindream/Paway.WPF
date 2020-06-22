using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paway.WPF
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx
    /// Retrieves the specified system metric or system configuration setting.
    /// Note that all dimensions retrieved by GetSystemMetrics are in pixels.
    /// </summary>
    internal enum SM
    {
        /// <summary>
        /// The amount of border padding for captioned windows, in pixels.
        /// Returns the amount of extra border padding around captioned windows
        /// Windows XP/2000:  This value is not supported.
        /// </summary>
        CXPADDEDBORDER = 92,
    }

    internal static class NativeMethods
    {
        /// <summary>
        /// 通过设置不同的标识符就可以获取系统分辨率、窗体显示区域的宽度和高度、滚动条的宽度和高度。
        /// </summary>
        /// <param name="nIndex">标识符</param>
        [DllImport("user32.dll")]
        internal static extern int GetSystemMetrics(SM nIndex);

        /// <summary>
        /// 检索当前双击鼠标的时间
        /// </summary>
        [DllImport("user32.dll")]
        internal static extern int GetDoubleClickTime();
    }
}
