
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
        /// </summary>
        Text,
        /// <summary>
        /// 文本(二级浅色)
        /// </summary>
        TextSub,
        /// <summary>
        /// 文本(三级浅色)
        /// </summary>
        TextLight,

        /// <summary>
        /// 浅色
        /// </summary>
        Light,
    }
}
