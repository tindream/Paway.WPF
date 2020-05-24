using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// <summary>
        /// 获取ITypeDescriptorContext中的属性值
        /// </summary>
        internal static T GetValue<T>(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                var service = (IProvideValueTarget)context.GetService(typeof(IProvideValueTarget));
                var objType = service.TargetObject.GetType();
                var obj = (DependencyObject)Activator.CreateInstance(objType);
                var property = (DependencyProperty)service.TargetProperty;
                return (T)obj.GetValue(property);
            }
            return default;
        }

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

        #region Window弹出
        /// <summary>
        /// 显示子窗体
        /// </summary>
        public static bool? Show(DependencyObject parent, Window window)
        {
            Parent(parent, out Window owner);
            window.ShowInTaskbar = false;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Closed += delegate
            {
                //容器Grid
                Grid grid = owner.Content as Grid;
                //父级窗体原来的内容
                var originalOld = VisualTreeHelper.GetChild(grid, 0) as UIElement;
                //将父级窗体原来的内容在容器Grid中移除
                grid.Children.Remove(originalOld);
                //赋给父级窗体
                owner.Content = originalOld;
            };
            //父级窗体原来的内容
            var original = owner.Content as UIElement;
            //将父级窗体原来的内容在容器Grid中移除
            owner.Content = null;
            //容器Grid
            var container = new Grid();
            //放入原来的内容
            container.Children.Add(original);
            //蒙板
            var layer = new Grid() { Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)) };
            //在上面放一层蒙板
            container.Children.Add(layer);
            //将装有原来内容和蒙板的容器赋给父级窗体
            owner.Content = container;
            //弹出消息框 
            window.Owner = owner;
            return window.ShowDialog();
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
                window.Dispatcher.BeginInvoke(new Action(() =>
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
                }));
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

        #endregion

        #region 返回指定控件的上下层控件
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
        /// <summary>
        /// 查找指定类型子(同级)控件
        /// </summary>
        /// <typeparam name="T">查找控件类型</typeparam>
        /// <param name="obj">控件</param>
        /// <param name="child">返回指定类型控件</param>
        /// <param name="name">指定控件名称</param>
        /// <param name="parent">指定搜索同级控件</param>
        /// <returns></returns>
        public static bool Child<T>(DependencyObject obj, out T child, string name = null, bool parent = true) where T : FrameworkElement
        {
            child = null;
            if (parent) obj = VisualTreeHelper.GetParent(obj);
            var count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                var value = VisualTreeHelper.GetChild(obj, i);
                if (value is T temp)
                {
                    if (name == null || temp.Name == name)
                    {
                        child = temp;
                        return true;
                    }
                }
                if (Child<T>(value, out child, name, false))
                {
                    return true;
                }
            }
            while (obj is ContentControl control)
            {
                obj = control.Content as DependencyObject;
                if (obj is T temp)
                {
                    if (name == null || temp.Name == name)
                    {
                        child = temp;
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
