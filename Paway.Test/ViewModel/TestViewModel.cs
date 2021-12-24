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
        public ObservableCollection<ListViewItemModel> List { get; private set; } = new ObservableCollection<ListViewItemModel>();

        private MonitorType userType = MonitorType.LeftToeAngle;
        public MonitorType UserType
        {
            get { return userType; }
            set { userType = value; RaisePropertyChanged(); }
        }

        #endregion

        public TestViewModel()
        {
            for (var i = 0; i < 5; i++) List.Add(new ListViewItemModel($"{i + 1}"));
        }
    }
}