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
    /// ProgressBar扩展
    /// </summary>
    public partial class ProgressBarEXT : ProgressBarBase
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(ProgressBarEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ForegroundColorLinearProperty =
            DependencyProperty.RegisterAttached(nameof(ForegroundColorLinear), typeof(ColorLinear), typeof(ProgressBarEXT),
                new PropertyMetadata(new ColorLinear()));

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
        /// 进度条线性颜色
        /// <para>默认值：默认</para>
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
        public ProgressBarEXT()
        {
            DefaultStyleKey = typeof(ProgressBarEXT);
        }

        #region 进度文本位置
        private FrameworkElement txt;
        private FrameworkElement indicator;
        private TranslateTransform txtOffset;
        /// <summary>
        /// 应用模板后挂接布局监听，用于跟随指示器摆放进度文本
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (this.indicator != null) this.indicator.SizeChanged -= Indicator_SizeChanged;
            if (this.txt != null) this.txt.SizeChanged -= Txt_SizeChanged;
            this.txt = this.GetTemplateChild("txt") as FrameworkElement;
            this.indicator = this.GetTemplateChild("PART_Indicator") as FrameworkElement;
            //无指示器的模板（如ProgressRound）不需要跟随
            if (this.txt != null && this.indicator != null)
            {
                this.txtOffset = new TranslateTransform();
                this.txt.RenderTransform = this.txtOffset;
                this.txt.SizeChanged += Txt_SizeChanged;
                this.indicator.SizeChanged += Indicator_SizeChanged;
            }
            UpdateTextOffset();
        }
        private void Indicator_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateTextOffset();
        }
        private void Txt_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateTextOffset();
        }
        /// <summary>
        /// 进度文本跟随指示器居中；指示器比文本还窄时贴左，装不下部分向右溢出显示
        /// </summary>
        private void UpdateTextOffset()
        {
            if (this.txtOffset == null || this.txt == null || this.indicator == null) return;
            var x = (this.indicator.ActualWidth - this.txt.ActualWidth) / 2;
            this.txtOffset.X = x > 0 ? x : 0;
        }

        #endregion
    }
}
