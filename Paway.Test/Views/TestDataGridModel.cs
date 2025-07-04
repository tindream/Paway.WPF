using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
    public class TestDataGridModel : ViewModelBasePlus
    {
        #region 属性
        private int _iValue;
        public int IValue
        {
            get { return _iValue; }
            set { _iValue = value; OnPropertyChanged(); }
        }
        public ICommand CurrentCellCommand => new RelayCommand<DataGridEXT>(e =>
        {
            if (e.CurrentCell.Column?.Header.ToStrings() != "List")
            {
                IValue = -1;
            }
        });
        public ObservableCollection<TestDataGridInfo> GridList { get; } = new ObservableCollection<TestDataGridInfo>();
        public List<ListViewItemModel> List { get; } = new List<ListViewItemModel>();

        #endregion

        public TestDataGridModel()
        {
            var index = 0;
            for (int i = 0; i < 5; i++)
            {
                var info = new TestDataGridInfo { Name = $"Hello{i + 1}" };
                for (int j = 0; j < i; j++) info.List.Add(new ListViewItemModel($"B{j + 1}") { Id = ++index });
                GridList.Add(info);
            }
            for (int j = 0; j < 5; j++) List.Add(new ListViewItemModel($"A{j + 1}") { Id = ++index });
        }
    }
    public class TestDataGridInfo : BaseModelInfo
    {
        public string Name { get; set; }
        public List<ListViewItemModel> List { get; set; } = new List<ListViewItemModel>();
    }
}