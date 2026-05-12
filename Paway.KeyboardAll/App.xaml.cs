using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.KeyboardAll
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (!PMethod.InitApp(existTitle: "虚拟键盘"))
            {
                Application.Current.Shutdown();
                return;
            }
            // 启动参数控制默认显示
            if (e.Args.Length > 0)
            {
                var param1 = e.Args[0];
                if (param1.TEquals("-allnum"))
                {
                    new KeyboardAllWindow(null, true, true).Show();
                }
                else if (param1.TEquals("-num"))
                {
                    new KeyboardNumWindow(null, true).Show();
                }
                else
                {
                    new KeyboardAllWindow(null, true).Show();
                }
            }
            else
            {
                new KeyboardAllWindow(null, true).Show();
            }
        }
    }
}
