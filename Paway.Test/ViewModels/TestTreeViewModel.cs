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
    public class TestTreeViewModel : ViewModelBasePlus
    {
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
        public ObservableCollection<TreeViewItemModel> TreeList { get; private set; } = new ObservableCollection<TreeViewItemModel>();

        public TestTreeViewModel()
        {
            AddTree();
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