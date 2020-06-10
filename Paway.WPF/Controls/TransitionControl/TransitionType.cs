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
        [Description("Default")]
        Default,
        /// <summary>
        /// Left
        /// </summary>
        [Description("Left")]
        Left,
        /// <summary>
        /// Up
        /// </summary>
        [Description("Up")]
        Up,
        /// <summary>
        /// Right
        /// </summary>
        [Description("Right")]
        Right,
        /// <summary>
        /// Down
        /// </summary>
        [Description("Down")]
        Down,
    }
}
