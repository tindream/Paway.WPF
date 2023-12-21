using Paway.Helper;
using System;
using System.Collections.Specialized;
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
            DependencyProperty.RegisterAttached(nameof(ItemWidthType), typeof(ItemWidthType), typeof(ListViewCustom));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.RegisterAttached(nameof(Row), typeof(int), typeof(ListViewCustom), new PropertyMetadata(1));
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
                    view.ItemBackground = new BrushEXT(Colors.Transparent, PMethod.AlphaColor(PConfig.Alpha - PConfig.Interval, color), PMethod.AlphaColor(PConfig.Alpha, color));
                }
                if (view.IsLight)
                {
                    if (view.ItemBorder == null || view.ItemBorder.Equals(new ThicknessEXT(0))) view.ItemBorder = new ThicknessEXT(1);
                    if (view.ItemBorder != null) view.ItemMargin = new Thickness(-view.ItemBorder.Normal.Left, -view.ItemBorder.Normal.Top, 0, 0);
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
            if (d is ListViewCustom listView)
            {
                Color? mouseColor = null, pressedColor = null;
                if (listView.ItemBackground.Mouse is SolidColorBrush mouse && mouse.Color != PMethod.AlphaColor(PConfig.Alpha - PConfig.Interval, PConfig.Color))
                {
                    if (listView.ItemBrush.Mouse.ToColor() == PMethod.AlphaColor(PConfig.Alpha, PConfig.Color))
                    {
                        mouseColor = PMethod.AlphaColor(mouse.Color.A, mouse.Color);
                    }
                }
                if (listView.ItemBackground.Pressed is SolidColorBrush pressed && pressed.Color != PMethod.AlphaColor(PConfig.Alpha, PConfig.Color))
                {
                    if (listView.ItemBrush.Pressed.ToColor() == PMethod.AlphaColor(PConfig.Alpha + PConfig.Interval, PConfig.Color))
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
                new PropertyMetadata(new BrushEXT(null, PConfig.Alpha, PConfig.Alpha + PConfig.Interval)));
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
            DependencyProperty.RegisterAttached(nameof(ItemTextFontSize), typeof(DoubleEXT), typeof(ListViewCustom), new PropertyMetadata(new DoubleEXT(PConfig.FontSize)));

        #endregion

        #region 扩展
        /// <summary>
        /// 项显示方向
        /// <para>默认值：Horizontal</para>
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
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("普通项，不响应鼠标事件")]
        public bool INormal { get; set; }
        /// <summary>
        /// 移动项，兼容移动
        /// <para>默认值：false</para>
        /// </summary>
        [Category("扩展")]
        [Description("移动项，兼容移动")]
        public bool IMove { get; set; }
        /// <summary>
        /// 指定何时应引发事件
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("指定何时应引发事件")]
        public ClickMode ClickMode { get; set; }
        /// <summary>
        /// 自定义项宽度
        /// <para>默认值：90</para>
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
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("宽度样式")]
        public ItemWidthType ItemWidthType
        {
            get { return (ItemWidthType)GetValue(ItemWidthTypeProperty); }
            set { SetValue(ItemWidthTypeProperty, value); }
        }
        /// <summary>
        /// 行数
        /// <para>默认值：1</para>
        /// </summary>
        [Category("扩展")]
        [Description("行数")]
        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }
        /// <summary>
        /// 自定义项高度
        /// <para>默认值：42</para>
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
        /// <para>默认值：None</para>
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
        /// <para>默认值：false</para>
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
        /// <para>默认值：未设置</para>
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
        /// <para>默认值：null, 200, 240</para>
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
        /// <para>默认值：Transparent, 160, 200</para>
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
        /// <para>默认值：未设置</para>
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
        /// <para>默认值：未设置</para>
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
        /// <para>默认值：5</para>
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
        /// <para>默认值：主题字体大小</para>
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
        /// <para>默认值：TextColor, White</para>
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本字体颜色")]
        public BrushEXT ItemTextForeground
        {
            get { return (BrushEXT)GetValue(ItemTextForegroundProperty); }
            set { SetValue(ItemTextForegroundProperty, value); }
        }

        #endregion

        #region 点击空白处抛出路由事件
        /// <summary>
        /// 点击空白处抛出路由事件
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> PreviewMouseClick;
        /// <summary>
        /// 点击空白处抛出路由事件
        /// </summary>
        private void OnPreviewMouseClick(MouseButtonEventArgs e)
        {
            PreviewMouseClick?.Invoke(this, e);
        }

        #endregion
        #region 点击项抛出路由事件
        /// <summary>
        /// 点击项抛出路由事件
        /// </summary>
        public event EventHandler<MouseButtonEventArgs> PreviewItemClick;
        /// <summary>
        /// 点击项抛出路由事件
        /// </summary>
        private void OnPreviewItemClick(MouseButtonEventArgs e)
        {
            PreviewItemClick?.Invoke(this, e);
        }

        #endregion

        /// <summary>
        /// </summary>
        public ListViewCustom()
        {
            DefaultStyleKey = typeof(ListViewCustom);
            PConfig.FontSizeChanged += Config_FontSizeChanged;
            this.SelectionChanged += ListViewCustom_SelectionChanged;
        }
        private ScrollViewerEXT ScrollViewer;
        /// <summary>
        /// 获取内部控件-滚动条
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ScrollViewer = Template.FindName("Part_ScrollViewer", this) as ScrollViewerEXT;
        }
        private void ListViewCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var temp in e.RemovedItems)
            {
                if (temp is ListViewItem item && item.Content is IListViewItem info)
                {
                    info.IsSelected = false;
                }
                else if (this.ItemContainerGenerator.ContainerFromItem(temp) is ListViewItem listViewItem && listViewItem.Content is IListViewItem info2)
                {
                    info2.IsSelected = false;
                }
            }
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
            this.ReleaseMouseCapture();
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
            ResetItemWidth();
            base.OnRenderSizeChanged(sizeInfo);
        }
        /// <summary>
        /// 等宽处理
        /// </summary>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            ResetItemWidth();
            base.OnItemsChanged(e);
        }
        /// <summary>
        /// 重置大小
        /// </summary>
        public void ResetItemWidth()
        {
            var actualWidth = ActualWidth - BorderThickness.Left - BorderThickness.Right - Padding.Left - Padding.Right;
            var margin = ItemMargin.Left + ItemMargin.Right;
            switch (ItemWidthType)
            {
                case ItemWidthType.OneColumn:
                    if (margin < 0) margin = 0;
                    for (var i = 0; i < Items.Count; i++)
                    {
                        IListViewItem item = null;
                        if (Items[i] is IListViewItem temp) item = temp;
                        else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is IListViewItem listViewItem) item = listViewItem;
                        if (item != null)
                        {
                            item.ItemWidth = actualWidth - margin;
                            if (IsLight)
                            {
                                if (i == 0) item.ItemMargin = new Thickness(0);
                                else item.ItemMargin = new Thickness(0, ItemMargin.Top, 0, 0);
                            }
                        }
                    }
                    break;
                case ItemWidthType.CustomRow:
                    if (Row < 1) Row = 1;
                    var columnCount = Items.Count / Row;
                    if (Items.Count % Row > 0) columnCount++;
                    var width = (int)(actualWidth / columnCount);
                    if (actualWidth % columnCount > 0) width++;
                    var count = columnCount - (columnCount * width - actualWidth);
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
                                    else if (i < columnCount) item.ItemMargin = new Thickness(ItemMargin.Left, 0, 0, 0);
                                    else if (j == 0) item.ItemMargin = new Thickness(0, ItemMargin.Top, 0, 0);
                                    if (j == 0) item.ItemWidth += margin;
                                }
                            }
                        }
                    }
                    break;
                default:
                    if (!IsLight) break;
                    var totalWidth = actualWidth;
                    var iFirst = true;
                    for (var i = 0; i < Items.Count; i++)
                    {
                        IListViewItem item = null;
                        if (Items[i] is IListViewItem temp) item = temp;
                        else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is IListViewItem listViewItem) item = listViewItem;
                        if (item != null && item.Visibility != Visibility.Collapsed)
                        {
                            var itemWidth = item.ItemWidth;
                            if (itemWidth.Equals(double.NaN)) itemWidth = this.ItemWidth;
                            itemWidth += margin;
                            totalWidth -= itemWidth;
                            if (i == 0)
                            {
                                item.ItemMargin = new Thickness(0);
                                totalWidth += margin;
                            }
                            else if (iFirst && totalWidth >= 0)
                            {
                                item.ItemMargin = new Thickness(ItemMargin.Left, 0, 0, 0);
                            }
                            else
                            {
                                iFirst = false;
                                if (totalWidth < 0)
                                {
                                    item.ItemMargin = new Thickness(0, ItemMargin.Top, 0, 0);
                                    itemWidth -= margin;
                                    totalWidth = actualWidth - itemWidth;
                                }
                                else
                                {
                                    item.ItemMargin = new Thickness(double.NaN);
                                }
                            }
                        }
                    }
                    break;
            }
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
        /// 鼠标按下前判断触发
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
            {
                e.Handled = true;
                return;
            }
            downItem = null;
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                var eventArg = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent,
                    Source = this
                };
                if (PMethod.Parent(e.OriginalSource, out downItem))
                {
                    if (INormal)
                    {
                        downItem = null;
                        e.Handled = true;
                    }
                    else
                    {
                        IsPressed(true);
                        if (ClickMode == ClickMode.Press || SelectionMode != SelectionMode.Single)
                        {
                            OnPreviewItemClick(e);
                            this.ReleaseMouseCapture();
                        }
                        else
                        {
                            e.Handled = true;
                        }
                    }
                    if (IMove)
                    {
                        PMethod.BeginInvoke(() =>
                        {
                            if (ScrollViewer != null && (ScrollViewer.ScrollableHeight > 0 || ScrollViewer.ScrollableWidth > 0))
                            {
                                ScrollViewer.RaiseEvent(eventArg);
                            }
                            else ToMove(eventArg);
                        });
                    }
                }
                else if (ScrollViewer != null && (ScrollViewer.ScrollableHeight > 0 || ScrollViewer.ScrollableWidth > 0)) { }
                else ToMove(eventArg);
            }
            base.OnPreviewMouseDown(e);
        }
        private void ToMove(MouseButtonEventArgs eventArg)
        {
            if (PMethod.Parent(this, out Window window))
            {
                OnPreviewMouseClick(eventArg);
                if (!eventArg.Handled && (bool)window.GetValue(WindowMonitor.IsDragMoveEnabledProperty)) window.DragMove();
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
        private void IsSelected(bool value)
        {
            if (downItem.Content is IListViewItem model)
            {
                if (model.IsSelected != value)
                {
                    model.IsSelected = value;
                }
            }
        }
        /// <summary>
        /// 鼠标移动取消按下状态
        /// </summary>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (downItem != null)
            {
                IsPressed(false);
            }
            base.OnPreviewMouseMove(e);
        }
        /// <summary>
        /// 鼠标离开取消按下状态
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (downItem != null)
            {
                IsPressed(false);
            }
            base.OnMouseLeave(e);
        }
        /// <summary>
        /// 鼠标抬起时判断触发
        /// </summary>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            if (downItem != null)
            {
                IsPressed(false);
                if (ClickMode == ClickMode.Release && SelectionMode == SelectionMode.Single)
                {
                    var point = Mouse.GetPosition(this);
                    var obj = this.InputHitTest(point);
                    if (PMethod.Parent(obj, out ListViewItem viewItem) && viewItem.Equals(downItem))
                    {
                        IsSelected(true);
                    }
                }
                downItem = null;
            }
            base.OnPreviewMouseUp(e);
        }

        #endregion

        #region 抛出滚动事件
        /// <summary>
        /// 当控件无需滚动时，抛出滚动事件
        /// </summary>
        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            if (ScrollViewer.ScrollableHeight == 0 && ScrollViewer.ScrollableWidth == 0)
            {
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent,
                    Source = this
                };
                this.RaiseEvent(eventArg);
            }
            base.OnPreviewMouseWheel(e);
        }

        #endregion
    }
}
