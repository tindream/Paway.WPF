using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Test.ViewModel
{
    public class ViewModelPlusBase : ViewModelBase
    {
        public override void RaisePropertyChanged(string propertyName = "")
        {
            if (propertyName == "") propertyName = PMethod.GetLastModelName();
            base.RaisePropertyChanged(propertyName);
        }
    }
}