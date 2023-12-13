using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace WpfWebBrowser
{
    public partial class WebBrowserHelper
    {
        private readonly WebBrowser _webBrowser;
        private object _cookie;

        /// <summary>
        /// </summary>
        public event CancelEventHandler NewWindow;

        /// <summary>
        /// </summary>
        public WebBrowserHelper(WebBrowser webBrowser)
        {
            _webBrowser = webBrowser ?? throw new ArgumentNullException("webBrowser");
            _webBrowser.Dispatcher.BeginInvoke(new Action(Attach), DispatcherPriority.Loaded);
        }

        /// <summary>
        /// </summary>
        public void Disconnect()
        {
            if (_cookie != null)
            {
                _cookie.ReflectInvokeMethod("Disconnect", new Type[] { }, null);
                _cookie = null;
            }
        }

        private void Attach()
        {
            var axIWebBrowser2 = _webBrowser.ReflectGetProperty("AxIWebBrowser2");
            var webBrowserEvent = new WebBrowserEvent(this);
            var cookieType = typeof(WebBrowser).Assembly.GetType("MS.Internal.Controls.ConnectionPointCookie");
            _cookie = Activator.CreateInstance(
                cookieType,
                ReflectionService.BindingFlags,
                null,
                new[] { axIWebBrowser2, webBrowserEvent, typeof(DWebBrowserEvents2) },
                CultureInfo.CurrentUICulture);
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
