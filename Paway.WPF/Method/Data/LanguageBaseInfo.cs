using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.WPF
{
    /// <summary>
    /// 语言包定义
    /// </summary>
    [Serializable]
    public class LanguageBaseInfo : ModelBase
    {
        /// <summary>
        /// 请输入
        /// </summary>
        [Description("请输入")]
        public virtual string PleaseInput { get; set; } = "请输入";
        /// <summary>
        /// 请输入
        /// </summary>
        [Description("请输入(水印)")]
        public virtual string PleaseInputWater { get; set; } = "请输入..";
        /// <summary>
        /// 请输入密码
        /// </summary>
        [Description("请输入密码(水印)")]
        public virtual string PleaseInputPasswordWater { get; set; } = "请输入密码";
        /// <summary>
        /// 今天
        /// </summary>
        [Description("今天")]
        public virtual string Today { get; set; } = "今天";
        /// <summary>
        /// 清空
        /// </summary>
        [Description("清空")]
        public virtual string Clear { get; set; } = "清空";
        /// <summary>
        /// 不可为空
        /// </summary>
        [Description("不可为空")]
        public virtual string CannotBeEmpty { get; set; } = "不可为空";
        /// <summary>
        /// 最小输入限制: {0}位
        /// </summary>
        [Description("最小输入限制: {0}位")]
        public virtual string MinimumInputLimit { get; set; } = "最小输入限制: {0}位";
        /// <summary>
        /// 最大输入限制: {0}位
        /// </summary>
        [Description("最大输入限制: {0}位")]
        public virtual string MaximumInputLimit { get; set; } = "最大输入限制: {0}位";
    }
    /// <summary>
    /// 注入拦截器类-触发更新事件
    /// </summary>
    public class InterceptorNotify
    {
        /// <summary>
        /// 注入拦截器类-触发更新事件
        /// </summary>
        public object Invoke(object @object, string @method, object[] parameters)
        {
            var retobj = @object.GetType().GetMethod(@method + "_Base").Invoke(@object, parameters);
            if (@method.StartsWith("set_"))
            {
                if (@object is INotify notify) notify.OnPropertyChanged(@method.Substring(4));
            }
            return retobj;
        }
    }
}
