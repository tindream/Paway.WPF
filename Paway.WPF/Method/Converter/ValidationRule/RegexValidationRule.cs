using Paway.Helper;
using System;
using System.Collections.Concurrent;
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
                //原实现这里传的是 MinLength，“最大输入限制”永远显示最小值
                validResult = new ValidationResult(false, string.Format(PConfig.LanguageBase.MaximumInputLimit, this.MaxLength));
            }
            return validResult;
        }
        private bool RegexChecked(string text)
        {
            if (RegexType == RegexType.None) return true;
            string pattern = Pattern;
            if (RegexType != RegexType.Custom) pattern = $"{RegexType.Tag()}";
            //未配置表达式（RegexType.Custom 但 Pattern 为空）时不做限制，原实现会抛 ArgumentNullException
            if (string.IsNullOrEmpty(pattern)) return true;
            try
            {
                //校验发生在每次键入时：缓存编译结果，并给回溯设上限，避免灾难性回溯冻结UI线程
                var regex = regexCache.GetOrAdd(pattern,
                    p => new Regex(p, RegexOptions.Singleline, TimeSpan.FromMilliseconds(RegexTimeout)));
                return regex.IsMatch(text);
            }
            catch (RegexMatchTimeoutException ex)
            {
                ex.Log("正则校验超时");
                return false;
            }
            catch (ArgumentException ex)
            {
                //表达式本身写错属配置问题，不应让用户输入全部判为非法
                ex.Log("正则表达式无效");
                return true;
            }
        }
        private const int RegexTimeout = 200;
        private static readonly ConcurrentDictionary<string, Regex> regexCache = new ConcurrentDictionary<string, Regex>();
    }
}
