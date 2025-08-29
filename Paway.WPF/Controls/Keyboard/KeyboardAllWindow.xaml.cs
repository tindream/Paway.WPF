using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
        /// <summary>
        /// 焦点问题
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            this.element?.Focus();
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
