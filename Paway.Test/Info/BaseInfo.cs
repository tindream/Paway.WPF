using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// Info基类
    /// </summary>
    [Serializable]
    public class BaseInfo : IId, INotifyPropertyChanged, IModel
    {
        [NoShow]
        [NoExcel]
        public int Id { get; set; }
        public virtual string Desc() { return null; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            OnPropertyChanged(Method.GetLastModelName());
        }
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
