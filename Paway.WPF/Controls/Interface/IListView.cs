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
    /// ListView数接口据定义
    /// </summary>
    public interface IListView : IId
    {
        /// <summary>
        /// 数据
        /// </summary>
        object Tag { get; set; }
        /// <summary>
        /// 按下状态
        /// </summary>
        bool IsPressed { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        string Text { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        string Desc { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        string Hit { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        ImageEXT Image { get; set; }
        /// <summary>
        /// 显示
        /// </summary>
        Visibility Visibility { get; set; }
        /// <summary>
        /// 选择项
        /// </summary>
        bool IsSelected { get; set; }
    }
    /// <summary>
    /// IListView数据模型
    /// </summary>
    public class ListViewModel : ModelBase, IListView
    {
        /// <summary>
        /// 数据
        /// </summary>
        [NoShow]
        public object Tag { get; set; }

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
        private bool isPressed;
        /// <summary>
        /// 按下状态
        /// </summary>
        [NoShow]
        public virtual bool IsPressed
        {
            get { return isPressed; }
            set { isPressed = value; OnPropertyChanged(); }
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
        private string desc;
        /// <summary>
        /// 描述
        /// </summary>
        [FillSize]
        public virtual string Desc
        {
            get { return desc; }
            set { desc = value; OnPropertyChanged(); }
        }
        private string hit;
        /// <summary>
        /// 提示
        /// </summary>
        [NoShow]
        public string Hit
        {
            get
            {
                if (!hit.IsEmpty()) return hit;
                if (!Text.IsEmpty()) return Text;
                if (!desc.IsEmpty()) return desc;
                return null;
            }
            set { hit = value; OnPropertyChanged(); }
        }
        private ImageEXT image;
        /// <summary>
        /// 图片
        /// </summary>
        [NoShow]
        public virtual ImageEXT Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged(); }
        }
        private Visibility visibility = Visibility.Visible;
        /// <summary>
        /// 显示
        /// </summary>
        [NoShow]
        public virtual Visibility Visibility
        {
            get { return visibility; }
            set { visibility = value; OnPropertyChanged(); }
        }
        private bool isSelected;
        /// <summary>
        /// 选择项
        /// </summary>
        [NoShow]
        public virtual bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 自定义项文本字体颜色
        /// </summary>
        [NoShow]
        public virtual BrushEXT ItemTextForeground { get; set; }
        /// <summary>
        /// 自定义项背景颜色
        /// </summary>
        [NoShow]
        public virtual BrushEXT ItemBackground { get; set; }
        /// <summary>
        /// 自定义项外边框
        /// </summary>
        [NoShow]
        public virtual BrushEXT ItemBorder { get; set; }
        /// <summary>
        /// 自定义项外边框颜色
        /// </summary>
        [NoShow]
        public virtual BrushEXT ItemBrush { get; set; }

        /// <summary>
        /// </summary>
        public ListViewModel()
        {
            this.Id = this.GetHashCode();
        }
        /// <summary>
        /// </summary>
        public ListViewModel(string text, string desc = null) : this()
        {
            this.Text = text;
            this.Desc = desc;
        }
    }
}
