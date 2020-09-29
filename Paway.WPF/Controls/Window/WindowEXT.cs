using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
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
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register(nameof(HeaderHeight), typeof(int), typeof(WindowEXT), new FrameworkPropertyMetadata(30));
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty ButtonMaxHeightProperty =
            DependencyProperty.Register(nameof(ButtonMaxHeight), typeof(double), typeof(WindowEXT), new FrameworkPropertyMetadata(32d));
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty ButtonAlignmentProperty =
            DependencyProperty.Register(nameof(ButtonAlignment), typeof(VerticalAlignment), typeof(WindowEXT), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));

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
        /// <summary>
        /// 标题栏高度
        /// </summary>
        [Category("扩展")]
        [Description("标题栏高度")]
        public int HeaderHeight
        {
            get { return (int)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }
        /// <summary>
        /// 按钮最大高度
        /// </summary>
        [Category("扩展")]
        [Description("按钮最大高度")]
        public double ButtonMaxHeight
        {
            get { return (double)GetValue(ButtonMaxHeightProperty); }
            set { SetValue(ButtonMaxHeightProperty, value); }
        }
        /// <summary>
        /// 按钮位置
        /// </summary>
        [Category("扩展")]
        [Description("按钮位置")]
        public VerticalAlignment ButtonAlignment
        {
            get { return (VerticalAlignment)GetValue(ButtonAlignmentProperty); }
            set { SetValue(ButtonAlignmentProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public WindowEXT()
        {
            DefaultStyleKey = typeof(WindowEXT);
            this.ContentRendered += WindowEXT_ContentRendered;
        }
        private void WindowEXT_ContentRendered(object sender, EventArgs e)
        {
            this.Activate();
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
            if (msg == (int)WindowsMessage.WM_NCACTIVATE) IsNonClientActive = wParam == (IntPtr)1;
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
