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
using System.Windows.Shapes;

namespace Paway.Model
{
    /// <summary>
    /// NameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NameWindow : WindowEXT
    {
        /// <summary>
        /// 单输入框通用窗体
        /// </summary>
        public NameWindow()
        {
            InitializeComponent();
            tbName.KeyDown += TbName_KeyDown;
        }
        private void TbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && this.DataContext is NameWindowModel window) window.EnterCommit(this);
        }
    }
}
