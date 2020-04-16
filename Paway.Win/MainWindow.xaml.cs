using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Win
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var list = new List<TestInfo>();
            list.Add(new TestInfo("Hello"));
            list.Add(new TestInfo("你好", 70));
            datagrid1.ItemsSource = list;
        }
    }
    public class TestInfo
    {
        [NoShow]
        public int Id { get; set; }
        [Text("名称")]
        public string Name { get; set; }
        public double Value { get; set; }

        public TestInfo(string name, double value = 0)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
