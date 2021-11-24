using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 模型接口
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// 描述
        /// </summary>
        string Desc();
    }
    /// <summary>
    /// INotifyPropertyChanged接口基类
    /// </summary>
    public class ModelBase : IId, IModel, INotifyPropertyChanged
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [NoShow, NoExcel]
        public int Id { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Desc() { return null; }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 自动实现
        /// </summary>
        public void OnPropertyChanged()
        {
            OnPropertyChanged(PMethod.GetLastModelName());
        }
        /// <summary>
        /// 手动引发
        /// </summary>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
