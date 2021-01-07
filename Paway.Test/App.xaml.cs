using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Paway.Test
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

            log4net.Config.XmlConfigurator.Configure();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            $"v{version} ({Environment.MachineName})".Log();

            DataService.Default.Init();
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            (e.ExceptionObject as Exception).Log("未经处理的线程异常。");
        }
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Exception.Log("未经处理的UI异常。");
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
