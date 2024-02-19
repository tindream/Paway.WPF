using Paway.Helper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Paway.WPF
{
    /// <summary>
    /// ProgressBar扩展-圆形
    /// </summary>
    public partial class ProgressRound : ProgressBarBase
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundColorLinear), typeof(ColorLinear), typeof(ProgressRound),
                new PropertyMetadata(new ColorLinear(Colors.LightGray, PMethod.ThemeColor())));

        #endregion

        #region 扩展
        /// <summary>
        /// 进度条线性颜色
        /// <para>默认值：LightGray, 主题色</para>
        /// </summary>
        [Category("扩展")]
        [Description("进度条线性颜色")]
        public ColorLinear ForegroundColorLinear
        {
            get { return (ColorLinear)GetValue(ForegroundColorLinearProperty); }
            set { SetValue(ForegroundColorLinearProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public ProgressRound()
        {
            DefaultStyleKey = typeof(ProgressRound);
        }
        /// <summary>
        /// 锁定长宽比
        /// </summary>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            this.Height = this.ActualWidth;
            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}
