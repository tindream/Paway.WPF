using CommunityToolkit.Mvvm.Messaging;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Model
{
    /// <summary>
    /// ThemePage.xaml 的交互逻辑
    /// </summary>
    public partial class ThemePage : Page
    {
        /// <summary>
        /// 主题设置页
        /// </summary>
        public ThemePage()
        {
            InitializeComponent();
            this.Unloaded += ThemePage_Unloaded;
            Application.Current.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }
        /// <summary>
        /// 离开时还原主题
        /// </summary>
        private void ThemePage_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Default.ThemeView.Restore();
        }
        /// <summary>
        /// 关闭主窗体时还原主题
        /// </summary>
        private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            ViewModelLocator.Default.ThemeView.Restore();
        }
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            WeakReferenceMessenger.Default.Send(new ThemeLoadMessage());
        }
    }
}
