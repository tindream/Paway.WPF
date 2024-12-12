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
    /// 双输入框通用窗体模型
    /// </summary>
    public class ValueWindowModel : BaseWindowModel
    {
        #region 属性_名称输入框
        private string _labelName = "名称:";
        /// <summary>
        /// 名称输入框Label
        /// </summary>
        public string LabelName
        {
            get { return _labelName; }
            set { _labelName = value; OnPropertyChanged(); }
        }
        private string _name;
        /// <summary>
        /// 名称输入框绑定值
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        #endregion
        #region 属性_值输入框
        private string _labelValue = "值:";
        /// <summary>
        /// 值输入框1Label
        /// </summary>
        public string LabelValue
        {
            get { return _labelValue; }
            set { _labelValue = value; OnPropertyChanged(); }
        }
        private string _value;
        /// <summary>
        /// 值输入框绑定值
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); }
        }

        private int _maxLength = 32;
        /// <summary>
        /// 值输入框最大输入长度
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; OnPropertyChanged(); }
        }
        private TextWrapping _textWrapping = TextWrapping.NoWrap;
        /// <summary>
        /// 值输入框换行
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
            if (!PMethod.ValidationError(wd, this, nameof(Name))) return null;
            if (!PMethod.ValidationError(wd, this, nameof(Value))) return null;
            return base.OnCommit(wd);
        }
        /// <summary>
        /// 双输入框通用窗体模型
        /// </summary>
        public ValueWindowModel() { }
    }
}