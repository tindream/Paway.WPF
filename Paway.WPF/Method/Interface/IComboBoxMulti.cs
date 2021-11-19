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
    public interface IComboMulti : IId
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
    public class ComboMultiModel : ModelBase, IComboMulti
    {
        private int id;
        /// <summary>
        /// 标识符
        /// </summary>
        [NoShow]
        public virtual int Id
        {
            get { return id; }
            set { id = value; OnPropertyChanged(); }
        }
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
        public ComboMultiModel()
        {
            this.Id = this.GetHashCode();
        }
        /// <summary>
        /// </summary>
        public ComboMultiModel(string text) : this()
        {
            this.Text = text;
        }
    }
}
