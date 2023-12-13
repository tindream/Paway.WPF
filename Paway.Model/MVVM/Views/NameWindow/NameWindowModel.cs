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
    /// <summary>
    /// 单输入框通用窗体模型
    /// </summary>
    public class NameWindowModel : BaseWindowModel
    {
        #region 属性
        private string _label = "名称:";
        /// <summary>
        /// 输入框Label
        /// </summary>
        public string Label
        {
            get { return _label; }
            set { _label = value; OnPropertyChanged(); }
        }

        private string _name;
        /// <summary>
        /// 输入框绑定值
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        #endregion

        /// <summary>
        /// 单输入框通用窗体模型
        /// </summary>
        public NameWindowModel() { }
    }
}