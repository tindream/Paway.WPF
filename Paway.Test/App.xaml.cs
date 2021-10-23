using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
            var repository = log4net.LogManager.CreateRepository(Config.LogRepository);
            log4net.Config.XmlConfigurator.Configure(repository, new FileInfo(Config.LogConfig));
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            $"v{version} ({Environment.MachineName})".Log();
            {//Test
            }
            //System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            ExceptionHelper.Init(null, Config.Text);
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            App.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        }
        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            var desc = $"捕获到未处理的异常";
            e.Exception.Log(desc);
        }
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ///设置该异常已察觉（这样处理后就不会引起程序崩溃）
            e.SetObserved();
            var desc = $"未经处理的Task异常";
            e.Exception.Log(desc);
            Messenger.Default.Send(new ErrorStatuMessage(desc, e.Exception));
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is Exception ex)
                {
                    var desc = $"未经处理的线程异常";
                    if (e.IsTerminating) desc += $"(致命错误)";
                    ex.Log(desc);
                    Messenger.Default.Send(new ErrorStatuMessage(desc, ex));
                }
            }
            catch (Exception ex)
            {
                var desc = $"不可恢复的未经处理线程异常";
                ex.Log(desc);
                Messenger.Default.Send(new ErrorStatuMessage(desc, ex));
            }

        }
        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var desc = $"未经处理的UI异常";
                e.Exception.Log(desc);
                e.Handled = true;
                Messenger.Default.Send(new ErrorStatuMessage(desc, e.Exception));
            }
            catch (Exception ex)
            {
                var desc = $"不可恢复的未经处理UI异常";
                ex.Log(desc);
                Messenger.Default.Send(new ErrorStatuMessage(desc, ex));
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
