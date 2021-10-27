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
    /// INotifyPropertyChanged接口基类
    /// </summary>
    public class ModelBase : INotifyPropertyChanged
    {
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
