using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace Paway.WPF
{
    /// <summary>
    /// Window扩展
    /// </summary>
    public class WindowEXT : Window
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty FunctionBarProperty =
            DependencyProperty.Register(nameof(FunctionBar), typeof(WindowFunctionBar), typeof(WindowEXT), new PropertyMetadata(default(WindowFunctionBar)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty FunctionBarRightProperty =
            DependencyProperty.Register(nameof(FunctionBarRight), typeof(WindowFunctionBar), typeof(WindowEXT), new PropertyMetadata(default(WindowFunctionBar)));
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty IsNonClientActiveProperty =
            DependencyProperty.Register(nameof(IsNonClientActive), typeof(bool), typeof(WindowEXT), new FrameworkPropertyMetadata(false));

        #endregion

        #region 扩展
        /// <summary>
        /// 左侧标题栏
        /// </summary>
        [Browsable(false)]
        public WindowFunctionBar FunctionBar
        {
            get => (WindowFunctionBar)GetValue(FunctionBarProperty);
            set => SetValue(FunctionBarProperty, value);
        }
        /// <summary>
        /// 右侧标题栏
        /// </summary>
        [Browsable(false)]
        public WindowFunctionBar FunctionBarRight
        {
            get => (WindowFunctionBar)GetValue(FunctionBarRightProperty);
            set => SetValue(FunctionBarRightProperty, value);
        }
        /// <summary>
        /// 窗体失去焦点时
        /// </summary>
        [Browsable(false)]
        public bool IsNonClientActive
        {
            get => (bool)GetValue(IsNonClientActiveProperty);
            set => SetValue(IsNonClientActiveProperty, value);
        }

        #endregion

        /// <summary>
        /// </summary>
        public WindowEXT()
        {
            DefaultStyleKey = typeof(WindowEXT);
        }

        /// <summary>
        /// </summary>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (SizeToContent == SizeToContent.WidthAndHeight && WindowChrome.GetWindowChrome(this) != null)
            {
                InvalidateMeasure();
            }
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WndProc));
        }
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowNotifications.WM_NCACTIVATE) IsNonClientActive = wParam == (IntPtr)1;
            return IntPtr.Zero;
        }
        /// <summary>
        /// </summary>
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            IsNonClientActive = true;
        }
        /// <summary>
        /// </summary>
        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            IsNonClientActive = false;
        }
    }
}
