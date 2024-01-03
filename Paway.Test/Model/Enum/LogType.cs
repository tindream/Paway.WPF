using Paway.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.Test
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        None,
        [Description("测试")]
        Test,
        [Description("登录")]
        Login,
        [Description("登录失败")]
        LoginError,
    }
}
