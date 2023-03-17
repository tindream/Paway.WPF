using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
            Config.Text = Config.Title;
            Method.InitApp(App.Current, Config.LogConfig);
            if (Method.IsAppInstanceExist())
            {//已经有实例在运行，则激活该实例的主窗体。 
                var hWnd = Win32Helper.ActiveForm(Config.Text);
                Shutdown();
                return;
            }
            {//Test
            }
            //System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            Config.Language = Proxy.Create<LanguageInfo>(typeof(InterceptorNotify), nameof(InterceptorNotify.Invoke), Config.Flags);
            var lan = XmlHelper.Load<LanguageInfo>("lan.xml");
            lan.Clone(Config.Language);
            XmlHelper.Save(Config.Language, "lan.xml");
            Config.InitLanguageBase(Config.Language);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
