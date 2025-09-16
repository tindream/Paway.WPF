using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// KeyboardAllWindow.xaml 的交互逻辑
    /// </summary>
    public partial class KeyboardAllWindow : WindowEXT
    {
        private bool _firstRender;
        private readonly FrameworkElement element;
        /// <summary>
        /// 关闭事件
        /// </summary>
        public event EventHandler<RoutedEventArgs> CloseEvent;
        /// <summary>
        /// 虚拟键盘-全键盘
        /// </summary>
        public KeyboardAllWindow() : this(null, true) { }
        /// <summary>
        /// 位置
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (element == null && !_firstRender)
            {
                _firstRender = true;
                // 获取主屏幕的工作区大小（不包括任务栏）
                this.Left = (SystemParameters.WorkArea.Width - this.ActualWidth) / 2;
                this.Top = SystemParameters.WorkArea.Height - this.ActualHeight;
            }
        }
        /// <summary>
        /// 虚拟键盘-全键盘
        /// </summary>
        public KeyboardAllWindow(FrameworkElement element, bool iTitle = false, bool iKeyboardNum = false)
        {
            InitializeComponent();
            this.element = element;
            this.dpTitle.Visibility = iTitle ? Visibility.Visible : Visibility.Collapsed;
            this.Height = iTitle ? 280 : 246;
            this.SourceInitialized += KeyboardAllWindow_SourceInitialized;
            keyboardAll.IKeyboardNum(iKeyboardNum);
            keyboardAll.CloseEvent += KeyboardAll_CloseEvent;
            keyboardAll.DragMovedEvent += KeyboardAll_DragMovedEvent;
            btnClose.Click += KeyboardAll_CloseEvent;
        }
        private void KeyboardAllWindow_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = this.Handle();
            NativeMethods.SetWindowLong(hwnd, -20, (int)(Helper.WindowStyle.WS_DISABLED | Helper.WindowStyle.WS_EX_TOOLWINDOW));

            // 创建并保持委托引用
            _wndProcDelegate = new WndProcDelegate(WndProc);
            // 替换窗口过程
            _originalWndProc = NativeMethods.SetWindowLong(hwnd, -4, Marshal.GetFunctionPointerForDelegate(_wndProcDelegate));
        }
        #region 阻止鼠标激活窗口
        private const int MA_NOACTIVATE = 0x0003;
        private IntPtr _originalWndProc;
        private WndProcDelegate _wndProcDelegate;
        // 定义窗口过程委托
        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == (int)WindowsMessage.WM_MOUSEACTIVATE)
            {
                // 完全阻止鼠标激活窗口
                return (IntPtr)MA_NOACTIVATE;
            }
            // 调用原始窗口过程处理其他消息
            return CallWindowProc(_originalWndProc, hWnd, msg, wParam, lParam);
        }
        /// <summary>
        /// 恢复原始窗口过程
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            if (_originalWndProc != IntPtr.Zero)
            {
                NativeMethods.SetWindowLong(this.Handle(), -4, _originalWndProc);
            }
            // 释放委托引用
            _wndProcDelegate = null;
            base.OnClosed(e);
        }

        #endregion

        /// <summary>
        /// 焦点问题
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            //this.element?.Focus();
        }
        private void KeyboardAll_DragMovedEvent(object sender, MouseButtonEventArgs e)
        {
            this.element?.Focus();
        }
        private void KeyboardAll_CloseEvent(object sender, RoutedEventArgs e)
        {
            if (CloseEvent != null) CloseEvent.Invoke(sender, e);
            else this.Close();
        }
    }
}
