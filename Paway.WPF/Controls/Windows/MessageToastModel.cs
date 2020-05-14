using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.WPF
{
    public class MessageToastModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged()
        {
            var name = Method.GetLastModelName();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private double yOffSetStart = -200;
        /// <summary>
        /// 开始帧速及方向
        /// </summary>
        public double YOffSetStart
        {
            get { return yOffSetStart; }
            set
            {
                yOffSetStart = value;
                OnPropertyChanged();
            }
        }
        private double yOffSetEnd = 200;
        /// <summary>
        /// 结束帧速及方向
        /// </summary>
        public double YOffSetEnd
        {
            get { return yOffSetEnd; }
            set
            {
                yOffSetEnd = value;
                OnPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }
    }
}
