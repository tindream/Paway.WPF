using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ComboMulti数接口据定义
    /// </summary>
    public interface IComboBoxItem : IId
    {
        /// <summary>
        /// 文本
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// 选中标记
        /// </summary>
        bool IsChecked { get; set; }
    }
    /// <summary>
    /// IComboMulti数据模型
    /// </summary>
    public class ComboBoxItemModel : BaseModelInfo, IComboBoxItem
    {
        private string text;
        /// <summary>
        /// 文本
        /// </summary>
        public virtual string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
        }
        private bool isChecked;
        /// <summary>
        /// 选中标记
        /// </summary>
        [NoShow]
        public virtual bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return Text;
        }
        /// <summary>
        /// </summary>
        public ComboBoxItemModel() { }
        /// <summary>
        /// </summary>
        public ComboBoxItemModel(string text) : this()
        {
            this.Text = text;
        }
    }
}
