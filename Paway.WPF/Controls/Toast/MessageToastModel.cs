using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.WPF
{
    /// <summary>
    /// Window系统消息框-Toast显示-Model
    /// </summary>
    public class MessageToastModel : INotifyPropertyChanged
    {
        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// </summary>
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
            set { yOffSetStart = value; OnPropertyChanged(); }
        }
        private double yOffSetEnd = 200;
        /// <summary>
        /// 结束帧速及方向
        /// </summary>
        public double YOffSetEnd
        {
            get { return yOffSetEnd; }
            set { yOffSetEnd = value; OnPropertyChanged(); }
        }

        private string message;
        /// <summary>
        /// 显示消息
        /// </summary>
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
