using CommunityToolkit.Mvvm.Input;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test
{
    public class TestComboBoxModel : ViewModelBasePlus
    {
        private int _value = 11;
        public int Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private MenuAuthType _menuAuthType;
        public MenuAuthType MenuAuthType
        {
            get { return _menuAuthType; }
            set { _menuAuthType = value; OnPropertyChanged(); }
        }
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<IComboBoxItem> MultiList { get; } = new ObservableCollection<IComboBoxItem>();

        private DateTime datePickerTime = DateTime.Now;
        public DateTime DatePickerTime
        {
            get { return datePickerTime; }
            set { datePickerTime = value; OnPropertyChanged(); }
        }
        private int treeId;
        public int TreeId
        {
            get { return treeId; }
            set
            {
                if (treeId != value)
                {
                    treeId = value;
                    OnPropertyChanged();
                }
            }
        }
        private readonly List<ListViewItemModel> list = new List<ListViewItemModel>();
        public List<ListViewItemModel> GridList { get { return list; } }
        public ObservableCollection<TreeViewItemModel> TreeList { get; private set; } = new ObservableCollection<TreeViewItemModel>();

        public ICommand CbxFilterCmd => new RelayCommand<CustomFilterEventArgs>(e =>
        {
            if (e.Source is DataGridEXT dataGrid)
            {
                var p = dataGrid.Columns.Predicate<ListViewItemModel>(e.Filter);
                e.List = this.list.AsParallel().Where(p).ToList();
            }
        });

        public TestComboBoxModel()
        {
            list.Add(new ListViewItemModel("Hello"));
            list.Add(new ListViewItemModel("你好123")
            {
                IsEnabled = false,
                Image = new ImageSourceEXT(null, @"pack://application:,,,/Paway.Test;component/Images/close_white.png")
            });
            for (int i = 0; i < 20; i++) list.Add(new ListViewItemModel("A" + i, "D" + i)
            {
                IsEnabled = i != 5,
                Image = new ImageSourceEXT(@"pack://application:,,,/Paway.Test;component/Images/close.png")
            });
            this.id = list[0].Id;
            AddComboMulti();
            AddTree();
        }
        private void AddComboMulti()
        {
            MultiList.Add(new ComboBoxItemModel("张三") { IsChecked = true });
            MultiList.Add(new ComboBoxItemModel("李四"));
            MultiList.Add(new ComboBoxItemModel("王五"));
            MultiList.Add(new ComboBoxItemModel("马六"));
            MultiList.Add(new ComboBoxItemModel("赵七") { IsChecked = true });
            MultiList.Add(new ComboBoxItemModel("王八"));
            MultiList.Add(new ComboBoxItemModel("陈九"));
        }
        private void AddTree()
        {
            var treeInfo = new TreeViewItemModel("单位名称(3/7)", true);
            this.TreeList.Add(treeInfo);
            var treeInfo2 = new TreeViewItemModel("未分组联系人(2/4)", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒1", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒2", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒3", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒4", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒5", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒6", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒7", "我要走向天空！", "3人");

            treeInfo2 = new TreeViewItemModel("未分组联系人", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒A", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒B", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒C", "我要走向天空！", "3人");
            treeInfo2.Add("刘棒5", "我要走向天空！", "3人");

            this.TreeId = treeInfo2.Children[0].Id;
        }
    }
}