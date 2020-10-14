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
        #region INotifyPropertyChanged
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

        #endregion

        #region 依赖扩展
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextForeground), typeof(BrushEXT), typeof(ListBoxItemEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ListBoxItemEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent), OnItemBackgroundChanged));
        private static void OnItemBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBoxItemEXT listBoxItem)
            {
                Color? mouseColor = null, pressedColor = null;
                if (listBoxItem.ItemBackground.Mouse is SolidColorBrush mouse && mouse.Color != Colors.Transparent)
                {
                    if ((listBoxItem.ItemBorderBrush.Mouse as SolidColorBrush).Color == Colors.Transparent)
                    {
                        mouseColor = Method.AlphaColor(mouse.Color.A + 50, mouse.Color);
                    }
                }
                if (listBoxItem.ItemBackground.Pressed is SolidColorBrush pressed && pressed.Color != Colors.Transparent)
                {
                    if ((listBoxItem.ItemBorderBrush.Pressed as SolidColorBrush).Color == Colors.Transparent)
                    {
                        pressedColor = Method.AlphaColor(pressed.Color.A + 100, pressed.Color);
                    }
                }
                if (mouseColor != null || pressedColor != null)
                {
                    listBoxItem.ItemBorderBrush = new BrushEXT(null, mouseColor, pressedColor);
                }
            }
        }
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorderBrush), typeof(BrushEXT), typeof(ListBoxItemEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent)));

        /// <summary>
        /// 自定义项文本字体颜色
        /// </summary>
        [Category("扩展")]
        [Description("自定义项文本字体颜色")]
        public BrushEXT ItemTextForeground
        {
            get { return (BrushEXT)GetValue(ItemTextForegroundProperty); }
            set { SetValue(ItemTextForegroundProperty, value); }
        }
        /// <summary>
        /// 自定义项背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("自定义项背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }
        /// <summary>
        /// 自定义项外边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("自定义项外边框颜色")]
        public BrushEXT ItemBorderBrush
        {
            get { return (BrushEXT)GetValue(ItemBorderBrushProperty); }
            set { SetValue(ItemBorderBrushProperty, value); }
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

        #endregion

        /// <summary>
        /// 标识符
        /// </summary>
        public int Id { get; set; }
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
        /// </summary>
        public ListBoxItemEXT() { }
    }
}
