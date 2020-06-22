using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// ListBoxItem扩展
    /// </summary>
    public partial class ListBoxItemEXT : ListBoxItem, IListView, INotifyPropertyChanged
    {
        /// <summary>
        /// 标识符
        /// </summary>
        public int Id { get; set; }
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
        private bool isPressed;
        /// <summary>
        /// 按下状态
        /// </summary>
        [Browsable(false)]
        public bool IsPressed
        {
            get { return isPressed; }
            set { isPressed = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return base.Content.ToStrs(); }
            set { base.Content = value; OnPropertyChanged(); }
        }
        private string desc;
        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return desc; }
            set { desc = value; OnPropertyChanged(); }
        }
        private ImageEXT image;
        /// <summary>
        /// 图片
        /// </summary>
        public ImageEXT Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 选中状态
        /// </summary>
        public new bool IsSelected
        {
            get { return base.IsSelected; }
            set { base.IsSelected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// </summary>
        public ListBoxItemEXT() { }
    }
}
