﻿using Paway.Helper;
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
            DependencyProperty.RegisterAttached(nameof(ItemPadding), typeof(ThicknessEXT), typeof(ListViewEXT), new PropertyMetadata(new ThicknessEXT(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessEXT), typeof(ListViewEXT), new PropertyMetadata(new ThicknessEXT(0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorderBrush), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(null, 170, 250)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent, 120, 150), OnItemBackgroundChanged));
        private static void OnItemBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListViewEXT listView)
            {
                Color? mouseColor = null, pressedColor = null;
                if (listView.ItemBackground.Mouse is SolidColorBrush mouse && mouse.Color != TMethod.AlphaColor(120, TConfig.Color))
                {
                    if ((listView.ItemBorderBrush.Mouse as SolidColorBrush).Color == TMethod.AlphaColor(170, TConfig.Color))
                    {
                        mouseColor = TMethod.AlphaColor(mouse.Color.A + 50, mouse.Color);
                    }
                }
                if (listView.ItemBackground.Pressed is SolidColorBrush pressed && pressed.Color != TMethod.AlphaColor(150, TConfig.Color))
                {
                    if ((listView.ItemBorderBrush.Pressed as SolidColorBrush).Color == TMethod.AlphaColor(250, TConfig.Color))
                    {
                        pressedColor = TMethod.AlphaColor(pressed.Color.A + 100, pressed.Color);
                    }
                }
                if (mouseColor != null || pressedColor != null)
                {
                    listView.ItemBorderBrush = new BrushEXT(null, mouseColor, pressedColor);
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
            DependencyProperty.RegisterAttached(nameof(ItemImageDock), typeof(Dock), typeof(ListViewEXT), new PropertyMetadata(Dock.Left));
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
            DependencyProperty.RegisterAttached(nameof(ItemTextForeground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(Color.FromArgb(255, 33, 33, 33), Colors.White)));
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
            DependencyProperty.RegisterAttached(nameof(ItemDescForeground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(Color.FromArgb(255, 66, 66, 66), Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemDescBackground), typeof(BrushEXT), typeof(ListViewEXT), new PropertyMetadata(new BrushEXT(Colors.Transparent)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemDescFontSizeProperty =
        DependencyProperty.RegisterAttached(nameof(ItemDescFontSize), typeof(DoubleEXT), typeof(ListViewEXT));

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
        public BrushEXT ItemBorderBrush
        {
            get { return (BrushEXT)GetValue(ItemBorderBrushProperty); }
            set { SetValue(ItemBorderBrushProperty, value); }
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
            Config_FontSizeChanged(TConfig.FontSize);
            TConfig.FontSizeChanged += Config_FontSizeChanged;
        }

        /// <summary>
        /// 更新字体大小
        /// </summary>
        private void Config_FontSizeChanged(double old)
        {
            if (this.ItemTextFontSize == null || this.ItemTextFontSize.Equals(new DoubleEXT(old)))
            {
                this.ItemTextFontSize = new DoubleEXT(TConfig.FontSize);
            }
            if (this.ItemDescFontSize == null || this.ItemDescFontSize.Equals(new DoubleEXT(old * 0.85)))
            {
                this.ItemDescFontSize = new DoubleEXT(TConfig.FontSize * 0.85);
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
                if (ClickMode == ClickMode.Release && TMethod.Parent(e.OriginalSource, out downItem))
                {
                    e.Handled = true;
                    IsPressed(true);
                }
                else if (e.ButtonState == MouseButtonState.Pressed)
                {
                    if (TMethod.Parent(this, out Window window))
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
            if (IAnimation && Mouse.DirectlyOver != null && TMethod.Parent<ListViewItem>(Mouse.DirectlyOver, out ListViewItem listViewItem) && this.moveItem != listViewItem)
            {
                if (this.moveItem != null) Animation(moveItem, false);
                this.moveItem = listViewItem;
                Animation(listViewItem, true);
            }
        }
        /// <summary>
        /// 重置动画线条位置
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            if (!IAnimation) return;
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (Items[i] is ListViewItem item)
                {
                    if (TMethod.Child(item, out Line line1, "line1", false))
                    {
                        line1.X2 = this.ItemWidth / 2;
                    }
                }
                else if (this.ItemContainerGenerator.ContainerFromItem(Items[i]) is ListViewItem listViewItem)
                {
                    if (TMethod.Child(listViewItem, out Line line1, "line1", false))
                    {
                        line1.X2 = this.ItemWidth / 2;
                    }
                }
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
                if (TMethod.Parent(e.OriginalSource, out ListViewItem item) && item == downItem)
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
                    if (item.Content is IListView info) TConfig.AddLog(this, info.Hit);
                    else if (item.Content is ContentControl content) TConfig.AddLog(this, content.Content.ToStrs());
                    else if (item.Content is TextBlock textBlock) TConfig.AddLog(this, textBlock.Text.ToStrs());
                    else TConfig.AddLog(this, item.Content.ToStrs());
                }
                item.IsSelected = value;
                Animation(item, value);
            }
        }
        private void Animation(ListViewItem item, bool value)
        {
            TMethod.Child(item, out Line line1, "line1", false);
            TMethod.Child(item, out Line line2, "line2", false);
            var storyboard = new Storyboard();
            var animTime = TMethod.AnimTime(this.ItemWidth / 2) * 0.5;
            if (value)
            {
                var animX1 = new DoubleAnimation(line1.X1, 0, new Duration(TimeSpan.FromMilliseconds(animTime)));
                var animX2 = new DoubleAnimation(line2.X2, this.ItemWidth / 2, new Duration(TimeSpan.FromMilliseconds(animTime)));
                if (line1 != null)
                {
                    if (line1.X2 > this.ActualWidth) line1.X2 = this.ActualWidth;
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X1Property));
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
                var animX1 = new DoubleAnimation(line1.X1, this.ItemWidth / 2, new Duration(TimeSpan.FromMilliseconds(animTime)));
                var animX2 = new DoubleAnimation(line2.X2, 0, new Duration(TimeSpan.FromMilliseconds(animTime)));
                if (line1 != null)
                {
                    Storyboard.SetTargetName(animX1, line1.Name);
                    Storyboard.SetTargetProperty(animX1, new PropertyPath(Line.X1Property));
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
