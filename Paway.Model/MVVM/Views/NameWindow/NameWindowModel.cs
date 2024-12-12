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
        private string _text;
        /// <summary>
        /// 输入框绑定值
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { _text = value; OnPropertyChanged(); }
        }

        private int _maxLength = 32;
        /// <summary>
        /// 输入框最大输入长度
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; OnPropertyChanged(); }
        }
        private TextWrapping _textWrapping = TextWrapping.NoWrap;
        /// <summary>
        /// 输入框换行
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return _textWrapping; }
            set { _textWrapping = value; OnPropertyChanged(); }
        }

        #endregion

        /// <summary>
        /// 提交检查
        /// </summary>
        public override bool? OnCommit(Window wd)
        {
            if (!PMethod.ValidationError(wd, this, nameof(Text))) return null;
            return base.OnCommit(wd);
        }
        /// <summary>
        /// 单输入框通用窗体模型
        /// </summary>
        public NameWindowModel() { }
    }
}