using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test.ViewModel
{
    public class TestViewModel : ViewModelBase
    {
        #region 属性
        private double angle = -27;
        public double Angle
        {
            get { return angle; }
            set { angle = value; RaisePropertyChanged(); }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                var angleMin = -27;
                var angleMax = 207;
                Angle = angleMin + (angleMax - angleMin) * value / 100.0;
                _value = value; RaisePropertyChanged();
            }
        }

        private MonitorType userType = MonitorType.LeftToeAngle;
        public MonitorType UserType
        {
            get { return userType; }
            set
            {
                if (value > 0) userType &= value;
                else userType = (MonitorType)(userType.GetHashCode() + value.GetHashCode());
                RaisePropertyChanged();
            }
        }
        private DateTime time = DateTime.Now.Date;
        public DateTime Time
        {
            get { return time; }
            set { time = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<TreeViewItemModel> TreeList { get; private set; } = new ObservableCollection<TreeViewItemModel>();

        private ICommand buttonCommand;
        public ICommand ButtonCommand
        {
            get
            {
                return buttonCommand ?? (buttonCommand = new RelayCommand(() =>
                {
                    if (Time.Second < 59) Time = Time.AddSeconds(1);
                    else if (Time.Minute < 59) Time = Time.AddMinutes(1);
                    else if (Time.Hour < 23) Time = Time.AddHours(1);
                }));
            }
        }

        #endregion

        public ObservableCollection<ListViewItemModel> List { get; private set; } = new ObservableCollection<ListViewItemModel>();
        public ObservableCollection<IComboBoxItem> MultiList { get; } = new ObservableCollection<IComboBoxItem>();
        public TestViewModel()
        {
            for (var i = 0; i < 16; i++) List.Add(new ListViewItemModel($"{i + 1}"));
            for (var i = 0; i < 16; i++) MultiList.Add(new ComboBoxItemModel($"{i + 1}"));
            var treeInfo = new TreeViewItemModel("分类A", true);
            this.TreeList.Add(treeInfo);
            treeInfo.Add("刘棒A1");
            treeInfo.Add("刘棒A2");
            treeInfo.Add("刘棒A3");
            treeInfo.Add("刘棒A4");
            treeInfo.Add("刘棒A5");
            treeInfo = new TreeViewItemModel("分类B", true);
            this.TreeList.Add(treeInfo);
            var treeInfo2 = new TreeViewItemModel("分类C", true);
            treeInfo.Add(treeInfo2);
            treeInfo2.Add("刘棒C1");
            treeInfo2.Add("刘棒C2");
            treeInfo.Add("刘棒B1");
            treeInfo.Add("刘棒B2");
        }
    }
}