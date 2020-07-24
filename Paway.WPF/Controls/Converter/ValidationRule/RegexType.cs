using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.WPF
{
    /// <summary>
    /// 预置的正则表达式
    /// </summary>
    public enum RegexType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 自定义
        /// </summary>
        [Description("输入错误")]
        Custom,

        /// <summary>
        /// 一般规则，不允许特殊字符
        /// 允许中文汉字+符号+密码规则
        /// </summary>
        [Description("不允许特殊字符")]
        [Tag(@"^[\u0022\u0391-\uFFE5\r\n a-zA-Z0-9`=\'\-\\\[\] ;,./~!@#$%^&*()_+|{}:<>?]{0,}$")]
        Normal,
        /// <summary>
        /// 密码
        /// 不允许单引号(')
        /// </summary>
        [Description("不允许特殊字符")]
        [Tag(@"^[\u0022a-zA-Z0-9`=\-\\\[\] ;,./~!@#$%^&*()_+|{}:<>?]{0,}$")]
        Password,
        /// <summary>
        /// IP地址
        /// </summary>
        [Description("IP地址输入错误")]
        [Tag(@"^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$")]
        Ip,
        /// <summary>
        /// 正整数
        /// </summary>
        [Description("正整数输入错误")]
        [Tag("^[0-9]*[1-9][0-9]*$")]
        UInt,
        /// <summary>
        /// 数字
        /// </summary>
        [Description("数字输入错误")]
        [Tag("^[1-9]+[0-9]*([{0}][0-9]+)?|[0-9]([.,][0-9]+)?$")]
        Number
    }
}
