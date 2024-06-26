﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// CheckBox扩展-按钮样式
    /// </summary>
    public partial class CheckButton : CheckBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(CheckButton), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBorderProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBorder), typeof(ThicknessEXT), typeof(CheckButton), new PropertyMetadata(new ThicknessEXT(1, 0, 0)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(CheckButton),
                new PropertyMetadata(new BrushEXT()));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemForegroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemForeground), typeof(BrushEXT), typeof(CheckButton),
            new PropertyMetadata(new BrushEXT(null, Colors.White, Colors.White)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.RegisterAttached(nameof(Type), typeof(ColorType), typeof(CheckButton),
            new PropertyMetadata(ColorType.Color, OnColorTypeChanged));
        private static void OnColorTypeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is CheckButton view)
            {
                if (view.Type != ColorType.None)
                {
                    var color = view.Type.Color();
                    view.ItemBrush = new BrushEXT(color.ToAlpha(PConfig.Alpha))
                    {
                        Normal = new ThemeForeground(Colors.LightGray)
                    };
                }
            }
        }
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached(nameof(Title), typeof(string), typeof(CheckButton));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty TitleMinWidthProperty =
            DependencyProperty.RegisterAttached(nameof(TitleMinWidth), typeof(double), typeof(CheckButton));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：3</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 自定义边框线
        /// <para>默认值：1, 0, 0</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框线")]
        public ThicknessEXT ItemBorder
        {
            get { return (ThicknessEXT)GetValue(ItemBorderProperty); }
            set { SetValue(ItemBorderProperty, value); }
        }
        /// <summary>
        /// 自定义文本颜色
        /// <para>默认值：主题前景, White, White</para>
        /// </summary>
        [Category("扩展")]
        [Description("自定义文本颜色")]
        public BrushEXT ItemForeground
        {
            get { return (BrushEXT)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }
        /// <summary>
        /// 边框颜色
        /// <para>默认值：默认</para>
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 颜色样式
        /// <para>默认值：Color</para>
        /// </summary>
        [Category("扩展")]
        [Description("颜色样式")]
        public ColorType Type
        {
            get { return (ColorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        /// <summary>
        /// 标题
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题")]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        /// <summary>
        /// 标题最小长度
        /// <para>默认值：未设置</para>
        /// </summary>
        [Category("扩展")]
        [Description("标题最小长度")]
        public double TitleMinWidth
        {
            get { return (double)GetValue(TitleMinWidthProperty); }
            set { SetValue(TitleMinWidthProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public CheckButton()
        {
            DefaultStyleKey = typeof(CheckButton);
        }
    }
}
