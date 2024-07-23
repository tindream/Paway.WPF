using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Paway.WPF
{
    /// <summary>
    /// </summary>
    [ComImport, TypeLibType(TypeLibTypeFlags.FHidden), Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
    public interface DWebBrowserEvents2
    {
        /// <summary>
        /// </summary>
        [DispId(0x66)]
        void StatusTextChange([In] string text);
        /// <summary>
        /// </summary>
        [DispId(0x6c)]
        void ProgressChange([In] int progress, [In] int progressMax);
        /// <summary>
        /// </summary>
        [DispId(0x69)]
        void CommandStateChange([In] long command, [In] bool enable);
        /// <summary>
        /// </summary>
        [DispId(0x6a)]
        void DownloadBegin();
        /// <summary>
        /// </summary>
        [DispId(0x68)]
        void DownloadComplete();
        /// <summary>
        /// </summary>
        [DispId(0x71)]
        void TitleChange([In] string text);
        /// <summary>
        /// </summary>
        [DispId(0x70)]
        void PropertyChange([In] string szProperty);
        /// <summary>
        /// </summary>
        [DispId(250)]
        void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers, [In, Out] ref bool cancel);
        /// <summary>
        /// </summary>
        [DispId(0xfb)]
        void NewWindow2([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object pDisp, [In, Out] ref bool cancel);
        /// <summary>
        /// </summary>
        [DispId(0xfc)]
        void NavigateComplete2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
        /// <summary>
        /// </summary>
        [DispId(0x103)]
        void DocumentComplete([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL);
        /// <summary>
        /// </summary>
        [DispId(0xfd)]
        void OnQuit();
        /// <summary>
        /// </summary>
        [DispId(0xfe)]
        void OnVisible([In] bool visible);
        /// <summary>
        /// </summary>
        [DispId(0xff)]
        void OnToolBar([In] bool toolBar);
        /// <summary>
        /// </summary>
        /// <summary>
        /// </summary>
        [DispId(0x100)]
        void OnMenuBar([In] bool menuBar);
        /// <summary>
        /// </summary>
        [DispId(0x101)]
        void OnStatusBar([In] bool statusBar);
        /// <summary>
        /// </summary>
        [DispId(0x102)]
        void OnFullScreen([In] bool fullScreen);
        /// <summary>
        /// </summary>
        [DispId(260)]
        void OnTheaterMode([In] bool theaterMode);
        /// <summary>
        /// </summary>
        [DispId(0x106)]
        void WindowSetResizable([In] bool resizable);
        /// <summary>
        /// </summary>
        [DispId(0x108)]
        void WindowSetLeft([In] int left);
        /// <summary>
        /// </summary>
        [DispId(0x109)]
        void WindowSetTop([In] int top);
        /// <summary>
        /// </summary>
        [DispId(0x10a)]
        void WindowSetWidth([In] int width);
        /// <summary>
        /// </summary>
        [DispId(0x10b)]
        void WindowSetHeight([In] int height);
        /// <summary>
        /// </summary>
        [DispId(0x107)]
        void WindowClosing([In] bool isChildWindow, [In, Out] ref bool cancel);
        /// <summary>
        /// </summary>
        [DispId(0x10c)]
        void ClientToHostWindow([In, Out] ref long cx, [In, Out] ref long cy);
        /// <summary>
        /// </summary>
        [DispId(0x10d)]
        void SetSecureLockIcon([In] int secureLockIcon);
        /// <summary>
        /// </summary>
        [DispId(270)]
        void FileDownload([In, Out] ref bool cancel);
        /// <summary>
        /// </summary>
        [DispId(0x10f)]
        void NavigateError([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In, Out] ref bool cancel);
        /// <summary>
        /// </summary>
        [DispId(0xe1)]
        void PrintTemplateInstantiation([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp);
        /// <summary>
        /// </summary>
        [DispId(0xe2)]
        void PrintTemplateTeardown([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp);
        /// <summary>
        /// </summary>
        [DispId(0xe3)]
        void UpdatePageStatus([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In] ref object nPage, [In] ref object fDone);
        /// <summary>
        /// </summary>
        [DispId(0x110)]
        void PrivacyImpactedStateChange([In] bool bImpacted);
    }
}
