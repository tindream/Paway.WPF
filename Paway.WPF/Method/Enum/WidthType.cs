
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 宽度样式类型
    /// </summary>
    public enum WidthType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,

        /// <summary>
        /// 单列并水平填满
        /// </summary>
        OneColumn = 21,

        /// <summary>
        /// 单列并水平填满
        /// </summary>
        OneRow = 1,

        /// <summary>
        /// 双列并水平填满
        /// </summary>
        TwoRow,

        /// <summary>
        /// 三列并水平填满
        /// </summary>
        ThreeRow,

        /// <summary>
        /// 四列并水平填满
        /// </summary>
        FoureRow,

        /// <summary>
        /// 五列并水平填满
        /// </summary>
        FiveRow,
    }
}
