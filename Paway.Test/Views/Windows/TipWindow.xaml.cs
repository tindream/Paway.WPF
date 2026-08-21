using CommunityToolkit.Mvvm.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TipWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TipWindow : WindowEXT
    {
        private bool iLoad;
        private bool iClose;
        public TipWindow()
        {
            InitializeComponent();
            this.SourceInitialized += MessageWindow_Initialized;
            this.Loaded += TipWindow_Loaded;
        }
        /// <summary>
        /// 移除系统菜单
        /// </summary>
        private void TipWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var menu = NativeMethods.GetSystemMenu(this.Handle(), false);
            NativeMethods.DeleteMenu(menu, (int)Paway.Helper.WindowStyle.SC_RESTORE, 0);
            NativeMethods.DeleteMenu(menu, (int)Paway.Helper.WindowStyle.SC_MINIMIZE, 0);
            NativeMethods.DeleteMenu(menu, (int)Paway.Helper.WindowStyle.SC_MAXIMIZE, 0);
        }
        /// <summary>
        /// 移除Tab切换窗口
        /// </summary>
        private void MessageWindow_Initialized(object sender, EventArgs e)
        {
            var hwnd = this.Handle();
            NativeMethods.SetWindowLong(hwnd, -20, (int)Paway.Helper.WindowStyle.WS_EX_TOOLWINDOW);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModelLocator.Default.TipWindow.IAll = false;
            e.Cancel = !iClose;
            base.OnClosing(e);
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (!this.iLoad)
            {
                this.iLoad = true;
                PConfig.Window.LocationChanged += Window_LocationChanged;
                PConfig.Window.SizeChanged += Window_LocationChanged;
                this.AutoHide();
                Window_LocationChanged2(false);
                WeakReferenceMessenger.Default.Send(new TipLoadMessage() { Obj = this });
                WeakReferenceMessenger.Default.Register<TipStateMessage>(this, (obj, msg) =>
                {
                    Window_LocationChanged2(true);
                });
                WeakReferenceMessenger.Default.Register<TipCloseMessage>(this, (obj, msg) =>
                {
                    this.iClose = true;
                    this.Close();
                });
            }
        }
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            Window_LocationChanged2(false);
        }
        private void Window_LocationChanged2(bool animation)
        {
            var point = PConfig.Window.PointToScreen(new Point(0, 0));
            this.MaxWidth = PConfig.Window.ActualWidth - 100;
            this.MaxHeight = PConfig.Window.ActualHeight - 100;

            var iAll = ViewModelLocator.Default.TipWindow.IAll;
            var left = point.X + PConfig.Window.ActualWidth - (iAll ? this.ActualWidth : (this.ActualWidth + 80) / 2) - 50;
            var top = point.Y + PConfig.Window.ActualHeight - (iAll ? this.ActualHeight : (this.ActualHeight + 80) / 2) - 50;
            var time = 125;
            if (this.Left != left)
            {
                if (animation) AnimationHelper.Start(this, Window.LeftProperty, this.Left, left, time);
                else this.Left = left;
            }
            if (this.Top != top)
            {
                if (animation) AnimationHelper.Start(this, Window.TopProperty, this.Top, top, time);
                else this.Top = top;
            }
        }
        private void AutoHide()
        {
            var handle = this.Handle();
            var taskHandle = Win32Helper.TaskHandle();
            var currentProcessId = Process.GetCurrentProcess().Id;
            Task.Run(() =>
            {
                var iShow = true;
                while (!Config.IClose)
                {
                    IntPtr current = NativeMethods.GetForegroundWindow();
                    var iCurrent = current == handle || current == Config.Handle || current == taskHandle;
                    if (!iCurrent)
                    {
                        NativeMethods.GetWindowThreadProcessId(current, out uint windowProcessId);
                        iCurrent = windowProcessId == currentProcessId;
                    }
                    if (!iCurrent)
                    {
                        if (iShow)
                        {
                            iShow = false;
                            PMethod.BeginInvoke(() => { AnimationHelper.Start(this, TransitionType.Opacity, 0, 125, iReset: false); });
                        }
                    }
                    else if (!iShow)
                    {
                        iShow = true;
                        PMethod.BeginInvoke(() => { AnimationHelper.Start(this, TransitionType.Opacity, 1, 125, iReset: false); });
                    }
                    Thread.Sleep(100);
                }
            });
        }
    }
}
