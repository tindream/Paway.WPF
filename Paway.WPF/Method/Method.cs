using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public class Method
    {
        #region 获取控件模板的XAML代码
        /// <summary>
        /// 获取控件模板的XAML代码
        /// </summary>
        public static string GetTemplateXaml(Control ctrl)
        {
            string xaml;
            if (ctrl.Template != null)
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = new string(' ', 4),
                    NewLineOnAttributes = true
                };
                var strbuild = new StringBuilder();
                var xmlwrite = XmlWriter.Create(strbuild, settings);
                try
                {
                    XamlWriter.Save(ctrl.Template, xmlwrite);
                    xaml = strbuild.ToString();
                }
                catch (Exception ex)
                {
                    xaml = ex.Message;
                }
            }
            else
            {
                xaml = "no template";
            }
            return xaml;
        }
        /// <summary>
        /// 从堆栈中获取上一个调用方法名称
        /// <para>属性Set方法去除set_前辍</para>
        /// </summary>
        public static string GetLastModelName(int index = 2)
        {
            //当前堆栈信息
            var sfs = new StackTrace().GetFrames();
            //方法名称
            //sfs[i].GetFileLineNumber();//没有PDB文件的情况下将始终返回0
            if (sfs.Length < index) return null;
            var name = sfs[index].GetMethod().Name;
            if (name.StartsWith("set_")) name = name.Remove(0, 4);
            return name;
        }

        #endregion

        #region 让系统可以处理队列中的所有Windows消息
        /// <summary>
        /// 让系统可以处理队列中的所有Windows消息
        /// </summary>
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        private static Object ExitFrame(Object state)
        {
            ((DispatcherFrame)state).Continue = false;
            return null;
        }

        #endregion

        #region Window系统消息框-Toast显示
        /// <summary>
        /// Window系统消息框-Toast显示
        /// </summary>
        public static void Toast(string msg, bool iError = false)
        {
            new WindowToast().Show(msg, iError);
        }

        #endregion

        #region Window系统消息框
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Information图标。</para>
        /// </summary>
        public static void Debug(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Debug);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Warning图标。</para>
        /// </summary>
        public static void Warning(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Warn);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Error图标。</para>
        /// </summary>
        public static void Error(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Error);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和指定图标。</para>
        /// </summary>
        public static void Show(DependencyObject parent, string msg, LeveType level = LeveType.Debug)
        {
            if (Parent(parent, out Window window))
            {
                switch (level)
                {
                    case LeveType.Debug:
                    default:
                        MessageBox.Show(window, msg, window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case LeveType.Warn:
                        MessageBox.Show(window, msg, window.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    case LeveType.Error:
                        MessageBox.Show(window, msg, window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OKCancel按钮和Question图标;它也会返回一个结果。</para>
        /// </summary>
        public static bool Ask(DependencyObject parent, string msg)
        {
            if (Parent(parent, out Window window))
            {
                return MessageBox.Show(window, msg, window.Title, MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
            }
            return false;
        }
        /// <summary>
        /// 返回控件的顶层Window
        /// </summary>
        public static Window Window(DependencyObject obj)
        {
            if (Parent(obj, out Window window)) return window;
            return null;
        }
        /// <summary>
        /// 返回控件的顶层指定类型控件
        /// </summary>
        public static bool Parent<T>(DependencyObject obj, out T parent) where T : FrameworkElement
        {
            while (obj != null)
            {
                if (obj is T)
                {
                    parent = (T)obj;
                    return true;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            parent = null;
            return false;
        }
        internal static bool Child<T>(DependencyObject obj, out T child) where T : FrameworkElement
        {
            var count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                var value = VisualTreeHelper.GetChild(obj, i);
                if (value is T)
                {
                    child = (T)value;
                    return true;
                }
                else
                {
                    if (Child<T>(value, out child))
                    {
                        return true;
                    }
                }
            }
            while (obj is ContentControl control)
            {
                obj = control.Content as DependencyObject;
                if (obj is T)
                {
                    child = (T)obj;
                    return true;
                }
            }
            child = null;
            return false;
        }

        #endregion
    }
}
