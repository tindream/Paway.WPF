using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MainWindow : WindowEXT
    {
        public List<TestInfo> list;
        public MainWindow()
        {
            InitializeComponent();
            var list = new List<TestInfo>();
            list.Add(new TestInfo("Hello"));
            list.Add(new TestInfo("你好123", 70)
            {
                Image = new ImageEXT(null, @"pack://application:,,,/Paway.Test;component/Images/close_while.png")
            });
            for (int i = 0; i < 20; i++) list.Add(new TestInfo("A" + i, i)
            {
                Desc = "D" + i,
                Image = new ImageEXT(@"pack://application:,,,/Paway.Test;component/Images/close.png")
            });
            datagrid1.ItemsSource = list;
            listView1.Items.Clear();
            listView1.ItemsSource = list;

            var multiList = new ObservableCollection<ComboBoxMultiInfo>();
            multiList.Add(new ComboBoxMultiInfo("张三"));
            multiList.Add(new ComboBoxMultiInfo("李四"));
            multiList.Add(new ComboBoxMultiInfo("王五"));
            multiList.Add(new ComboBoxMultiInfo("马六"));
            multiList.Add(new ComboBoxMultiInfo("赵七"));
            multiList.Add(new ComboBoxMultiInfo("王八"));
            multiList.Add(new ComboBoxMultiInfo("陈九"));
            MultiCmb.ItemsSource = multiList;

            var treeList = new ObservableCollection<ITreeView>()
            {
                new TreeViewInfo()
                {
                    IsGrouping = true,
                    GroupName = "单位名称(3/7)",
                    Children = new ObservableCollection<ITreeView>()
                    {
                        new TreeViewInfo(){
                            IsGrouping=true,
                            GroupName="未分组联系人(2/4)",
                            Children=new ObservableCollection<ITreeView>()
                            {
                                new TreeViewInfo(){
                                    IsGrouping=false,
                                    SurName="刘",
                                    Name="刘棒",
                                    Subtitle="我要走向天空！",
                                    Desc="3人"
                                }
                            }
                        }
                    },
                }
            };
            TreeViewOrg.ItemsSource = treeList;
        }

        private bool b;
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            var xml = Method.GetTemplateXaml(TreeViewOrg);
            //Method.Toast(this, xml);
            b = !b;
            if (b) Method.Progress(this);
            else Method.Hide();
            transition.Transition = (TransitionType)new Random().Next(0, 5);
            //transition.Transition = TransitionType.Up;
        }
    }
    public class TreeViewInfo : ITreeView
    {
        public bool? IsChecked { get; set; } = false;
        public bool IsGrouping { get; set; }
        public ObservableCollection<ITreeView> Children { get; set; }
        public string GroupName { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Desc { get; set; }
    }

    public class ComboBoxMultiInfo : IComboBoxMulti
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsChecked { get; set; }
        public ComboBoxMultiInfo(string text)
        {
            this.Id = this.GetHashCode();
            this.Text = text;
        }
    }
    public class TestInfo : IListView
    {
        [NoShow]
        public int Id { get; set; }
        [Text("名称")]
        public string Content { get; set; }
        public double Money { get; set; }
        [Text("进度")]
        public double Value { get; set; }
        public bool IsSelected { get; set; }
        public ImageEXT Image { get; set; }
        public string Desc { get; set; }

        public TestInfo(string name, double value = 0)
        {
            this.Content = name;
            this.Money = value;
            this.Value = value;
        }
    }
}
