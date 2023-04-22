using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
using Paway.WPF;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Paway.Model
{
    public class NameWindowModel : BaseWindowModel
    {
        #region 属性
        private string _label = "名称:";
        public string Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        #endregion

        public NameWindowModel() { }
    }
}