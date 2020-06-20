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
    /// 带进度条的Path按钮
    /// </summary>
    public partial class ButtonPath : Button
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
                DependencyProperty.RegisterAttached(nameof(Value), typeof(double), typeof(ButtonPath), new PropertyMetadata(0d, OnValueChanged));
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is ButtonPath btn)
            {
                btn.IWarn = btn.Value < 25;
            }
        }

        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.RegisterAttached(nameof(BorderColor), typeof(Brush), typeof(ButtonPath),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 200, 200, 200))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BorderFocusedColorProperty =
            DependencyProperty.RegisterAttached(nameof(BorderFocusedColor), typeof(Brush), typeof(ButtonPath),
            new PropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 35, 175, 255))));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IWarnProperty =
            DependencyProperty.RegisterAttached(nameof(IWarn), typeof(bool), typeof(ButtonPath));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IEmptyProperty =
            DependencyProperty.RegisterAttached(nameof(IEmpty), typeof(bool), typeof(ButtonPath));

        #endregion

        #region 扩展
        /// <summary>
        /// 内部进度条的值
        /// </summary>
        [Category("扩展")]
        [Description("内部进度条的值")]
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        /// <summary>
        /// 默认的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("默认的边框颜色")]
        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
        /// <summary>
        /// 鼠标移过时的边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("鼠标移过时的边框颜色")]
        public Brush BorderFocusedColor
        {
            get { return (Brush)GetValue(BorderFocusedColorProperty); }
            set { SetValue(BorderFocusedColorProperty, value); }
        }
        /// <summary>
        /// 余量低报警
        /// </summary>
        [Browsable(false)]
        [Description("余量低报警")]
        public bool IWarn
        {
            get { return (bool)GetValue(IWarnProperty); }
            set { SetValue(IWarnProperty, value); }
        }
        /// <summary>
        /// 余量空报警
        /// </summary>
        [Browsable(false)]
        [Description("余量空报警")]
        public bool IEmpty
        {
            get { return (bool)GetValue(IEmptyProperty); }
            set { SetValue(IEmptyProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ButtonPath()
        {
            DefaultStyleKey = typeof(ButtonPath);
        }
    }
}
