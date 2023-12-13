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
    /// 欢迎页模型
    /// </summary>
    public class WelcomePageModel : ViewModelBasePlus
    {
        #region 属性
        private string _desc = "欢迎使用";
        /// <summary>
        /// 欢迎语
        /// </summary>
        public string Desc
        {
            get { return _desc; }
            set { _desc = value; OnPropertyChanged(); }
        }

        #endregion

        /// <summary>
        /// 欢迎页模型
        /// </summary>
        public WelcomePageModel() { }
    }
}