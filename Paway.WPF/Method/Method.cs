using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        #region internal
        #region TypeConverter
        /// <summary>
        /// 获取ITypeDescriptorContext中的属性值
        /// </summary>
        internal static T GetValue<T>(ITypeDescriptorContext context)
        {
            if (context != null)
            {
                var service = (IProvideValueTarget)context.GetService(typeof(IProvideValueTarget));
                if (service.TargetObject != null)
                {
                    var objType = service.TargetObject.GetType();
                    var obj = (DependencyObject)Activator.CreateInstance(objType);
                    var property = (DependencyProperty)service.TargetProperty;
                    return (T)obj.GetValue(property);
                }
            }
            return default;
        }
        /// <summary>
        /// 控件状态转换
        /// </summary>
        internal static Tuple<T, I?, I?, I?, int?> ElementStatu<T, I>(ITypeDescriptorContext context, CultureInfo culture, string str,
            Func<string, I> func, Func<T, string, I?> funcValue)
            //where T : IElementStatu<I> 
            where I : struct
        {
            var old = Method.GetValue<T>(context);

            var strs = str.Split(';');
            I? normal = null;
            I? mouse = null;
            I? pressed = null;
            int? alpha = null;
            if (strs.Length > 0)
            {
                if (!string.IsNullOrEmpty(strs[0])) normal = func(strs[0]);
                else normal = funcValue(old, "Normal");
            }
            if (strs.Length > 1)
            {
                if (!string.IsNullOrEmpty(strs[1])) mouse = func(strs[1]);
                else mouse = funcValue(old, "Mouse");
            }
            if (strs.Length > 2)
            {
                if (!string.IsNullOrEmpty(strs[2])) pressed = func(strs[2]);
                else pressed = funcValue(old, "Pressed");
            }
            if (strs.Length > 3)
            {
                if (!string.IsNullOrEmpty(strs[3])) alpha = Convert.ToInt32(strs[3], culture);
                else if (old != null) alpha = (int)old.GetValue("Alpha");
            }
            return new Tuple<T, I?, I?, I?, int?>(old, normal, mouse, pressed, alpha);
        }

        #endregion

        #endregion

        #region 获取对象指定名称字段、执行对象方法
        /// <summary>
        /// 获取对象指定名称字段
        /// </summary>
        public static bool GetField<T>(object parent, string name, out T value)
        {
            return GetField(parent, parent.GetType(), name, out value);
        }
        /// <summary>
        /// 获取对象指定名称字段
        /// </summary>
        private static bool GetField<T>(object parent, Type type, string name, out T value)
        {
            value = default;
            var field = type.GetField(name, TConfig.Flags | BindingFlags.Instance);
            if (field == null && type.BaseType != null) return GetField(parent, type.BaseType, name, out value);
            if (field != null && field.GetValue(parent) is T t)
            {
                value = t;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 执行对象方法
        /// </summary>
        public static bool ExecuteMethod(object obj, string name, params object[] args)
        {
            return ExecuteMethod(obj, obj.GetType(), name, args);
        }
        /// <summary>
        /// 执行对象方法
        /// </summary>
        private static bool ExecuteMethod(object obj, Type type, string name, params object[] args)
        {
            var method = obj.GetType().GetMethod(name, TConfig.Flags | BindingFlags.Instance);
            if (method == null && type.BaseType != null) return ExecuteMethod(obj, type.BaseType, name, args);
            if (method != null)
            {
                method.Invoke(obj, args);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 执行对象方法
        /// </summary>
        public static bool ExecuteMethod<T>(object obj, string name, out T result, params object[] args)
        {
            return ExecuteMethod(obj, obj.GetType(), name, out result, args);
        }
        /// <summary>
        /// 执行对象方法
        /// </summary>
        private static bool ExecuteMethod<T>(object obj, Type type, string name, out T result, params object[] args)
        {
            result = default;
            var method = obj.GetType().GetMethod(name, TConfig.Flags | BindingFlags.Instance);
            if (method == null && type.BaseType != null) return ExecuteMethod(obj, type.BaseType, name, out result, args);
            if (method != null)
            {
                result = (T)method.Invoke(obj, args);
                return true;
            }
            return false;
        }

        #endregion

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

        #region Window进度条
        private static WindowProgress progress;
        /// <summary>
        /// 模式显示Window进度条，执行完成后关闭
        /// </summary>
        public static void Progress(DependencyObject parent, Action action, Action completed = null)
        {
            parent.Dispatcher.BeginInvoke(new Action(() =>
            {
                new Action(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ex.Log();
                        parent.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Method.Error(parent, ex.Message());
                        }));
                    }
                }).BeginInvoke(new AsyncCallback(ar =>
                {
                    ProgressCompleted(parent, completed);
                }), null);
                Progress(parent, true);
            }));
        }
        /// <summary>
        /// 同步步模式显示Window进度条，执行完成后关闭
        /// </summary>
        public static void ProgressSync(DependencyObject parent, Action action, Action completed = null)
        {
            parent.Dispatcher.Invoke(() =>
            {
                new Action(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ex.Log();
                        parent.Dispatcher.Invoke(() =>
                        {
                            Method.Error(parent, ex.Message());
                        });
                    }
                }).BeginInvoke(new AsyncCallback(ar =>
                {
                    ProgressCompleted(parent, completed);
                }), null);
                Progress(parent, true);
            });
        }
        private static void ProgressCompleted(DependencyObject parent, Action completed = null)
        {
            parent.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    Method.Hide();
                    completed?.Invoke();
                }
                catch (Exception ex)
                {
                    ex.Log();
                    Method.Error(parent, ex.Message());
                }
            }));
        }
        /// <summary>
        /// 同步显示Window进度条
        /// </summary>
        public static void Progress(DependencyObject parent = null, bool dialog = false)
        {
            if (progress == null) progress = new WindowProgress();
            if (Parent(parent, out Window owner))
            {
                owner.Closed += delegate
                {
                    Hide();
                };
                progress.Owner = owner;
            }
            else
            {
                progress.Topmost = true;
            }
            if (dialog) progress.ShowDialog();
            else progress.Show();
        }
        /// <summary>
        /// 隐藏Window进度条
        /// </summary>
        public static void Hide()
        {
            if (progress != null) progress.Close();
            progress = null;
        }

        #endregion

        #region Window弹出
        /// <summary>
        /// 显示子窗体
        /// </summary>
        public static bool? Show(DependencyObject parent, Window window, bool iEscExit = true)
        {
            if (!Parent(parent, out Window owner)) return null;
            window.ShowInTaskbar = false;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
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
            if (iEscExit)
            {
                var iExit = false;
                var exitTime = DateTime.MinValue;
                var interval = NativeMethods.GetDoubleClickTime();
                window.KeyDown += delegate (object sender, System.Windows.Input.KeyEventArgs e)
                {
                    if (e.Key == System.Windows.Input.Key.Escape)
                    {
                        if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < interval)
                        {
                            window.Close();
                            return;
                        }
                        iExit = true;
                        exitTime = DateTime.Now;
                    }
                };
            }
            return window.ShowDialog();
        }

        #endregion

        #region Window系统消息框-Toast显示
        /// <summary>
        /// Window系统消息框-Toast显示
        /// </summary>
        public static void Toast(string msg, bool iError = false)
        {
            Toast(null, msg, iError);
        }
        /// <summary>
        /// Window系统消息框-Toast显示
        /// </summary>
        public static void Toast(DependencyObject parent, string msg, bool iError = false)
        {
            parent.Dispatcher.Invoke(() =>
            {
                var toast = new WindowToast();
                if (Parent(parent, out Window owner))
                {
                    toast.Owner = owner;
                }
                toast.Show(msg, iError);
            });
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
        public static bool Parent<T>(object obj, out T parent) where T : FrameworkElement
        {
            parent = null;
            if (!(obj is DependencyObject dependency)) return false;
            while (dependency != null)
            {
                if (dependency is T t)
                {
                    parent = t;
                    return true;
                }
                dependency = VisualTreeHelper.GetParent(dependency);
            }
            return false;
        }
        /// <summary>
        /// 查找指定类型子(同级)控件
        /// </summary>
        /// <typeparam name="T">查找控件类型</typeparam>
        /// <param name="obj">控件</param>
        /// <param name="child">返回指定类型控件</param>
        /// <param name="name">指定控件名称</param>
        /// <param name="iParent">指定搜索同级控件</param>
        /// <returns></returns>
        public static bool Child<T>(object obj, out T child, string name = null, bool iParent = true) where T : FrameworkElement
        {
            child = null;
            if (!(obj is DependencyObject dependency)) return false;
            if (iParent)
            {
                var parent = VisualTreeHelper.GetParent(dependency);
                if (parent != null) dependency = parent;
            }
            var count = VisualTreeHelper.GetChildrenCount(dependency);
            for (int i = 0; i < count; i++)
            {
                var value = VisualTreeHelper.GetChild(dependency, i);
                if (value is T temp)
                {
                    if (name == null || temp.Name == name)
                    {
                        child = temp;
                        return true;
                    }
                }
                if (Child(value, out child, name, false))
                {
                    return true;
                }
            }
            while (dependency is ContentControl control)
            {
                dependency = control.Content as DependencyObject;
                if (dependency is T temp)
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
