using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Paway.WPF
{
    /// <summary>
    /// Windows 消息列表
    /// </summary>
    internal enum WindowsMessage
    {
        /// <summary>
        /// Sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
        /// </summary>
        WM_NCACTIVATE = 0x0086,
    }

    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms724385(v=vs.85).aspx
    /// 检索指定的系统指标或系统配置设置。
    /// 请注意，GetSystemMetrics检索的所有尺寸均以像素为单位。
    /// </summary>
    internal enum SystemMessage
    {
        /// <summary>
        /// The amount of border padding for captioned windows, in pixels.
        /// Returns the amount of extra border padding around captioned windows
        /// Windows XP/2000:  This value is not supported.
        /// </summary>
        CXPADDEDBORDER = 0x5C,
    }
}
