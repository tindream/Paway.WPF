using Paway.Helper;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paway.WPF
{
    /// <summary>
    /// 窗体帮助类
    /// </summary>
    public static partial class WindowHelper
    {
        /// <summary>
        /// 消息提示
        /// </summary>
        public static void Hit(this string msg, FrameworkElement parent, LevelType level = LevelType.Info, int timeout = 3, double fontSize = 15)
        {
            // 判断当前是否在 UI 线程上
            if (Application.Current.Dispatcher.CheckAccess()) MessageWindow.Hit(parent, msg, level, timeout, fontSize);
            else PMethod.Invoke(() => MessageWindow.Hit(parent, msg, level, timeout, fontSize));
        }
    }
}