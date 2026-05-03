using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TestPlotView.xaml 的交互逻辑
    /// </summary>
    public partial class TestPlotView : Page
    {
        public TestPlotView()
        {
            InitializeComponent();
            this.Unloaded += TestPlotView_Unloaded;
        }
        private void TestPlotView_Unloaded(object sender, RoutedEventArgs e)
        {
            //关闭时解绑，释放引用
            //plotView.Model = null;
        }
    }
}
