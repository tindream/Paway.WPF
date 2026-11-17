using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 一些帮助方法
    /// </summary>
    public partial class PMethod : TMethod
    {
        static PMethod()
        {
            PConfig.InitResources();
        }

        #region Init
        /// <summary>
        /// 初始化App
        /// <para>existTitle：不为空时，检查实例，实例存在时激活指定标题的窗体，未找到实例时 关闭当前进程名称的其它所有进程</para>
        /// <para>logFirstChanceException：记录异常，默认记录</para>
        /// </summary>
        public new static bool InitApp(string logFile = "Log.xml", string existTitle = null, object key = null, bool logFirstChanceException = true)
        {
            if (!InitAppInstance(existTitle, key))
            {
                Application.Current.Shutdown();
                return false;
            }
            InitAppError(logFile, logFirstChanceException);
            //禁用Backspace退格导航返回Page页
            NavigationCommands.BrowseBack.InputGestures.Clear();
            Application.Current.DispatcherUnhandledException += App_DispatcherUnhandledException;
            PConfig.InitResources();
            Task.Run(() =>
            {
                try
                {
                    //等待主窗体创建（最多约5秒）
                    for (var i = 0; i < 1000; i++)
                    {
                        if (PConfig.Window != null) break;
                        Invoke(() =>
                        {
                            //主窗体未创建时赋值会触发异常并刷日志，这里先判空
                            var mainWindow = Application.Current?.MainWindow;
                            if (mainWindow != null && PConfig.Window == null) PConfig.Window = mainWindow;
                        });
                        if (PConfig.Window != null) break;
                        Thread.Sleep(5);
                    }
                    if (PConfig.Window == null) "获取MainWindow失败".Warn();
                }
                catch (Exception ex)
                {
                    ex.Log("获取MainWindow失败");
                }
            });
            return true;
        }
        /// <summary>
        /// 未经处理的UI异常通知
        /// <para>默认把异常标记为已处理（只记录日志，进程继续运行）</para>
        /// <para>宿主可在处理函数内把 e.Handled 置为 false，让异常继续向上抛出（进程通常会退出）</para>
        /// </summary>
        public static event Action<DispatcherUnhandledExceptionEventArgs> UnhandledException;
        private static void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //先给出默认处置（存活），再把参数交给宿主，宿主改 e.Handled 才真正起作用
            e.Handled = true;
            try
            {
                e.Exception.Log("[未经处理的UI异常]");
                UnhandledException?.Invoke(e);
            }
            catch (Exception ex)
            {
                //日志或宿主回调自身出错：保持默认处置继续运行，不把二次异常抛回 Dispatcher
                try
                {
                    ex.Log("[未经处理UI异常的处理流程失败]");
                }
                catch
                {
                    // ignored
                }
            }
        }

        #endregion

        #region Menu
        /// <summary>
        /// Menu菜单绑定多语言
        /// </summary>
        public static void LanguageMenuBinding(MenuItem menu, List<string> languageList, object model, ICommand command, string languageName)
        {
            menu.Items.Clear();
            foreach (var language in languageList)
            {
                var menuItem = new MenuItem() { Header = language };
                menuItem.Command = command;
                menuItem.CommandParameter = language;
                {  //实例化绑定对象
                    var isCheckedBinding = new Binding();
                    //设置要绑定源
                    isCheckedBinding.Source = model;//绑定ViewModel类
                    isCheckedBinding.Path = new PropertyPath(languageName);//绑定MainWindow类下的Language属性。
                    isCheckedBinding.Mode = BindingMode.TwoWay;//绑定模式双向绑定
                    isCheckedBinding.Converter = new ValueToTrue();
                    isCheckedBinding.ConverterParameter = language;
                    menuItem.SetBinding(MenuItem.IsCheckedProperty, isCheckedBinding);//设置绑定到要绑定的控件
                }
                menu.Items.Add(menuItem);
            }
        }
        #endregion

        #region 颜色
        /// <summary>
        /// 取颜色拾取器中的颜色值
        /// <para>拖动取色时高频调用，避免每次 Linq 与数组分配</para>
        /// </summary>
        /// <param name="offset"></param>
        public static Color ColorSelector(double offset)
        {
            var application = Application.Current;
            if (application == null) return Colors.Transparent;
            if (!(application.TryFindResource("ColorSelector") is LinearGradientBrush linearGradientBrush)) return Colors.Transparent;

            var stops = linearGradientBrush.GradientStops;
            var count = stops.Count;
            if (count == 0) return Colors.Transparent;
            if (offset <= 0) return stops[0].Color;
            if (offset >= 1) return stops[count - 1].Color;

            var left = stops[0];
            var right = stops[count - 1];
            for (var i = 0; i < count; i++)
            {
                var stop = stops[i];
                if (stop.Offset <= offset)
                {
                    left = stop;
                    continue;
                }
                right = stop;
                break;
            }
            var interval = right.Offset - left.Offset;
            if (interval == 0) return left.Color;
            offset = Math.Round((offset - left.Offset) / interval, 2);
            var a = (byte)((right.Color.A - left.Color.A) * offset + left.Color.A);
            var r = (byte)((right.Color.R - left.Color.R) * offset + left.Color.R);
            var g = (byte)((right.Color.G - left.Color.G) * offset + left.Color.G);
            var b = (byte)((right.Color.B - left.Color.B) * offset + left.Color.B);
            return Color.FromArgb(a, r, g, b);
        }

        #endregion
    }
}
