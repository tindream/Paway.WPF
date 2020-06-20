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
        public List<ListViewModel> list;
        public MainWindow()
        {
            InitializeComponent();
            var list = new List<ListViewModel>();
            list.Add(new ListViewModel("Hello"));
            list.Add(new ListViewModel("你好123")
            {
                Image = new ImageEXT(null, @"pack://application:,,,/Paway.Test;component/Images/close_while.png")
            });
            for (int i = 0; i < 20; i++) list.Add(new ListViewModel("A" + i, "D" + i)
            {
                Image = new ImageEXT(@"pack://application:,,,/Paway.Test;component/Images/close.png")
            });
            datagrid1.ItemsSource = list;
            listView1.Items.Clear();
            listView1.ItemsSource = list;

            var multiList = new ObservableCollection<ComboBoxMultiModel>();
            multiList.Add(new ComboBoxMultiModel("张三"));
            multiList.Add(new ComboBoxMultiModel("李四"));
            multiList.Add(new ComboBoxMultiModel("王五"));
            multiList.Add(new ComboBoxMultiModel("马六"));
            multiList.Add(new ComboBoxMultiModel("赵七"));
            multiList.Add(new ComboBoxMultiModel("王八"));
            multiList.Add(new ComboBoxMultiModel("陈九"));
            MultiCmb.ItemsSource = multiList;

            var treeList = new ObservableCollection<ITreeView>();
            var treeInfo = new TreeViewModel("单位名称(3/7)", true);
            treeList.Add(treeInfo);
            var treeInfo2 = new TreeViewModel("未分组联系人(2/4)", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒1", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒2", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒3", "我要走向天空！", "3人");
            TreeViewOrg.ItemsSource = treeList;
        }

        private bool b;
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            var xml = Method.GetTemplateXaml(datagrid1);
            //Method.Toast(this, xml);
            b = !b;
            if (b) Method.Progress(this);
            else Method.Hide();
            transition.Transition = (TransitionType)new Random().Next(0, 5);
            //transition.Transition = TransitionType.Up;
        }
    }
}
