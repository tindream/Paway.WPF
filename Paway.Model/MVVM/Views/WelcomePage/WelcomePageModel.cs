using CommunityToolkit.Mvvm.ComponentModel;
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
        private string _title = "欢迎使用";
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }
        private string _subject;
        /// <summary>
        /// 副标题
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChanged(); }
        }

        #endregion

        /// <summary>
        /// 欢迎页模型
        /// </summary>
        public WelcomePageModel() { }
    }
}