using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法 - 系统
    /// </summary>
    public partial class PMethod
    {
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
        /// <summary>
        /// 刷新主题样式
        /// </summary>
        public static void DoStyles()
        {
            var list = new List<ResourceDictionary>();
            foreach (var item in Application.Current.Resources.MergedDictionaries) list.Add(item);

            Application.Current.Resources.MergedDictionaries.Clear();
            foreach (var item in list) Application.Current.Resources.MergedDictionaries.Add(item);
        }

        #endregion

        #region Window弹框
        /// <summary>
        /// Window弹框
        /// </summary>
        public static bool? Show(DependencyObject parent, Window window, int alpha = 100, bool iFocus = true, bool iEscExit = true)
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
            if (iFocus) window.LostKeyboardFocus += delegate
            {
                if (!window.IsKeyboardFocusWithin) window.Focus();
            };
            if (!(window is WindowEXT))
            {
                //window.Loaded += delegate { window.Activate(); };
            }
            //父级窗体原来的内容
            var original = owner.Content as UIElement;
            //将父级窗体原来的内容在容器Grid中移除
            owner.Content = null;
            //容器Grid
            var container = new Grid();
            //放入原来的内容
            container.Children.Add(original);
            //蒙板
            var layer = new Grid() { Background = AlphaColor(alpha, Colors.Black).ToBrush() };
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

        #region Window系统消息框
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Warning图标。</para>
        /// </summary>
        public static void ShowWarning(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Warn);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和Error图标。</para>
        /// </summary>
        public static void ShowError(DependencyObject parent, string msg)
        {
            Show(parent, msg, LeveType.Error);
        }
        /// <summary>
        /// Window系统消息框
        /// <para>该消息框显示消息、 标题栏标题、 OK按钮和指定图标(默认Information)。</para>
        /// </summary>
        public static void Show(DependencyObject parent, string msg, LeveType level = LeveType.Debug)
        {
            if (!Parent(parent, out Window window)) return;
            BeginInvoke(obj =>
            {
                switch (level)
                {
                    case LeveType.Debug:
                    default:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case LeveType.Warn:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    case LeveType.Error:
                        MessageBox.Show(window, obj, window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }, msg);
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
        /// 返回控件树中指定类型控件
        /// </summary>
        public static bool Find<T>(object obj, out T parent, string name = null) where T : FrameworkElement
        {
            parent = null;
            if (!(obj is DependencyObject dependency)) return false;
            if (Child(obj, out parent, name))
            {
                return true;
            }
            var hasParent = false;
            while (dependency != null)
            {
                if (dependency is T t)
                {
                    if (name == null || t.Name == name)
                    {
                        parent = t;
                        return true;
                    }
                }
                var temp = VisualTreeHelper.GetParent(dependency);
                if (temp == null) break;
                dependency = temp;
                hasParent = true;
            }
            if (hasParent && Child(dependency, out parent, name))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 返回控件的顶层指定类型控件
        /// </summary>
        public static bool Parent<T>(object obj, out T parent, string name = null) where T : FrameworkElement
        {
            if (obj is T t1)
            {
                if (name == null || t1.Name == name)
                {
                    parent = t1;
                    return true;
                }
            }
            parent = null;
            if (!(obj is DependencyObject dependency)) return false;
            dependency = VisualTreeHelper.GetParent(dependency);
            while (dependency != null)
            {
                if (dependency is T t)
                {
                    if (name == null || t.Name == name)
                    {
                        parent = t;
                        return true;
                    }
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
        /// <param name="func">外部条件，在多子项时判断</param>
        /// <returns></returns>
        public static bool Child<T>(object obj, out T child, string name = null, bool iParent = true, Func<T, bool> func = null) where T : FrameworkElement
        {
            child = null;
            if (!(obj is DependencyObject dependency)) return false;
            if (iParent)
            {
                var parent = VisualTreeHelper.GetParent(dependency);
                if (parent != null) dependency = parent;
            }
            var count = VisualTreeHelper.GetChildrenCount(dependency);
            for (int i = count - 1; i >= 0; i--)
            {
                var value = VisualTreeHelper.GetChild(dependency, i);
                if (value is T temp)
                {
                    if ((name == null || temp.Name == name) && func?.Invoke(temp) != false)
                    {
                        child = temp;
                        return true;
                    }
                }
                if (Child(value, out child, name, false, func))
                {
                    return true;
                }
            }
            while (dependency is ContentControl control)
            {
                dependency = control.Content as DependencyObject;
                if (dependency is T temp)
                {
                    if ((name == null || temp.Name == name) && func?.Invoke(temp) != false)
                    {
                        child = temp;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取所有子控件中的验证错误列表
        /// </summary>
        public static List<string> ValidationError(DependencyObject dependency)
        {
            var errorList = new List<string>();
            ValidationError(dependency, errorList);
            return errorList;
        }
        private static void ValidationError(DependencyObject dependency, List<string> errorList)
        {
            var count = VisualTreeHelper.GetChildrenCount(dependency);
            for (var i = 0; i < count; i++)
            {
                var value = VisualTreeHelper.GetChild(dependency, i);
                if (Validation.GetHasError(value))
                {
                    foreach (ValidationError error in Validation.GetErrors(value))
                    {
                        errorList.Add(error.ErrorContent.ToString());
                    }
                }
                ValidationError(value, errorList);
            }
        }
        /// <summary>
        /// 验证模型中的指定名称控件值输入错误
        /// <para>输入控件限定为TextBoxEXT，控件名称为tb+name</para>
        /// </summary>
        public static bool ValidationError<T>(DependencyObject parent, T mode, string name, bool allEmpty = false) where T : class
        {
            if (Find(parent, out TextBoxEXT tbName, "tb" + name))
            {
                if (!allEmpty && mode.GetValue(name).ToStrings().IsEmpty())
                {
                    Hit(parent, "请输入" + mode.Property(name).Text(), ColorType.Warn);
                    tbName.Focus();
                    return false;
                }
                if (Validation.GetHasError(tbName))
                {
                    Hit(parent, Validation.GetErrors(tbName).First().ErrorContent, ColorType.Error);
                    tbName.Focus();
                    return false;
                }
            }
            return true;
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

        #endregion

        #region 统一Invoke处理
        /// <summary>
        /// 同步调用
        /// <para>任何与 Application 不在同一个线程的代码，都可能遭遇 Application.Current 为 null。如Shutdown关闭</para>
        /// </summary>
        public static void Invoke(Action action, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    action.Invoke();
                });
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }
        /// <summary>
        /// 带参数同步调用
        /// </summary>
        public static void Invoke<T>(Action<T> action, T t, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.Invoke(() =>
                {
                    action.Invoke(t);
                });
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }
        /// <summary>
        /// 同步调用，并返回结果
        /// </summary>
        public static T Invoke<T>(Func<T> action, Action<Exception> error = null)
        {
            try
            {
                return Application.Current == null ? default : Application.Current.Dispatcher.Invoke(() =>
                {
                    return action.Invoke();
                });
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
                return default;
            }
        }
        /// <summary>
        /// 带参数同步调用，并返回结果
        /// </summary>
        public static O Invoke<T, O>(Func<T, O> action, T t, Action<Exception> error = null)
        {
            try
            {
                return Application.Current == null ? default : Application.Current.Dispatcher.Invoke(() =>
                {
                    return action.Invoke(t);
                });
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
                return default;
            }
        }
        /// <summary>
        /// 异步调用
        /// </summary>
        public static void BeginInvoke(Action action, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                }));
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }
        /// <summary>
        /// 带参数异步调用
        /// </summary>
        public static void BeginInvoke<T>(Action<T> action, T t, Action<Exception> error = null)
        {
            try
            {
                Application.Current?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        action.Invoke(t);
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                }));
            }
            catch (Exception ex)
            {
                if (error == null) ex.Log();
                else error.Invoke(ex);
            }
        }

        #endregion

        #region Init
        /// <summary>
        /// 初始化App
        /// </summary>
        public static void InitApp(Application app, string fileName = "Log.xml")
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(fileName));
            var version = Assembly.GetEntryAssembly().GetName().Version;
            $"v{version} ({Environment.MachineName})".Log(LeveType.Error);

            //禁用Backspace退格导航返回Page页
            NavigationCommands.BrowseBack.InputGestures.Clear();
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            app.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // 设置该异常已察觉（这样处理后就不会引起程序崩溃）
            e.SetObserved();
            var desc = $"未经处理的Task异常";
            e.Exception.Log(desc);
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is Exception ex)
                {
                    var desc = $"未经处理的线程异常";
                    if (e.IsTerminating) desc += $"(致命错误)";
                    ex.Log(desc);
                }
            }
            catch (Exception ex)
            {
                var desc = $"不可恢复的未经处理线程异常";
                ex.Log(desc);
            }

        }
        private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var desc = $"未经处理的UI异常";
                e.Exception.Log(desc);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                var desc = $"不可恢复的未经处理UI异常";
                ex.Log(desc);
            }
        }

        #endregion
    }
}
