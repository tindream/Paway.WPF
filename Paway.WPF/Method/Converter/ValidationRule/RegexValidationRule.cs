﻿using Paway.Helper;
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
    public class RegexValidationRule : ValidationRule
    {
        /// <summary>
        /// 正则表达式类型
        /// </summary>
        public RegexType RegexType { get; set; }
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
            var validResult = new ValidationResult(true, null);

            var text = value.ToStrings();
            if (text.Length == 0)
            {
                if (this.MinLength == 0) return validResult;
                else validResult = new ValidationResult(false, PConfig.LanguageBase.CannotBeEmpty);
            }
            else if (!RegexChecked(text))
            {
                if (ErrorMessage == null) ErrorMessage = RegexType.Description();
                validResult = new ValidationResult(false, string.Format(ErrorMessage, text));
            }
            else if (this.MinLength != 0 && text.Length < this.MinLength)
            {
                validResult = new ValidationResult(false, string.Format(PConfig.LanguageBase.MinimumInputLimit, this.MinLength));
            }
            else if (this.MaxLength != 0 && text.Length > this.MaxLength)
            {
                validResult = new ValidationResult(false, string.Format(PConfig.LanguageBase.MaximumInputLimit, this.MinLength));
            }
            return validResult;
        }
        private bool RegexChecked(string text)
        {
            string pattern = Pattern;
            switch (RegexType)
            {
                case RegexType.None: return true;
                case RegexType.Custom: break;
                default:
                    pattern = $"{RegexType.Tag()}";
                    break;
            }
            return Regex.IsMatch(text, pattern, RegexOptions.Singleline);
        }
    }
}
