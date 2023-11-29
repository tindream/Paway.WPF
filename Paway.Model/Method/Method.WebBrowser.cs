using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace Paway.Model
{
    public partial class Method
    {
        /// <summary>
        /// 过滤WebBrowser javascript错误提示
        /// <para>这是一个C＃例程，能够将WPF的WebBrowser置于静默模式。</para>
        /// <para>您不能在WebBrowser初始化时调用它，因为它为时过早，而是在导航发生之后调用。</para>
        /// <para></para>
        /// </summary>
        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            if (browser.Document is IOleServiceProvider sp)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out object webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }
    }
}
