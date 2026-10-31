using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        /// <summary>
        /// 窗体消息
        /// </summary>
        public MessageWindow()
        {
            InitializeComponent();
            this.SourceInitialized += MessageWindow_Initialized;
        }
        private void MessageWindow_Initialized(object sender, EventArgs e)
        {
            var hwnd = this.Handle();
            NativeMethods.SetWindowLong(hwnd, -16, unchecked((int)0x94000000));
            NativeMethods.SetWindowLong(hwnd, -20, (int)(Helper.WindowStyle.WS_DISABLED | Helper.WindowStyle.WS_EX_TOOLWINDOW | Helper.WindowStyle.WS_EX_NOHIT));
        }

        private static MessageWindow window;
        private IntPtr Handle;
        private FrameworkElement parentElement;
        private Window parentWindow;
        private readonly ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// 动画显示消息
        /// <para>同进程内点击窗体会获取焦点</para>
        /// </summary>
        public static MessageWindow Hit(FrameworkElement parent, string msg, LevelType level = LevelType.Info, int timeout = 3, double fontSize = 15, Action<TextBlock> action = null)
        {
            if (parent == null) return null;
            //关闭上一个提示：事件可能已随窗体关闭而释放，需容错
            var old = window;
            if (old != null)
            {
                try { old.manualResetEvent.Set(); }
                catch (ObjectDisposedException) { }
            }
            window = new MessageWindow();
            window.parentElement = parent;
            window.textBlock.Text = msg;
            window.textBlock.FontSize = fontSize;
            var type = ColorType.Color;
            switch (level)
            {
                case LevelType.Warn: type = ColorType.Warn; break;
                case LevelType.Error:
                case LevelType.Fatal: type = ColorType.Error; break;
            }
            window.textBlock.Foreground = Colors.White.ToBrush();
            window.border.Background = type.Color().ToAlpha(PConfig.Alpha).ToBrush();
            action?.Invoke(window.textBlock);
            window.LoadParent(parent, timeout);
            window.Show();
            window.textBlock.Margin = new Thickness(0);
            {
                var storyboard = PMethod.HitStoryboard(window, Colors.Transparent, 3000);
                storyboard.Begin(window, true);
            }
            return window;
        }
        /// <summary>
        /// 关闭提示窗体
        /// </summary>
        public static void HitClose()
        {
            try { window?.manualResetEvent.Set(); }
            catch (ObjectDisposedException) { }
        }
        /// <summary>
        /// 关闭时解除静态引用与事件订阅
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            //静态字段持有窗体与父元素：不清空会把已关闭的提示窗及其整棵可视树一直钉在内存里
            if (ReferenceEquals(window, this)) window = null;
            try { this.manualResetEvent.Set(); }
            catch (ObjectDisposedException) { }
            this.UnhookParent();
            this.parentElement = null;
            base.OnClosed(e);
        }
        private void LoadParent(FrameworkElement element, int timeout)
        {
            this.Handle = element.Handle();
            var point = element.PointToScreen(new Point(0, 0));
            this.Left = point.X + (element.ActualWidth - this.Width) / 2;
            this.Top = point.Y + (element.ActualHeight - this.Height) / 2;
            if (element.Parent(out Window parentWindow))
            {
                this.parentWindow = parentWindow;
                parentWindow.Closing += ParentWindow_Closing;
                parentWindow.LocationChanged += ParentWindow_LocationChanged;
                parentWindow.SizeChanged += ParentWindow_LocationChanged;
            }
            var taskHandle = Win32Helper.TaskHandle();
            Task.Run(() =>
            {
                try
                {
                    var iShow = true;
                    for (var i = 0; timeout == 0 || i < timeout * 25; i++)
                    {
                        IntPtr current = NativeMethods.GetForegroundWindow();
                        if (current != this.Handle && current != taskHandle)
                        {
                            if (iShow)
                            {
                                iShow = false;
                                this.BeginInvoke(() => { AnimationHelper.Start(this, TransitionType.Opacity, 0, 125, iReset: false); });
                            }
                        }
                        else
                        {
                            if (!iShow)
                            {
                                iShow = true;
                                this.BeginInvoke(() => { AnimationHelper.Start(this, TransitionType.Opacity, 1, 125, iReset: false); });
                            }
                        }
                        if (manualResetEvent.WaitOne(40)) break;
                    }
                }
                catch (ObjectDisposedException) { }//窗体已关闭，无需再处理
                finally
                {
                    this.Invoke(() => { this.PrevoewClose(); });
                    //ManualResetEvent 持有内核句柄，不释放则每次提示泄漏一个
                    manualResetEvent.Dispose();
                }
            });
        }
        private void PrevoewClose()
        {
            this.UnhookParent();
            this.Close();
        }
        /// <summary>
        /// 解除父窗体事件订阅
        /// </summary>
        private void UnhookParent()
        {
            var parentWindow = this.parentWindow;
            if (parentWindow == null) return;
            this.parentWindow = null;
            parentWindow.Closing -= ParentWindow_Closing;
            parentWindow.LocationChanged -= ParentWindow_LocationChanged;
            parentWindow.SizeChanged -= ParentWindow_LocationChanged;
        }
        private void ParentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.PrevoewClose();
        }
        private void ParentWindow_LocationChanged(object sender, EventArgs e)
        {
            if (parentElement == null) return;
            var point = parentElement.PointToScreen(new Point(0, 0));
            this.Left = point.X + (parentElement.ActualWidth - this.Width) / 2;
            this.Top = point.Y + (parentElement.ActualHeight - this.Height) / 2;
        }
    }
}
