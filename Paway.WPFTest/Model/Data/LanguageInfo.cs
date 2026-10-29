using Newtonsoft.Json;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 语言包定义
    /// </summary>
    [Serializable]
    public class LanguageInfo : LanguageBaseInfo
    {
        [Description("测试")]
        public virtual string Test { get; set; } = "测试";
    }
}
