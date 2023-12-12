using Newtonsoft.Json;
using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯相关的一些帮助方法
    /// </summary>
    public static class ConvertHelper
    {
        /// <summary>
        /// 从HTTP内容中获取原始请求地址
        /// </summary>
        public static string IpAddress(this HttpListenerContext context)
        {
            return context.Request.Headers["X-Forwarded-For"];
        }
        /// <summary>
        /// 字符串转int列表
        /// </summary>
        public static List<int> ToIdList(this string ids)
        {
            if (ids.IsEmpty()) return new List<int>();
            return ids.Split(',').ToList().ConvertAll(c => c.ToInt()).FindAll(c => c > 0);
        }
        /// <summary>
        /// 字符串转string列表
        /// </summary>
        public static List<string> ToStrList(this string ids)
        {
            if (ids.IsEmpty()) return new List<string>();
            return ids.Replace("，", ",").Split(',').ToList();
        }
    }
}
