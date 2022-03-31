using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paway.WPF
{
    /// <summary>
    /// 过渡类型
    /// </summary>
    public enum TransitionType
    {
        /// <summary>
        /// Default
        /// </summary>
        [Description(PConfig.None)]
        None,
        /// <summary>
        /// 渐入
        /// <para>使控件立即进行透明度从0至1的渐变动画。若控件尚未加载完成，则将在其加载完成后再执行动画。</para>
        /// </summary>
        [Description("渐入")]
        FadeIn,
        /// <summary>
        /// 渐出
        /// </summary>
        [Description("渐出")]
        FadeOut,
        /// <summary>
        /// 从左侧滑入
        /// <para>使控件立即进行从相对左侧偏移一个控件宽度的位置，移动至当前位置的渐变动画。若控件尚未加载完成，则将在其加载完成后再执行动画。</para>
        /// </summary>
        [Description("从左侧滑入")]
        Left,
        /// <summary>
        /// 从右侧滑入
        /// <para>使控件立即进行从相对右侧偏移一个控件宽度的位置，移动至当前位置的渐变动画。若控件尚未加载完成，则将在其加载完成后再执行动画。</para>
        /// </summary>
        [Description("从右侧滑入")]
        Right,
        /// <summary>
        /// 从顶部滑入
        /// <para>使控件立即进行从相对顶部偏移一个控件高度的位置，移动至当前位置的渐变动画。若控件尚未加载完成，则将在其加载完成后再执行动画。</para>
        /// </summary>
        [Description("从顶部滑入")]
        Top,
        /// <summary>
        /// 从底部滑入
        /// <para>使控件立即进行从相对底部偏移一个控件高度的位置，移动至当前位置的渐变动画。若控件尚未加载完成，则将在其加载完成后再执行动画。</para>
        /// </summary>
        [Description("从底部滑入")]
        Bottom,
        /// <summary>
        /// 滑入左侧
        /// </summary>
        [Description("滑入左侧")]
        ToLeft,
        /// <summary>
        /// 滑入右侧
        /// </summary>
        [Description("滑入右侧")]
        ToRight,
        /// <summary>
        /// 滑入顶部
        /// </summary>
        [Description("滑入顶部")]
        ToTop,
        /// <summary>
        /// 滑入底部
        /// </summary>
        [Description("滑入底部")]
        ToBottom,
        /// <summary>
        /// 淡入淡出
        /// </summary>
        [Description("淡入淡出")]
        Opacity,
        /// <summary>
        /// 宽度(拉抽屉)
        /// </summary>
        [Description("宽度")]
        Width,
        /// <summary>
        /// 高度
        /// </summary>
        [Description("高度")]
        Height,
    }
}
