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
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test.ViewModel
{
    public class TestViewModel : ViewModelPlus
    {
        #region 属性
        private double _value;
        public double Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }

        #endregion
    }
}