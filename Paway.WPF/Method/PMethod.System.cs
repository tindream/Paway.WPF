using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using Microsoft.Win32;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法 - 系统
    /// </summary>
    public partial class PMethod
    {
        #region 导入导出框
        /// <summary>
        /// 打开单个文件
        /// </summary>
        public new static bool OpenFile(string titleName, out string file, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            file = null;
            var ofd = new OpenFileDialog
            {
                Title = $"选择要导入的 {titleName}",
                Filter = filter,
            };
            if (ofd.ShowDialog() == true)
            {
                file = ofd.FileName;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 打开多个文件
        /// </summary>
        public new static bool OpenFiles(string title, out string[] file, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            file = null;
            var ofd = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = true,
            };
            if (ofd.ShowDialog() == true)
            {
                file = ofd.FileNames;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 导出到文件
        /// </summary>
        public new static bool SaveFile(string fileName, out string outFile, string filter = null)
        {
            if (filter == null)
            {
                var extension = Path.GetExtension(fileName);
                switch (extension)
                {
                    case ".xls":
                    case ".xlsx": filter = $"Excel 工作簿|*{extension}|所有文件|*.*"; break;
                    case ".doc":
                    case ".docx": filter = $"Word 文档|*{extension}|所有文件|*.*"; break;
                    case ".ppt":
                    case ".pptx": filter = $"PPT 文稿|*{extension}|所有文件|*.*"; break;
                    case ".pdf": filter = $"PDF 文件|*{extension}|所有文件|*.*"; break;
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".bmp": filter = $"图像文件|*{extension}|所有文件|*.*"; break;
                    case ".avi":
                    case ".wmv":
                    case ".mp4":
                    case ".mpg":
                    case ".mpeg":
                    case ".mov":
                    case ".rm":
                    case ".ram":
                    case ".swf":
                    case ".flv": filter = $"视频文件|*{extension}|所有文件|*.*"; break;
                    default: filter = "Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls"; break;
                }
            }
            outFile = null;
            var sfd = new SaveFileDialog()
            {
                Title = $"选择要导出的文件位置",
                Filter = filter,
                FileName = fileName,
            };
            if (sfd.ShowDialog() == true)
            {
                outFile = sfd.FileName;
                return true;
            }
            return false;
        }

        #endregion

        #region 对一个Handle控件进行截图
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);
        /// <summary>
        /// 对一个Handle控件进行截图
        /// </summary>
        public static System.Drawing.Bitmap PrintWindow(FrameworkElement framework, IntPtr intptr)
        {
            // 获取宽高
            int screenWidth = (int)framework.ActualWidth;
            int screenHeight = (int)framework.ActualHeight;

            //创建图形
            var bitmap = new System.Drawing.Bitmap(screenWidth, screenHeight, System.Drawing.Imaging.PixelFormat.Format16bppRgb555);
            using (var g = System.Drawing.Graphics.FromImage(bitmap))
            {
                var hdc = g.GetHdc();

                //调用api 把hwnd的内容用图形绘制到hdc 如果你有代码洁癖 可以不使用api 使用g.CopyFromScreen，请自行研究
                var result = PrintWindow(intptr, hdc, 0);
                g.ReleaseHdc(hdc);
                g.Flush();
                if (result) return bitmap;
            }
            return null;
        }

        #endregion

        #region 代码移除全局焦点样式
        /// <summary>
        /// 代码移除全局焦点样式
        /// <para>在主窗体构造或更早之前调用</para>
        /// </summary>
        public static void RemoveFocusVisualStyle()
        {
            EventManager.RegisterClassHandler(typeof(FrameworkElement), FrameworkElement.GotFocusEvent, new RoutedEventHandler(RemoveFocusVisualStyle), true);
        }
        private static void RemoveFocusVisualStyle(object sender, RoutedEventArgs e)
        {
            (sender as FrameworkElement).FocusVisualStyle = null;
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
        /// Window模式弹框（带灰层背景）
        /// </summary>
        public static bool? ShowWindow(DependencyObject parent, Window window, int alpha = 100, bool iFocus = true, bool iEscExit = true)
        {
            if (!Parent(parent, out Window owner)) return null;
            //蒙板
            var layer = new Grid() { Background = AlphaColor(alpha, Colors.Black).ToBrush() };
            //使用装饰器装载
            var desktopAdorner = CustomAdorner(owner, layer);

            window.ShowInTaskbar = false;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Closed += delegate
            {
                //移除装饰器
                ClearAdorner(owner, desktopAdorner);
            };
            if (iFocus) window.LostKeyboardFocus += delegate
            {
                if (!window.IsKeyboardFocusWithin) window.Focus();
            };
            if (!(window is WindowEXT))
            {
                //window.Loaded += delegate { window.Activate(); };
            }
            //弹出消息框 
            window.Owner = owner;
            if (iEscExit)
            {
                var iExit = false;
                var exitTime = DateTime.MinValue;
                window.KeyDown += delegate (object sender, KeyEventArgs e)
                {
                    if (e.Key == Key.Escape)
                    {
                        if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < PConfig.DoubleInterval)
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
        /// <summary>
        /// 再次加载空窗体，达到释放资源的目的？
        /// CleanWindow
        /// </summary>
        public static void CleanWindow(Window window)
        {
            window.ShowInTaskbar = false;
            window.WindowState = WindowState.Normal;
            window.Left = SystemParameters.PrimaryScreenWidth;
            window.Show();
            window.Close();
        }
        /// <summary>
        /// Window全屏模式弹框
        /// </summary>
        public static void FullscreenWindow(UIElement element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            ContentPresenter content = null;
            Panel panel = null;
            if (parent is ContentPresenter content2)
            {
                content = content2;
                content.Content = null;
            }
            else if (parent is Panel panel2)
            {
                panel = panel2;
                panel.Children.Remove(element);
            }
            var fullScreenWindow = new Window
            {
                WindowStyle = System.Windows.WindowStyle.None,
                WindowState = WindowState.Maximized,
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Content = element
            };
            fullScreenWindow.Closed += (sender, e) =>
            {
                fullScreenWindow.Content = null;
                if (content != null) content.Content = element;
                else panel?.Children.Add(element);
                parent = null;
            };

            fullScreenWindow.ShowDialog();
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
            if (Find(parent, out TextBoxEXT tbName, "tb" + name) && tbName.Visibility == Visibility.Visible)
            {
                if (!allEmpty && mode.GetValue(name).ToStrings().IsEmpty())
                {
                    Hit(parent, $"{PConfig.LanguageBase.PleaseInput}{mode.Property(name).Text()}", ColorType.Warn);
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

        #region 获取控件的XAML代码
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
        /// 获取组件界面XAML代码
        /// <para>从相对路径URI</para>
        /// </summary>
        public static string GetComponentXmal(string uriStr, string toFlile)
        {
            var uri = new Uri(uriStr, UriKind.Relative);
            var obj = Application.LoadComponent(uri);
            var xaml = XamlWriter.Save(obj);
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
                Application.Current?.Dispatcher.Invoke(new Action<T>(arg =>
                {
                    action.Invoke(arg);
                }), t);
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
                return Application.Current == null ? default : (O)Application.Current.Dispatcher.Invoke(new Func<T, O>(arg =>
                {
                    return action.Invoke(arg);
                }), t);
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
                Application.Current?.Dispatcher.BeginInvoke(new Action<T>(arg =>
                {
                    try
                    {
                        action.Invoke(arg);
                    }
                    catch (Exception ex)
                    {
                        if (error == null) ex.Log();
                        else error.Invoke(ex);
                    }
                }), t);
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
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(file));
            var version = Assembly.GetEntryAssembly().GetName().Version;
            $"{AppDomain.CurrentDomain.FriendlyName} v{version} ({Environment.MachineName})".Log(LeveType.Error);

            //禁用Backspace退格导航返回Page页
            NavigationCommands.BrowseBack.InputGestures.Clear();
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            app.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                // 设置该异常已察觉（这样处理后就不会引起程序崩溃）
                e.SetObserved();
                var desc = $"未经处理的Task异常";
                e.Exception.Log(desc);
            }
            catch (Exception ex)
            {
                var desc = $"不可恢复的未经处理Task异常";
                ex.Log(desc);
            }
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                var desc = $"未经处理的线程异常";
                if (e.IsTerminating) desc += $"(致命错误)";
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
