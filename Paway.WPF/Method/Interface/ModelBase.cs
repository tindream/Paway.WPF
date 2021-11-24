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
    public class ModelBase : IId, INotifyPropertyChanged
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
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
