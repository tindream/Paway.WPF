using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class ModelBase : IId, INotify
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [NoShow, NoExcel]
        public virtual int Id { get; set; }
        /// <summary>
        /// 当前对象的描述
        /// </summary>
        public override string ToString()
        {
            return $"{Id}";
        }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 触发更新
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 触发更新
        /// </summary>
        public void OnPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 触发更新
        /// </summary>
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
    /// <summary>
    /// INotifyPropertyChanged接口扩展
    /// </summary>
    public interface INotify : INotifyPropertyChanged
    {
        /// <summary>
        /// 触发更新
        /// </summary>
        void OnPropertyChanged([CallerMemberName] string propertyName = null);
        /// <summary>
        /// 触发更新
        /// </summary>
        void OnPropertyChanged(PropertyChangedEventArgs e);
    }
}
