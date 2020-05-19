using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<TestInfo> list;
        public MainWindow()
        {
            InitializeComponent();
            var list = new List<TestInfo>();
            list.Add(new TestInfo("Hello"));
            list.Add(new TestInfo("你好123", 70)
            {
                ImageMouse = new BitmapImage(new Uri(@"pack://application:,,,/Paway.Test;component/Images/close_while.png"))
            });
            for (int i = 0; i < 20; i++) list.Add(new TestInfo("A" + i, i)
            {
                Desc = "D" + i,
                Image = new BitmapImage(new Uri(@"pack://application:,,,/Paway.Test;component/Images/close.png"))
            });
            datagrid1.ItemsSource = list;
            listView1.Items.Clear();
            listView1.ItemsSource = list;
            listView1.SelectionChanged += ListView1_SelectionChanged;
        }

        private void ListView1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0) Method.Show(this, (e.AddedItems[0] as TestInfo).Content);
            listView1.SelectedIndex = -1;
        }

        private void ButtonRound_Click(object sender, RoutedEventArgs e)
        {
            var xml = Method.GetTemplateXaml(tb);
            Method.Toast(xml);
        }
    }
    public class TestInfo
    {
        [NoShow]
        public int Id { get; set; }
        [Text("名称")]
        public string Content { get; set; }
        public double Money { get; set; }
        [Text("进度")]
        public double Value { get; set; }
        public bool IsSelected { get; set; }
        public ImageSource Image { get; set; }
        public ImageSource ImageMouse { get; set; }
        public string Desc { get; set; }

        public TestInfo(string name, double value = 0)
        {
            this.Content = name;
            this.Money = value;
            this.Value = value;
            //this.Image = BitmapHelper.GetBitmapFormFile("close.png");
        }
    }
}
