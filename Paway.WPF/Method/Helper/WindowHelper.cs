using Paway.Helper;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

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

        #region 统一Invoke处理
        /// <summary>
        /// 同步调用
        /// <para>任何与 Application 不在同一个线程的代码，都可能遭遇 Application.Current 为 null。如Shutdown关闭</para>
        /// </summary>
        public static bool Invoke(this DispatcherObject obj, Action action, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, error);
        }
        /// <summary>
        /// 带参数同步调用
        /// </summary>
        public static bool Invoke<T>(this DispatcherObject obj, Action<T> action, T t, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, t, error);
        }
        /// <summary>
        /// 同步调用，并返回结果
        /// </summary>
        public static T Invoke<T>(this DispatcherObject obj, Func<T> action, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, error);
        }
        /// <summary>
        /// 带参数同步调用，并返回结果
        /// </summary>
        public static O Invoke<T, O>(this DispatcherObject obj, Func<T, O> action, T t, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, t, error);
        }
        /// <summary>
        /// 异步调用
        /// </summary>
        public static DispatcherOperation BeginInvoke(this DispatcherObject obj, Action action, Action<Exception> error = null)
        {
            return PMethod.BeginInvoke(action, error);
        }
        /// <summary>
        /// 带参数异步调用
        /// </summary>
        public static DispatcherOperation BeginInvoke<T>(this DispatcherObject obj, Action<T> action, T t, Action<Exception> error = null)
        {
            return PMethod.BeginInvoke(action, t, error);
        }

        /// <summary>
        /// 同步调用
        /// <para>任何与 Application 不在同一个线程的代码，都可能遭遇 Application.Current 为 null。如Shutdown关闭</para>
        /// </summary>
        public static bool Invoke(this INotifyPropertyChanged obj, Action action, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, error);
        }
        /// <summary>
        /// 带参数同步调用
        /// </summary>
        public static bool Invoke<T>(this INotifyPropertyChanged obj, Action<T> action, T t, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, t, error);
        }
        /// <summary>
        /// 同步调用，并返回结果
        /// </summary>
        public static T Invoke<T>(this INotifyPropertyChanged obj, Func<T> action, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, error);
        }
        /// <summary>
        /// 带参数同步调用，并返回结果
        /// </summary>
        public static O Invoke<T, O>(this INotifyPropertyChanged obj, Func<T, O> action, T t, Action<Exception> error = null)
        {
            return PMethod.Invoke(action, t, error);
        }
        /// <summary>
        /// 异步调用
        /// </summary>
        public static DispatcherOperation BeginInvoke(this INotifyPropertyChanged obj, Action action, Action<Exception> error = null)
        {
            return PMethod.BeginInvoke(action, error);
        }
        /// <summary>
        /// 带参数异步调用
        /// </summary>
        public static DispatcherOperation BeginInvoke<T>(this INotifyPropertyChanged obj, Action<T> action, T t, Action<Exception> error = null)
        {
            return PMethod.BeginInvoke(action, t, error);
        }
        #endregion
        #region 返回指定控件的上下层控件
        /// <summary>
        /// 返回控件树中指定类型控件
        /// </summary>
        public static bool Find<T>(this DependencyObject dependency, out T parent, string name = null) where T : FrameworkElement
        {
            return PMethod.Find(dependency, out parent, name);
        }
        /// <summary>
        /// 返回控件的顶层指定类型控件
        /// </summary>
        public static bool Parent<T>(this DependencyObject dependency, out T parent, string name = null) where T : FrameworkElement
        {
            return PMethod.Parent(dependency, out parent, name);
        }
        /// <summary>
        /// 查找指定类型子(同级)控件
        /// </summary>
        /// <typeparam name="T">查找控件类型</typeparam>
        /// <param name="dependency">控件</param>
        /// <param name="child">返回指定类型控件</param>
        /// <param name="name">指定控件名称</param>
        /// <param name="func">外部条件，在多子项时判断</param>
        /// <returns></returns>
        public static bool Child<T>(this DependencyObject dependency, out T child, string name = null, Func<T, bool> func = null) where T : FrameworkElement
        {
            return PMethod.Child(dependency, out child, name, func);
        }
        /// <summary>
        /// 返回控件的顶层Window
        /// </summary>
        public static Window Window(this DependencyObject obj)
        {
            if (PMethod.Parent(obj, out Window window)) return window;
            return null;
        }

        #endregion
    }
}