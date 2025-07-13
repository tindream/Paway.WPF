using GalaSoft.MvvmLight.Messaging;
using Paway.Helper;
using Paway.WPF;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TestDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class TestDataGrid : Page
    {
        /// <summary>
        /// 合并行测试
        /// </summary>
        public TestDataGrid()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Messenger.Default.Send(new TestDataGridLoadMessage() { Obj = Root });
        }
    }
}
