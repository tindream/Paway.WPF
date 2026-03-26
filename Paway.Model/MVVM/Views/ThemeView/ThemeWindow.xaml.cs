using CommunityToolkit.Mvvm.Messaging;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paway.Model
{
    /// <summary>
    /// ThemeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ThemeWindow : WindowEXT
    {
        /// <summary>
        /// 主题设置窗体
        /// </summary>
        public ThemeWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            WeakReferenceMessenger.Default.Send(new ThemeLoadMessage());
        }
        /// <summary>
        /// 关闭时还原主题
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModelLocator.Default.ThemeView.Restore();
            base.OnClosing(e);
        }
    }
}
