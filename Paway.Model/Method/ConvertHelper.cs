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

namespace Paway.Model
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 字符串转int列表
        /// </summary>
        public static List<int> ToIdList(this string ids)
        {
            if (ids.IsEmpty()) return new List<int>();
            return ids.Split(',').ToList().ConvertAll(c => c.ToInt());
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
