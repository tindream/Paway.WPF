
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 颜色样式类型
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 主题颜色
        /// </summary>
        Color,
        /// <summary>
        /// 主题深色
        /// </summary>
        High,

        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 文本
        /// <para>34</para>
        /// </summary>
        Text,
        /// <summary>
        /// 文本(二级浅色)
        /// <para>68</para>
        /// <para>Light:30</para>
        /// </summary>
        TextSub,
        /// <summary>
        /// 文本(三级浅色)
        /// <para>119</para>
        /// <para>Light:80</para>
        /// </summary>
        TextLight,

        /// <summary>
        /// 浅色(一级)
        /// <para>220</para>
        /// <para>Light:180</para>
        /// </summary>
        Border,
        /// <summary>
        /// 浅色(二级)
        /// <para>228</para>
        /// <para>Light:190</para>
        /// </summary>
        BorderSub,
        /// <summary>
        /// 浅色
        /// <para>242</para>
        /// <para>Light:200</para>
        /// </summary>
        Light,
    }
}
