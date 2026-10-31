using Paway.Helper;
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
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty HeaderHeightProperty =
            DependencyProperty.Register(nameof(HeaderHeight), typeof(int), typeof(WindowEXT), new FrameworkPropertyMetadata(30));
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register(nameof(ButtonWidth), typeof(int), typeof(WindowEXT), new FrameworkPropertyMetadata(45));
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register(nameof(ButtonHeight), typeof(int), typeof(WindowEXT), new FrameworkPropertyMetadata(30));
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty LogoSizeProperty =
            DependencyProperty.Register(nameof(LogoSize), typeof(int), typeof(WindowEXT), new FrameworkPropertyMetadata(24));
        /// <summary>
        /// </summary>
        private static readonly DependencyProperty ButtonAlignmentProperty =
            DependencyProperty.Register(nameof(ButtonAlignment), typeof(VerticalAlignment), typeof(WindowEXT), new FrameworkPropertyMetadata(VerticalAlignment.Stretch));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TextPaddingProperty =
            DependencyProperty.Register(nameof(TextPadding), typeof(Thickness), typeof(WindowEXT));

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
        /// <para>默认值：30</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题栏高度")]
        public int HeaderHeight
        {
            get { return (int)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }
        /// <summary>
        /// 标题栏按钮宽度
        /// <para>默认值：45</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题栏按钮宽度")]
        public int ButtonWidth
        {
            get { return (int)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }
        /// <summary>
        /// 标题栏按钮高度
        /// <para>默认值：42</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题栏按钮高度")]
        public int ButtonHeight
        {
            get { return (int)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }
        /// <summary>
        /// Logo大小
        /// <para>默认值：24</para>
        /// </summary>
        [Category("扩展")]
        [Description("Logo大小")]
        public int LogoSize
        {
            get { return (int)GetValue(LogoSizeProperty); }
            set { SetValue(LogoSizeProperty, value); }
        }
        /// <summary>
        /// 按钮位置
        /// <para>默认值：Stretch</para>
        /// </summary>
        [Category("扩展")]
        [Description("按钮位置")]
        public VerticalAlignment ButtonAlignment
        {
            get { return (VerticalAlignment)GetValue(ButtonAlignmentProperty); }
            set { SetValue(ButtonAlignmentProperty, value); }
        }
        /// <summary>
        /// 标题内边距
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题内边距")]
        public Thickness TextPadding
        {
            get { return (Thickness)GetValue(TextPaddingProperty); }
            set { SetValue(TextPaddingProperty, value); }
        }

        #endregion

        private bool _chromeFixed;

        /// <summary>
        /// Window扩展
        /// </summary>
        public WindowEXT()
        {
            DefaultStyleKey = typeof(WindowEXT);
            //this.ContentRendered += WindowEXT_ContentRendered;
            this.Loaded += WindowEXT_Loaded;
        }
        private void WindowEXT_Loaded(object sender, RoutedEventArgs e)
        {
            this.FixWindowChrome();
            if (ShowActivated) this.Activate();
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
            this.FixWindowChrome();
            IntPtr handle = this.Handle();
            HwndSource.FromHwnd(handle).AddHook(WndProc);
        }
        /// <summary>
        /// 修正 WindowChrome 占用的非客户区
        /// <para>WindowChrome 默认保留 ResizeBorderThickness（系统为 4px）与 CaptionHeight 两块非客户区，
        /// 这两块由系统接管，鼠标按下不会进入 WPF，表现为「窗口边缘按下无法拖动窗体」</para>
        /// <para>以下两种情况需要修正：不可调整大小的窗体不需要调整边框；没有标题栏的窗体不该占用顶部内容区</para>
        /// </summary>
        private void FixWindowChrome()
        {
            if (_chromeFixed) return;
            var chrome = WindowChrome.GetWindowChrome(this);
            if (chrome == null) return;
            var noResize = this.ResizeMode == ResizeMode.NoResize;
            var noHeader = this.HeaderHeight <= 0;
            if (!noResize && !noHeader)
            {
                _chromeFixed = true;
                return;
            }
            //WindowChrome 实例可能来自样式共享，改动会影响其它窗体，这里按窗体复制一份
            var copy = new WindowChrome
            {
                CaptionHeight = noHeader ? 0 : chrome.CaptionHeight,
                ResizeBorderThickness = noResize ? new Thickness(0) : chrome.ResizeBorderThickness,
                GlassFrameThickness = chrome.GlassFrameThickness,
                CornerRadius = chrome.CornerRadius,
                UseAeroCaptionButtons = chrome.UseAeroCaptionButtons,
                NonClientFrameEdges = chrome.NonClientFrameEdges,
            };
            WindowChrome.SetWindowChrome(this, copy);
            _chromeFixed = true;
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

        /// <summary>
        /// 判断按钮执行提交命令
        /// </summary>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (this.DataContext is IWindowModel windowModel)
                {
                    this.DialogResult = windowModel.OnCommit(this);
                    e.Handled = true;
                }
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
