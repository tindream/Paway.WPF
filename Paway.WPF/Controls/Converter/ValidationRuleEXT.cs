using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Paway.WPF
{
    /// <summary>
    /// 输入验证
    /// </summary>
    public class ValidationRuleEXT : ValidationRule
    {
        /// <summary>
        /// 验证正则表达式
        /// </summary>
        public string Pattern { get; set; }
        /// <summary>
        /// 验证失败提示消息
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// 最小输入
        /// </summary>
        public int MinLength { get; set; }
        /// <summary>
        /// 最大输入
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = Regex.IsMatch(value.ToStrs(), Pattern, RegexOptions.IgnoreCase);
            if (!result)
            {
                return new ValidationResult(false, string.Format(ErrorMessage, value));
            }
            return new ValidationResult(true, null);
        }
    }
}
