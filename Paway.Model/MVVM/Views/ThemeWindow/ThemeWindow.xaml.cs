using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
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
    public partial class ThemeWindow : Window
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
            Messenger.Default.Send(new ThemeLoadMessage() { Obj = root });
        }
    }
}
