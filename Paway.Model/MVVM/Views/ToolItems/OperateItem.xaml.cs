using GalaSoft.MvvmLight.Messaging;
using Paway.WPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Model
{
    /// <summary>
    /// OperateItem.xaml 的交互逻辑
    /// </summary>
    public partial class OperateItem
    {
        /// <summary>
        /// 通用工具栏
        /// </summary>
        public OperateItem()
        {
            InitializeComponent();
            Messenger.Default.Send(new OperateLoadMessage() { Obj = dpOperateItem });
            this.Loaded += OperateItem_Loaded;
        }
        /// <summary>
        /// 监听顶层窗体按键
        /// </summary>
        private void OperateItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (PMethod.Parent(this, out Window window))
            {
                window.PreviewKeyDown += Window_PreviewKeyDown;
            }
            this.Loaded -= OperateItem_Loaded;
        }
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!this.IsLoaded) return;
            var keyMsg = new KeyMessage(e.Key);
            Messenger.Default.Send(keyMsg, this.dpOperateItem.GetHashCode());
            e.Handled = keyMsg.Handled;
        }
    }
}
