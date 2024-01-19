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
    public interface IListViewItem : IId
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
        /// <summary>
        /// 获取或设置一个值，该值指示是否 用户界面 (UI) 中启用此元素。
        /// </summary>
        bool IsEnabled { get; set; }
        /// <summary>
        /// 占位标记，该值指示是该项是否可用
        /// </summary>
        bool IsNormal { get; set; }
        /// <summary>
        /// 自定义项宽度
        /// </summary>
        double ItemWidth { get; set; }
        /// <summary>
        /// 自定义项高度
        /// </summary>
        double ItemHeight { get; set; }
        /// <summary>
        /// 自定义项圆角
        /// </summary>
        RadiusEXT ItemRadius { get; set; }
        /// <summary>
        /// 自定义项文本内边距
        /// </summary>
        ThicknessEXT ItemPadding { get; set; }

        /// <summary>
        /// 自定义项文本字体颜色
        /// </summary>
        BrushEXT ItemTextForeground { get; set; }
        /// <summary>
        /// 自定义项背景颜色
        /// </summary>
        BrushEXT ItemBackground { get; set; }
        /// <summary>
        /// 自定义项外边框
        /// </summary>
        ThicknessEXT ItemBorder { get; set; }
        /// <summary>
        /// 自定义项外边距
        /// </summary>
        Thickness ItemMargin { get; set; }
        /// <summary>
        /// 自定义项外边框颜色
        /// </summary>
        BrushEXT ItemBrush { get; set; }
    }
    /// <summary>
    /// IListView数据模型
    /// </summary>
    public class ListViewItemModel : BaseModelInfo, IListViewItem
    {
        /// <summary>
        /// 数据
        /// </summary>
        [NoShow]
        public object Tag { get; set; }

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
        private bool isEnabled = true;
        /// <summary>
        /// 获取或设置一个值，该值指示是否 用户界面 (UI) 中启用此元素。
        /// </summary>
        [NoShow]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; OnPropertyChanged(); }
        }
        private bool isNormal;
        /// <summary>
        /// 占位标记，该值指示是该项是否可用
        /// </summary>
        [NoShow]
        public bool IsNormal
        {
            get { return isNormal; }
            set { isNormal = value; OnPropertyChanged(); }
        }
        private Style styleEXT;
        /// <summary>
        /// 自定义外部样式
        /// </summary>
        [NoShow]
        public Style StyleEXT
        {
            get { return styleEXT; }
            set { styleEXT = value; OnPropertyChanged(); }
        }
        private double itemWidth = double.NaN;
        /// <summary>
        /// 自定义项宽度
        /// </summary>
        [NoShow]
        public double ItemWidth
        {
            get { return itemWidth; }
            set { itemWidth = value; OnPropertyChanged(); }
        }
        private double itemHeight = double.NaN;
        /// <summary>
        /// 自定义项高度
        /// </summary>
        [NoShow]
        public double ItemHeight
        {
            get { return itemHeight; }
            set { itemHeight = value; OnPropertyChanged(); }
        }
        private RadiusEXT itemRadius;
        /// <summary>
        /// 自定义项圆角
        /// </summary>
        [NoShow]
        public RadiusEXT ItemRadius
        {
            get { return itemRadius; }
            set { itemRadius = value; OnPropertyChanged(); }
        }
        private ThicknessEXT itemPadding = new ThicknessEXT(double.NaN);
        /// <summary>
        /// 自定义项文本内边距
        /// </summary>
        [NoShow]
        public ThicknessEXT ItemPadding
        {
            get { return itemPadding; }
            set { itemPadding = value; OnPropertyChanged(); }
        }

        private BrushEXT itemTextForeground;
        /// <summary>
        /// 自定义项文本字体颜色
        /// </summary>
        [NoShow]
        public BrushEXT ItemTextForeground
        {
            get { return itemTextForeground; }
            set { itemTextForeground = value; OnPropertyChanged(); }
        }
        private BrushEXT itemBackground;
        /// <summary>
        /// 自定义项背景颜色
        /// </summary>
        [NoShow]
        public BrushEXT ItemBackground
        {
            get { return itemBackground; }
            set { itemBackground = value; OnPropertyChanged(); }
        }
        private ThicknessEXT itemBorder;
        /// <summary>
        /// 自定义项外边框
        /// </summary>
        [NoShow]
        public ThicknessEXT ItemBorder
        {
            get { return itemBorder; }
            set { itemBorder = value; OnPropertyChanged(); }
        }
        private Thickness itemMargin = new Thickness(double.NaN);
        /// <summary>
        /// 自定义项外边距
        /// </summary>
        [NoShow]
        public Thickness ItemMargin
        {
            get { return itemMargin; }
            set { itemMargin = value; OnPropertyChanged(); }
        }
        private BrushEXT itemBrush;
        /// <summary>
        /// 自定义项外边框颜色
        /// </summary>
        [NoShow]
        public BrushEXT ItemBrush
        {
            get { return itemBrush; }
            set { itemBrush = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// </summary>
        public override string ToString()
        {
            return Text;
        }
        /// <summary>
        /// </summary>
        public ListViewItemModel()
        {
            this.Id = this.GetHashCode();
        }
        /// <summary>
        /// </summary>
        public ListViewItemModel(string text, string desc = null) : this()
        {
            this.Text = text;
            this.Desc = desc;
        }
    }
}
