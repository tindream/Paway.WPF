using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace Paway.WPF
{
    public partial class WebBrowserHelper : IDisposable
    {
        private readonly WebBrowser _webBrowser;
        private object _cookie;
        private int attachCount;

        /// <summary>
        /// </summary>
        public event CancelEventHandler NewWindow;

        /// <summary>
        /// </summary>
        public WebBrowserHelper(WebBrowser webBrowser)
        {
            _webBrowser = webBrowser ?? throw new ArgumentNullException(nameof(webBrowser));
            //控件卸载时解绑COM连接点：ConnectionPointCookie 强引用 WebBrowser/WebBrowserHelper，
            //不释放则整棵含浏览器的可视树永不回收，且回调会继续打到已卸载的控件
            _webBrowser.Unloaded += WebBrowser_Unloaded;
            _webBrowser.Dispatcher.BeginInvoke(new Action(Attach), DispatcherPriority.Loaded);
        }

        private void WebBrowser_Unloaded(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }

        /// <summary>
        /// </summary>
        public void Disconnect()
        {
            var cookie = _cookie;
            if (cookie != null)
            {
                _cookie = null;//先置空，保证可重入安全
                try
                {
                    ReflectionService.ReflectInvokeMethod(cookie, "Disconnect", new Type[] { }, null);
                }
                catch (Exception ex)
                {
                    ex.Log("解除WebBrowser事件连接失败");
                }
            }
            if (_webBrowser != null) _webBrowser.Unloaded -= WebBrowser_Unloaded;
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }

        private void Attach()
        {
            if (_cookie != null) return;
            if (attachCount++ > 20)
            {
                "WebBrowser 事件连接失败：AxIWebBrowser2 始终未就绪".Warn();
                return;
            }
            try
            {
                var axIWebBrowser2 = ReflectionService.ReflectGetProperty(_webBrowser, "AxIWebBrowser2");
                //控件尚未加载完成时取不到，稍后重试；原实现把 null 直接传给 ConnectionPointCookie 构造函数，
                //异常抛在 Dispatcher 回调里等于未处理异常
                if (axIWebBrowser2 == null)
                {
                    _webBrowser.Dispatcher.BeginInvoke(new Action(Attach), DispatcherPriority.Background);
                    return;
                }
                var webBrowserEvent = new WebBrowserEvent(this);
                var cookieType = typeof(WebBrowser).Assembly.GetType("MS.Internal.Controls.ConnectionPointCookie");
                if (cookieType == null)
                {
                    "WebBrowser 事件连接失败：找不到 ConnectionPointCookie 类型".Warn();
                    return;
                }
                _cookie = Activator.CreateInstance(
                    cookieType,
                    ReflectionService.BindingFlags,
                    null,
                    new[] { axIWebBrowser2, webBrowserEvent, typeof(IDWebBrowserEvents2) },
                    CultureInfo.CurrentUICulture);
            }
            catch (Exception ex)
            {
                ex.Log("绑定WebBrowser事件失败");
            }
        }

        private void OnNewWindow(ref bool cancel)
        {
            var eventArgs = new CancelEventArgs(cancel);
            if (NewWindow != null)
            {
                NewWindow(_webBrowser, eventArgs);
                cancel = eventArgs.Cancel;
            }
        }
    }
}
