
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 颜色类型
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// 信息
        /// </summary>
        [Color(0, 188, 212)]
        Info,

        /// <summary>
        /// 成功
        /// </summary>
        [Color(45, 184, 77)]
        Success,

        /// <summary>
        /// 警告
        /// </summary>
        [Color(255, 153, 0)]
        Warn,

        /// <summary>
        /// 错误
        /// </summary>
        [Color(248, 51, 30)]
        Error,

        /// <summary>
        /// 默认
        /// </summary>
        [Color(35, 175, 255)]
        Normal,

        /// <summary>
        /// 浅色
        /// </summary>
        [Color(235, 235, 235)]
        Light,

        /// <summary>
        /// 深色
        /// </summary>
        [Color(0, 63, 99)]
        High,
    }
}
