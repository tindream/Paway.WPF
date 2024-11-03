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
            NativeMethods.SetWindowLong(hwnd, -20, 0x08000088);
        }

        private static MessageWindow window;
        private IntPtr Handle;
        private FrameworkElement parentElement;
        private Window parentWindow;
        private ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// 动画显示消息
        /// <para>同进程内点击窗体会获取焦点</para>
        /// </summary>
        public static MessageWindow Hit(FrameworkElement element, string msg, LevelType level = LevelType.Info, int timeout = 3, double fontSize = 15, Action<TextBlock> action = null)
        {
            if (window == null) window = new MessageWindow();
            else
            {
                window.manualResetEvent.Set();
                window = new MessageWindow();
            }
            window.parentElement = element;
            window.textBlock.Text = msg;
            window.textBlock.FontSize = fontSize;
            var type = ColorType.Color;
            switch (level)
            {
                case LevelType.Warn: type = ColorType.Warn; break;
                case LevelType.Error: type = ColorType.Error; break;
            }
            window.textBlock.Foreground = Colors.White.ToBrush();
            window.border.Background = type.Color().ToAlpha(PConfig.Alpha).ToBrush();
            action?.Invoke(window.textBlock);
            window.LoadParent(element, timeout);
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
            window?.manualResetEvent.Set();
        }
        private void LoadParent(FrameworkElement element, int timeout)
        {
            this.Handle = element.Handle();
            var point = element.PointToScreen(new Point(0, 0));
            this.Left = point.X + (element.ActualWidth - this.Width) / 2;
            this.Top = point.Y + (element.ActualHeight - this.Height) / 2;
            if (PMethod.Parent(element, out Window parentWindow))
            {
                this.parentWindow = parentWindow;
                this.textBlock.MouseDown += TextBlock_MouseDown;
                parentWindow.Closing += ParentWindow_Closing;
                parentWindow.LocationChanged += ParentWindow_LocationChanged;
                parentWindow.SizeChanged += ParentWindow_LocationChanged;
            }
            Task.Run(() =>
            {
                var iShow = true;
                for (var i = 0; timeout == 0 || i < timeout * 25; i++)
                {
                    IntPtr current = NativeMethods.GetForegroundWindow();
                    if (current != this.Handle)
                    {
                        if (iShow)
                        {
                            iShow = false;
                            PMethod.BeginInvoke(() => { AnimationHelper.Start(this, TransitionType.Opacity, 0, 125, iReset: false); });
                        }
                    }
                    else
                    {
                        if (!iShow)
                        {
                            iShow = true;
                            PMethod.BeginInvoke(() => { AnimationHelper.Start(this, TransitionType.Opacity, 1, 125, iReset: false); });
                        }
                    }
                    if (manualResetEvent.WaitOne(40)) break;
                }
                PMethod.Invoke(() => { this.PrevoewClose(); });
            });
        }
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            parentWindow.Focus();
        }
        private void PrevoewClose()
        {
            parentWindow.Closing -= ParentWindow_Closing;
            parentWindow.LocationChanged -= ParentWindow_LocationChanged;
            parentWindow.SizeChanged -= ParentWindow_LocationChanged;
            this.Close();
        }
        private void ParentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.PrevoewClose();
        }
        private void ParentWindow_LocationChanged(object sender, EventArgs e)
        {
            var point = parentElement.PointToScreen(new Point(0, 0));
            this.Left = point.X + (parentElement.ActualWidth - this.Width) / 2;
            this.Top = point.Y + (parentElement.ActualHeight - this.Height) / 2;
        }
    }
}
