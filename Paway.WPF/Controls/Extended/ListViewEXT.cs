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
    /// ListView扩展
    /// </summary>
    public partial class ListViewEXT : ListView
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IAnimationProperty =
            DependencyProperty.RegisterAttached(nameof(IAnimation), typeof(bool), typeof(ListViewEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.RegisterAttached(nameof(Orientation), typeof(Orientation), typeof(ListViewEXT), new PropertyMetadata(Orientation.Horizontal));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty INormalProperty =
            DependencyProperty.RegisterAttached(nameof(INormal), typeof(bool), typeof(ListViewEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ItemWidth), typeof(double), typeof(ListViewEXT), new PropertyMetadata(90d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemWidthTypeProperty =
            DependencyProperty.RegisterAttached(nameof(WidthType), typeof(WidthType), typeof(ListViewEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.RegisterAttached(nameof(ItemHeight), typeof(double), typeof(ListViewEXT), new PropertyMetadata(42d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemRadiusProperty =
            DependencyProperty.RegisterAttached(nameof(ItemRadius), typeof(RadiusEXT), typeof(ListViewEXT), new PropertyMetadata(new RadiusEXT(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.RegisterAttached(nameof(ItemMargin), typeof(Thickness), typeof(ListViewEXT), new PropertyMetadata(new Thickness(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemPadding), typeof(ThicknessEXT), typeof(ListViewEXT), new PropertyMetadata(new ThicknessEXT(5)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessEXT), typeof(ListViewEXT), new PropertyMetadata(new ThicknessEXT(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(null, 170, 240)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StyleEXTProperty =
            DependencyProperty.RegisterAttached(nameof(StyleEXT), typeof(Style), typeof(ListViewEXT));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent, 120, 210), OnItemBackgroundChanged));
        private static void OnItemBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListViewEXT listView)
            {
                Color? mouseColor = null, pressedColor = null;
                if (listView.ItemBackground.Mouse is SolidColorBrush mouse && mouse.Color != PMethod.AlphaColor(120, PConfig.Color))
                {
                    if ((listView.ItemBrush.Mouse as SolidColorBrush).Color == PMethod.AlphaColor(170, PConfig.Color))
                    {
                        mouseColor = PMethod.AlphaColor(mouse.Color.A + 50, mouse.Color);
                    }
                }
                if (listView.ItemBackground.Pressed is SolidColorBrush pressed && pressed.Color != PMethod.AlphaColor(150, PConfig.Color))
                {
                    if ((listView.ItemBrush.Pressed as SolidColorBrush).Color == PMethod.AlphaColor(240, PConfig.Color))
                    {
                        pressedColor = PMethod.AlphaColor(pressed.Color.A + 100, pressed.Color);
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
        public static readonly DependencyProperty ItemImageWidthProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageWidth), typeof(double), typeof(ListViewEXT), new PropertyMetadata(32d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageHeightProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageHeight), typeof(double), typeof(ListViewEXT), new PropertyMetadata(32d));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageDockProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageDock), typeof(Dock), typeof(ListViewEXT), new PropertyMetadata(Dock.Top));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemImageMarginProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageMargin), typeof(ThicknessEXT), typeof(ListViewEXT), new PropertyMetadata(new ThicknessEXT(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.RegisterAttached(nameof(ItemImageStretch), typeof(Stretch), typeof(ListViewEXT),
            new PropertyMetadata(Stretch.None));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextPadding), typeof(Thickness), typeof(ListViewEXT), new PropertyMetadata(new Thickness(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextForeground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(PConfig.TextColor, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextBackground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemTextFontSizeProperty =
            DependencyProperty.RegisterAttached(nameof(ItemTextFontSize), typeof(DoubleEXT), typeof(ListViewEXT));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescDockProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescDock), typeof(Dock), typeof(ListViewEXT), new PropertyMetadata(Dock.Right));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescPaddingProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescPadding), typeof(Thickness), typeof(ListViewEXT), new PropertyMetadata(new Thickness(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescForeground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(PConfig.LightText, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescBackground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescFontSizeProperty =
        DependencyProperty.RegisterAttached(nameof(ItemDescFontSize), typeof(DoubleEXT), typeof(ListViewEXT));

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IsLightProperty =
            DependencyProperty.RegisterAttached(nameof(IsLight), typeof(bool), typeof(ListViewEXT),
            new UIPropertyMetadata(false, OnColorTypeChanged));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.RegisterAttached(nameof(Type), typeof(ColorType), typeof(ListViewEXT),
            new UIPropertyMetadata(ColorType.None, OnColorTypeChanged));
        private static void OnColorTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ListViewEXT view)
            {
                if (view.Type != ColorType.None)
                {
                    var color = view.Type.Color();
                    view.ItemBackground = new BrushEXT(PMethod.AlphaColor(160, color));
                }
                if (view.IsLight)
                {
                    view.BorderThickness = new Thickness(1, 1, 0, 0);
                    view.BorderBrush = new SolidColorBrush(Colors.LightGray);
                    view.ItemBorder = new ThicknessEXT(new Thickness(0, 0, 1, 1));
                    view.ItemBackground.Normal = new SolidColorBrush(Colors.Transparent);
                }
                view.UpdateDefaultStyle();
            }
        }

        #endregion

        #region 扩展.项
        /// <summary>
        /// 动画
        /// </summary>
        [Category("扩展.项")]
        [Description("动画")]
        public bool IAnimation
        {
            get { return (bool)GetValue(IAnimationProperty); }
            set { SetValue(IAnimationProperty, value); }
        }
        /// <summary>
        /// 项显示方向
        /// </summary>
        [Category("扩展.项")]
        [Description("项显示方向")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        /// <summary>
        /// 普通项，不响应鼠标事件
        /// </summary>
        [Category("扩展.项")]
        [Description("普通项，不响应鼠标事件")]
        public bool INormal
        {
            get { return (bool)GetValue(INormalProperty); }
            set { SetValue(INormalProperty, value); }
        }
        /// <summary>
        /// 自定义项宽度
        /// </summary>
        [Category("扩展.项")]
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
        [Category("扩展.项")]
        [Description("宽度样式")]
        public WidthType ItemWidthType
        {
            get { return (WidthType)GetValue(ItemWidthTypeProperty); }
            set { SetValue(ItemWidthTypeProperty, value); }
        }
        /// <summary>
        /// 自定义项高度
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        /// <summary>
        /// 自定义项圆角
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项圆角")]
        public RadiusEXT ItemRadius
        {
            get { return (RadiusEXT)GetValue(ItemRadiusProperty); }
            set { SetValue(ItemRadiusProperty, value); }
        }
        /// <summary>
        /// 自定义项外边距
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项外边距")]
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }
        /// <summary>
        /// 自定义项内边距
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项内边距")]
        public ThicknessEXT ItemPadding
        {
            get { return (ThicknessEXT)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }
        /// <summary>
        /// 自定义项外边框
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项外边框")]
        public ThicknessEXT ItemBorder
        {
            get { return (ThicknessEXT)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        /// <summary>
        /// 自定义项外边框颜色
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项外边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 自定义外部样式
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义外部样式")]
        public Style StyleEXT
        {
            get { return (Style)GetValue(StyleEXTProperty); }
            set { SetValue(StyleEXTProperty, value); }
        }
        /// <summary>
        /// 自定义项背景颜色
        /// </summary>
        [Category("扩展.项")]
        [Description("自定义项背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }
        /// <summary>
        /// 指定何时应引发事件
        /// </summary>
        [Category("扩展.项")]
        [Description("指定何时应引发事件")]
        public ClickMode ClickMode { get; set; }
        /// <summary>
        /// 颜色样式
        /// </summary>
        [Category("扩展.项")]
        [Description("颜色样式")]
        public ColorType Type
        {
            get { return (ColorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary>
        /// 轻颜色样式
        /// </summary>
        [Category("扩展.项")]
        [Description("轻颜色样式")]
        public bool IsLight
        {
            get { return (bool)GetValue(IsLightProperty); }
            set { SetValue(IsLightProperty, value); }
        }

        #endregion
        #region 扩展.项图片
        /// <summary>
        /// 自定义项图片宽度
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片宽度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ItemImageWidth
        {
            get { return (double)GetValue(ItemImageWidthProperty); }
            set { SetValue(ItemImageWidthProperty, value); }
        }
        /// <summary>
        /// 自定义项图片高度
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片高度")]
        [TypeConverter(typeof(LengthConverter))]
        public double ItemImageHeight
        {
            get { return (double)GetValue(ItemImageHeightProperty); }
            set { SetValue(ItemImageHeightProperty, value); }
        }
        /// <summary>
        /// 自定义项图片位置
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片位置")]
        public Dock ItemImageDock
        {
            get { return (Dock)GetValue(ItemImageDockProperty); }
            set { SetValue(ItemImageDockProperty, value); }
        }
        /// <summary>
        /// 自定义项图片外边距
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片外边距")]
        public ThicknessEXT ItemImageMargin
        {
            get { return (ThicknessEXT)GetValue(ItemImageMarginProperty); }
            set { SetValue(ItemImageMarginProperty, value); }
        }
        /// <summary>
        /// 背景图片的内容如何拉伸才适合其磁贴
        /// </summary>
        [Category("扩展.项图片")]
        [Description("自定义项图片的内容如何拉伸才适合其磁贴")]
        public Stretch ItemImageStretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        #endregion
        #region 扩展.项文本
        /// <summary>
        /// 自定义项文本内边距
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本内边距")]
        public Thickness ItemTextPadding
        {
            get { return (Thickness)GetValue(ItemTextPaddingProperty); }
            set { SetValue(ItemTextPaddingProperty, value); }
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
        /// <summary>
        /// 自定义项文本背景颜色
        /// </summary>
        [Category("扩展.项文本")]
        [Description("自定义项文本背景颜色")]
        public BrushEXT ItemTextBackground
        {
            get { return (BrushEXT)GetValue(ItemTextBackgroundProperty); }
            set { SetValue(ItemTextBackgroundProperty, value); }
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

        #endregion
        #region 扩展.项描述
        /// <summary>
        /// 自定义项描述位置
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述位置")]
        public Dock ItemDescDock
        {
            get { return (Dock)GetValue(ItemDescDockProperty); }
            set { SetValue(ItemDescDockProperty, value); }
        }
        /// <summary>
        /// 自定义项描述内边距
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述内边距")]
        public Thickness ItemDescPadding
        {
            get { return (Thickness)GetValue(ItemDescPaddingProperty); }
            set { SetValue(ItemDescPaddingProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体颜色
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushEXT ItemDescForeground
        {
            get { return (BrushEXT)GetValue(ItemDescForegroundProperty); }
            set { SetValue(ItemDescForegroundProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体颜色
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体颜色")]
        public BrushEXT ItemDescBackground
        {
            get { return (BrushEXT)GetValue(ItemDescBackgroundProperty); }
            set { SetValue(ItemDescBackgroundProperty, value); }
        }
        /// <summary>
        /// 自定义项描述字体大小
        /// </summary>
        [Category("扩展.项描述")]
        [Description("自定义项描述字体大小")]
        public DoubleEXT ItemDescFontSize
        {
            get { return (DoubleEXT)GetValue(ItemDescFontSizeProperty); }
            set { SetValue(ItemDescFontSizeProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ListViewEXT()
        {
            DefaultStyleKey = typeof(ListViewEXT);
            Config_FontSizeChanged(PConfig.FontSize);
            PConfig.FontSizeChanged += Config_FontSizeChanged;
            this.SizeChanged += ListViewEXT_SizeChanged;
        }

        /// <summary>
        /// 等宽处理
        /// </summary>
        private void ListViewEXT_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ItemWidthType == WidthType.None) return;
            var actualWidth = ActualWidth - BorderThickness.Left - BorderThickness.Right - Padding.Left - Padding.Right;
            switch (ItemWidthType)
            {
                case WidthType.WidthFill:
                    var width = (int)(actualWidth / Items.Count);
                    if (width < 0) width = 0;
                    if (actualWidth % this.Items.Count > 0) width++;
                    var count = Items.Count - (Items.Count * width - actualWidth);
                    for (var i = 0; i < Items.Count; i++)
                    {
                        if (Items[i] is IListView item)
                        {
                            item.ItemWidth = count > i ? width : width - 1;
                        }
                        else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is IListView listViewItem)
                        {
                            listViewItem.ItemWidth = count > i ? width : width - 1;
                        }
                    }
                    break;
                case WidthType.OneColumn:
                    for (var i = 0; i < Items.Count; i++)
                    {
                        if (Items[i] is IListView item)
                        {
                            item.ItemWidth = actualWidth;
                        }
                        else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is IListView listViewItem)
                        {
                            listViewItem.ItemWidth = actualWidth;
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 更新字体大小
        /// </summary>
        private void Config_FontSizeChanged(double old)
        {
            if (this.ItemTextFontSize == null || this.ItemTextFontSize.Equals(new DoubleEXT(old)))
            {
                this.ItemTextFontSize = new DoubleEXT(PConfig.FontSize);
            }
            if (this.ItemDescFontSize == null || this.ItemDescFontSize.Equals(new DoubleEXT(old * 0.85)))
            {
                this.ItemDescFontSize = new DoubleEXT(PConfig.FontSize * 0.85);
            }
        }

        #region 指定按钮按下并释放时，应引发事件。
        /// <summary>
        /// 第一次按下时的item序号(Shift)
        /// </summary>
        private int firstIndex = -1;
        /// <summary>
        /// 鼠标移过项
        /// </summary>
        private ListViewItem moveItem;
        /// <summary>
        /// 上一次按下时的item
        /// </summary>
        private ListViewItem lastItem;
        /// <summary>
        /// 按下时的item
        /// </summary>
        private ListViewItem downItem;
        /// <summary>
        /// 鼠标按下时取消触发
        /// </summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            if (INormal)
            {
                e.Handled = true;
                return;
            }
            downItem = null;
            if (e.ChangedButton == MouseButton.Left)
            {
                if (ClickMode == ClickMode.Release && PMethod.Parent(e.OriginalSource, out downItem))
                {
                    e.Handled = true;
                    IsPressed(true);
                }
                else if (e.ButtonState == MouseButtonState.Pressed)
                {
                    if (PMethod.Parent(this, out Window window))
                    {
                        window.DragMove();
                    }
                }
            }
            else e.Handled = true;
        }
        private void IsPressed(bool value)
        {
            if (downItem.Content is IListView model)
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
                if (e.LeftButton == MouseButtonState.Released)
                {
                    IsPressed(false);
                    downItem = null;
                }
            }
            if (IAnimation && Mouse.DirectlyOver != null && PMethod.Parent<ListViewItem>(Mouse.DirectlyOver, out ListViewItem listViewItem) && this.moveItem != listViewItem)
            {
                if (this.moveItem != null) Animation(moveItem, false);
                this.moveItem = listViewItem;
                Animation(listViewItem, true);
            }
        }
        /// <summary>
        /// 鼠标离开控件
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (IAnimation && this.moveItem != null)
            {
                Animation(moveItem, false);
                this.moveItem = null;
            }
        }

        /// <summary>
        /// 鼠标抬起时判断触发
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (ClickMode == ClickMode.Release && e.ChangedButton == MouseButton.Left && downItem != null)
            {
                IsPressed(false);
                if (PMethod.Parent(e.OriginalSource, out ListViewItem item) && item == downItem)
                {
                    if (SelectionMode == SelectionMode.Extended)
                    {
                        if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift)
                        {
                            if (firstIndex == -1)
                            {
                                firstIndex = Index(item);
                                SetSelected(item, true);
                            }
                            else
                            {
                                var index = Index(item);
                                var min = Math.Min(firstIndex, index);
                                var max = Math.Max(firstIndex, index);
                                Selected(i => i >= min && i <= max);
                            }
                        }
                        else
                        {
                            firstIndex = Index(item);
                            if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
                            {
                                SetSelected(item, !item.IsSelected);
                            }
                            else
                            {
                                Selected(i => false);
                                Selected(i => i == Index(item));
                            }
                        }
                    }
                    else if (SelectionMode == SelectionMode.Multiple)
                    {
                        SetSelected(item, !item.IsSelected);
                    }
                    else
                    {
                        if (lastItem != null && lastItem != item) SetSelected(lastItem, false);
                        SetSelected(item, true);
                        lastItem = item;
                    }
                }
            }
        }
        private void Selected(Func<int, bool> action)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (Items[i] is ListViewItem item)
                {
                    SetSelected(item, action(i));
                }
                else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is ListViewItem listViewItem)
                {
                    SetSelected(listViewItem, action(i));
                }
            }
        }
        private void SetSelected(ListViewItem item, bool value)
        {
            if (item.IsSelected != value)
            {
                if (value)
                {
                    this.Focus();
                    if (item.Content is IListView info) PConfig.AddLog(this, info.Hit);
                    else if (item.Content is ContentControl content) PConfig.AddLog(this, content.Content.ToStrings());
                    else if (item.Content is TextBlock textBlock) PConfig.AddLog(this, textBlock.Text.ToStrings());
                    else PConfig.AddLog(this, item.Content.ToStrings());
                }
                item.IsSelected = value;
                Animation(item, value);
            }
        }
        private void Animation(ListViewItem item, bool value)
        {
            PMethod.Child(item, out Line line1, "line1", false);
            PMethod.Child(item, out Line line2, "line2", false);
            var storyboard = new Storyboard();
            var animTime = PMethod.AnimTime(this.ItemWidth / 2) * 0.5;
            if (value)
            {
                var itemWidth = (int)(item.ActualWidth / 2);
                var animX1 = new DoubleAnimation(line1.X2, itemWidth, new Duration(TimeSpan.FromMilliseconds(animTime)));
                var animX2 = new DoubleAnimation(line2.X2, itemWidth, new Duration(TimeSpan.FromMilliseconds(animTime)));
                if (line1 != null)
                {
                    if (line1.X2 > this.ActualWidth) line1.X2 = this.ActualWidth;
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            else
            {
                var animX1 = new DoubleAnimation(line1.X2, 0, new Duration(TimeSpan.FromMilliseconds(animTime)));
                var animX2 = new DoubleAnimation(line2.X2, 0, new Duration(TimeSpan.FromMilliseconds(animTime)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX1);
                }
                if (line2 != null)
                {
                    Storyboard.SetTargetName(animX2, line2.Name);
                    Storyboard.SetTargetProperty(animX2, new PropertyPath(Line.X2Property));
                    storyboard.Children.Add(animX2);
                }
            }
            if (line1 != null) storyboard.Begin(line1);
        }
        private int Index(ListViewItem item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is ListViewItem listViewItem)
                {
                    if (item.Equals(listViewItem)) return i;
                }
            }
            return -1;
        }

        #endregion
    }
}
