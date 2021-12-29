using Paway.Helper;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// ListView扩展外部自定义样式
    /// </summary>
    public partial class ListViewCustom : ListView
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.RegisterAttached(nameof(Orientation), typeof(Orientation), typeof(ListViewCustom), new PropertyMetadata(Orientation.Horizontal));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ItemWidth), typeof(double), typeof(ListViewCustom), new PropertyMetadata(90d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemWidthTypeProperty =
            DependencyProperty.RegisterAttached(nameof(WidthType), typeof(WidthType), typeof(ListViewCustom));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached(nameof(ItemHeight), typeof(double), typeof(ListViewCustom), new PropertyMetadata(42d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsLightProperty =
            DependencyProperty.RegisterAttached(nameof(IsLight), typeof(bool), typeof(ListViewCustom),
            new UIPropertyMetadata(false, OnColorTypeChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.RegisterAttached(nameof(Type), typeof(ColorType), typeof(ListViewCustom),
            new UIPropertyMetadata(ColorType.None, OnColorTypeChanged));
        private static void OnColorTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ListViewCustom view)
            {
                if (view.Type != ColorType.None)
                {
                    var color = view.Type.Color();
                    view.ItemBackground = new BrushEXT(PMethod.AlphaColor(PConfig.Alpha - PConfig.Interval, color));
                }
                if (view.IsLight)
                {
                    view.ItemBorder = new ThicknessEXT(1);
                    view.ItemMargin = new Thickness(-1, -1, 0, 0);
                    view.ItemBackground.Normal = new SolidColorBrush(Colors.Transparent);
                }
                view.UpdateDefaultStyle();
            }
        }

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(ItemRadius), typeof(RadiusEXT), typeof(ListViewCustom));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ListViewCustom),
                new PropertyMetadata(new BrushEXT(Colors.Transparent, PConfig.Alpha - PConfig.Interval, PConfig.Alpha), OnItemBackgroundChanged));
        private static void OnItemBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListViewEXT listView)
            {
                Color? mouseColor = null, pressedColor = null;
                if (listView.ItemBackground.Mouse is SolidColorBrush mouse && mouse.Color != PMethod.AlphaColor(PConfig.Alpha - PConfig.Interval, PConfig.Color))
                {
                    if ((listView.ItemBrush.Mouse as SolidColorBrush).Color == PMethod.AlphaColor(PConfig.Alpha - PConfig.Interval, PConfig.Color))
                    {
                        mouseColor = PMethod.AlphaColor(mouse.Color.A + PConfig.Interval, mouse.Color);
                    }
                }
                if (listView.ItemBackground.Pressed is SolidColorBrush pressed && pressed.Color != PMethod.AlphaColor(PConfig.Alpha, PConfig.Color))
                {
                    if ((listView.ItemBrush.Pressed as SolidColorBrush).Color == PMethod.AlphaColor(PConfig.Alpha + PConfig.Interval, PConfig.Color))
                    {
                        pressedColor = PMethod.AlphaColor(pressed.Color.A + PConfig.Interval, pressed.Color);
                    }
                }
                if (mouseColor != null || pressedColor != null)
                {
                    listView.ItemBrush = new BrushEXT(null, mouseColor, pressedColor);
                }
            }
        }

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ListViewCustom),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessEXT), typeof(ListViewCustom));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.RegisterAttached(nameof(ItemMargin), typeof(Thickness), typeof(ListViewCustom));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemPadding), typeof(ThicknessEXT), typeof(ListViewCustom), new PropertyMetadata(new ThicknessEXT(5)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextForeground), typeof(BrushEXT), typeof(ListViewCustom),
                new PropertyMetadata(new BrushEXT(PConfig.TextColor, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextFontSizeProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextFontSize), typeof(DoubleEXT), typeof(ListViewCustom));

        #endregion

        #region 扩展
        /// <summary>
        /// 项显示方向
        /// </summary>
        [Category("扩展")]
        [Description("项显示方向")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        /// <summary>
        /// 普通项，不响应鼠标事件
        /// </summary>
        [Category("扩展")]
        [Description("普通项，不响应鼠标事件")]
        public bool INormal { get; set; }
        /// <summary>
        /// 指定何时应引发事件
        /// </summary>
        [Category("扩展")]
        [Description("指定何时应引发事件")]
        public ClickMode ClickMode { get; set; }
        /// <summary>
        /// 自定义项宽度
        /// </summary>
        [Category("扩展")]
        [Description("自定义项宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        /// <summary>
        /// 宽度样式
        /// </summary>
        [Category("扩展")]
        [Description("宽度样式")]
        public WidthType ItemWidthType
        {
            get { return (WidthType)GetValue(ItemWidthTypeProperty); }
            set { SetValue(ItemWidthTypeProperty, value); }
        }
        /// <summary>
        /// 自定义项高度
        /// </summary>
        [Category("扩展")]
        [Description("自定义项高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        /// <summary>
        /// 颜色样式
        /// </summary>
        [Category("扩展")]
        [Description("颜色样式")]
        public ColorType Type
        {
            get { return (ColorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary>
        /// 轻颜色样式
        /// </summary>
        [Category("扩展")]
        [Description("轻颜色样式")]
        public bool IsLight
        {
            get { return (bool)GetValue(IsLightProperty); }
            set { SetValue(IsLightProperty, value); }
        }
        /// <summary>
        /// 自定义项圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义项圆角")]
        public RadiusEXT ItemRadius
        {
            get { return (RadiusEXT)GetValue(ItemRadiusProperty); }
            set { SetValue(ItemRadiusProperty, value); }
        }
        /// <summary>
        /// 自定义项外边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("自定义项外边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
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
        /// 自定义项外边框
        /// </summary>
        [Category("扩展")]
        [Description("自定义项外边框")]
        public ThicknessEXT ItemBorder
        {
            get { return (ThicknessEXT)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        /// <summary>
        /// 自定义项外边距
        /// </summary>
        [Category("扩展")]
        [Description("自定义项外边距")]
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
        /// <summary>
        /// 自定义项内边距
        /// </summary>
        [Category("扩展")]
        [Description("自定义项内边距")]
        public ThicknessEXT ItemPadding
        {
            get { return (ThicknessEXT)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }
        /// <summary>
        /// 自定义项文本字体大小
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本字体大小")]
        public DoubleEXT ItemTextFontSize
        {
            get { return (DoubleEXT)GetValue(ItemTextFontSizeProperty); }
            set { SetValue(ItemTextFontSizeProperty, value); }
        }
        /// <summary>
        /// 自定义项文本字体颜色
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本字体颜色")]
        public BrushEXT ItemTextForeground
        {
            get { return (BrushEXT)GetValue(ItemTextForegroundProperty); }
            set { SetValue(ItemTextForegroundProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ListViewCustom()
        {
            DefaultStyleKey = typeof(ListViewCustom);
            Config_FontSizeChanged(PConfig.FontSize);
            PConfig.FontSizeChanged += Config_FontSizeChanged;
            this.SelectionChanged += ListViewCustom_SelectionChanged;
        }
        private void ListViewCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var temp in e.AddedItems)
            {
                if (temp is ListViewItem item && item.Content is IListViewItem info)
                {
                    PConfig.AddLog(this, info.Hit);
                }
                else if (this.ItemContainerGenerator.ContainerFromItem(temp) is ListViewItem listViewItem && listViewItem.Content is IListViewItem info2)
                {
                    PConfig.AddLog(this, info2.Hit);
                }
            }
        }
        /// <summary>
        /// 更新字体大小
        /// </summary>
        protected virtual void Config_FontSizeChanged(double old)
        {
            if (this.ItemTextFontSize == null || this.ItemTextFontSize.Equals(new DoubleEXT(old)))
            {
                this.ItemTextFontSize = new DoubleEXT(PConfig.FontSize);
            }
        }
        /// <summary>
        /// 等宽处理
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            var actualWidth = ActualWidth - BorderThickness.Left - BorderThickness.Right - Padding.Left - Padding.Right;
            switch (ItemWidthType)
            {
                case WidthType.OneColumn:
                    for (var i = 0; i < Items.Count; i++)
                    {
                        IListViewItem item = null;
                        if (Items[i] is IListViewItem temp) item = temp;
                        else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is IListViewItem listViewItem) item = listViewItem;
                        if (item != null)
                        {
                            item.ItemWidth = actualWidth;
                            if (IsLight)
                            {
                                if (i == 0) item.ItemMargin = new Thickness(0);
                                else item.ItemMargin = new Thickness(0, -1, 0, 0);
                            }
                        }
                    }
                    break;
                case WidthType.OneRow:
                case WidthType.TwoRow:
                case WidthType.ThreeRow:
                case WidthType.FoureRow:
                case WidthType.FiveRow:
                    var rowCount = (int)ItemWidthType;
                    var columnCount = Items.Count / rowCount;
                    var width = (int)(actualWidth / columnCount);
                    if (width < 0) width = 0;
                    if (actualWidth % columnCount > 0) width++;
                    var count = columnCount - (columnCount * width - actualWidth);
                    var margin = ItemMargin.Left + ItemMargin.Right;
                    for (var i = 0; i < Items.Count;)
                    {
                        for (var j = 0; j < columnCount && i < Items.Count; j++, i++)
                        {
                            IListViewItem item = null;
                            if (Items[i] is IListViewItem temp) item = temp;
                            else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is IListViewItem listViewItem) item = listViewItem;
                            if (item != null)
                            {
                                item.ItemWidth = (count > j ? width : width - 1) - margin;
                                if (IsLight)
                                {
                                    if (i == 0 && j == 0) item.ItemMargin = new Thickness(0);
                                    else if (i < columnCount) item.ItemMargin = new Thickness(-1, 0, 0, 0);
                                    else if (j == 0) item.ItemMargin = new Thickness(0, -1, 0, 0);
                                    if (j == 0) item.ItemWidth += margin;
                                }
                            }
                        }
                    }
                    break;
                default:
                    if (!IsLight) break;
                    margin = ItemMargin.Left + ItemMargin.Right;
                    var totalWidth = actualWidth;
                    var iFirst = true;
                    for (var i = 0; i < Items.Count; i++)
                    {
                        IListViewItem item = null;
                        if (Items[i] is IListViewItem temp) item = temp;
                        else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is IListViewItem listViewItem) item = listViewItem;
                        if (item != null)
                        {
                            var itemWidth = item.ItemWidth;
                            if (itemWidth.Equals(double.NaN)) itemWidth = this.ItemWidth;
                            itemWidth += margin;
                            totalWidth -= itemWidth;
                            if (i == 0)
                            {
                                item.ItemMargin = new Thickness(0);
                                itemWidth -= margin;
                                totalWidth += margin;
                            }
                            else if (iFirst && totalWidth >= 0)
                            {
                                item.ItemMargin = new Thickness(-1, 0, 0, 0);
                            }
                            else
                            {
                                iFirst = false;
                                if (totalWidth < 0)
                                {
                                    item.ItemMargin = new Thickness(0, -1, 0, 0);
                                    totalWidth = actualWidth - itemWidth;
                                }
                            }
                        }
                    }
                    break;
            }
            base.OnRenderSizeChanged(sizeInfo);
        }

        #region 按下状态
        /// <summary>
        /// 鼠标移过项
        /// </summary>
        protected ListViewItem moveItem;
        /// <summary>
        /// 按下时的item
        /// </summary>
        private ListViewItem downItem;
        /// <summary>
        /// 鼠标按下时取消触发
        /// </summary>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (INormal)
            {
                e.Handled = true;
                return;
            }
            downItem = null;
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (ClickMode == ClickMode.Release && PMethod.Parent(e.OriginalSource, out downItem))
                {
                    IsPressed(true);
                }
                else if (PMethod.Parent(this, out Window window))
                {
                    window.DragMove();
                }
            }
        }
        private void IsPressed(bool value)
        {
            if (downItem.Content is IListViewItem model)
            {
                if (model.IsPressed != value)
                {
                    model.IsPressed = value;
                }
            }
        }
        /// <summary>
        /// 判断按下状态
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (ClickMode == ClickMode.Release && downItem != null)
            {
                IsPressed(false);
                downItem = null;
            }
        }
        /// <summary>
        /// 鼠标抬起时判断触发
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (ClickMode == ClickMode.Release && e.ButtonState == MouseButtonState.Pressed && downItem != null)
            {
                IsPressed(false);
            }
        }

        #endregion
    }
}
