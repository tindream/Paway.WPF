using Newtonsoft.Json;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Model
{
    /// <summary>
    /// 语言包定义示例
    /// </summary>
    [Serializable]
    public class LanguageInfo : ModelBase
    {
        public virtual string Test { get; set; }
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
