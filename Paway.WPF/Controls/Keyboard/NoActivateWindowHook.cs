using Paway.Helper;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Paway.WPF
{
    /// <summary>
    /// 阻止鼠标激活窗口的窗口过程钩子
    /// <para>虚拟键盘是独立窗口，点击时会抢走宿主的输入焦点。这里把窗口改为不激活样式，
    /// 并拦截 WM_MOUSEACTIVATE 返回 MA_NOACTIVATE：点击仍落到键盘上，焦点留在原输入框。</para>
    /// <para>构造时订阅 SourceInitialized（句柄此时才存在），调用方在窗口关闭时 Dispose 恢复原窗口过程。</para>
    /// </summary>
    internal sealed class NoActivateWindowHook : IDisposable
    {
        private const int GWL_EXSTYLE = -20;
        private const int GWLP_WNDPROC = -4;
        private const int MA_NOACTIVATE = 0x0003;

        private readonly Window window;
        /// <summary>
        /// 必须保持引用：委托被回收后，原生的回调会跳到已释放的存根
        /// </summary>
        private WndProcDelegate wndProc;
        private IntPtr originalProc;
        private bool installed;
        private IntPtr handle;

        internal NoActivateWindowHook(Window window)
        {
            this.window = window;
            window.SourceInitialized += Window_SourceInitialized;
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            this.handle = window.Handle();
            if (handle == IntPtr.Zero) return;
            //0x08000000 在扩展样式位上是 WS_EX_NOACTIVATE，配合 WS_EX_TOOLWINDOW 不占任务栏
            SetWindowLong32(handle, GWL_EXSTYLE, (int)(Helper.WindowStyle.WS_DISABLED | Helper.WindowStyle.WS_EX_TOOLWINDOW));

            wndProc = WndProc;
            originalProc = SetWindowProc(handle, Marshal.GetFunctionPointerForDelegate(wndProc));
            installed = originalProc != IntPtr.Zero;
        }
        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            //完全阻止鼠标激活窗口，其余消息交回原窗口过程
            if (msg == (int)WindowsMessage.WM_MOUSEACTIVATE) return (IntPtr)MA_NOACTIVATE;
            return CallWindowProc(originalProc, hWnd, msg, wParam, lParam);
        }

        /// <summary>
        /// 恢复原窗口过程并解除订阅
        /// </summary>
        public void Dispose()
        {
            window.SourceInitialized -= Window_SourceInitialized;
            if (!installed) return;
            installed = false;
            if (this.handle != IntPtr.Zero) SetWindowProc(this.handle, originalProc);
            originalProc = IntPtr.Zero;
            wndProc = null;
        }

        /// <summary>
        /// 窗口过程指针是机器字长度。x64 上必须走 SetWindowLongPtr：
        /// SetWindowLong 只写入 32 位，函数指针高位被截断后回调会跳到非法地址
        /// </summary>
        private static IntPtr SetWindowProc(IntPtr handle, IntPtr proc)
        {
            if (IntPtr.Size == 8) return SetWindowLongPtr64(handle, GWLP_WNDPROC, proc);
            return (IntPtr)SetWindowLong32(handle, GWLP_WNDPROC, proc.ToInt32());
        }

        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true)]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "CallWindowProcW", SetLastError = true)]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    }
}
