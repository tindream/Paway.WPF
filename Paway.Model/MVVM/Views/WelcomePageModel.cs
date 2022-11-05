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
    public class WelcomePageModel : ViewModelBase
    {
        #region 属性
        private string _desc = "欢迎使用";
        public string Desc
        {
            get { return _desc; }
            set { _desc = value; RaisePropertyChanged(); }
        }

        #endregion

        public WelcomePageModel() { }
    }
}