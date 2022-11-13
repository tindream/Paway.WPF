using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Comm
{
    public partial class Method : Model.Method
    {
        #region HTTP同步
        /// <summary>
        /// 接收数据，JSON转实体，更新数据
        /// </summary>
        public static void Sync(SyncMessage msg)
        {
            if (msg.IType == null || msg.List == null) return;
            if (msg.List.Count == 0 && msg.OperType != OperType.Reset) return;
            bool isJson = msg.List.GenericType() == typeof(object) || msg.List.GenericType() == typeof(JObject);
            {
                var list = !isJson ? msg.List.Clone(true) : JsonToIList(msg.List, msg.IType);
                if (Cache.Dic.ContainsKey(msg.IType))
                {
                    lock (Cache.Dic[msg.IType])
                    {
                        Method.Update(msg.OperType, Cache.Dic[msg.IType], list);
                        Method.Sorted(Cache.Dic[msg.IType]);
                    }
                }
            }
        }
        /// <summary>
        /// Json->List
        /// </summary>
        public static IList JsonToIList(IList json, Type type)
        {
            if (json.GenericType() == type) return json;
            var list = type.GenericList();
            foreach (var item in json) list.Add(JsonConvert.DeserializeObject(item.ToString(), type));
            return list;
        }
        public static Exception HttpError(WebException ex, bool iDecompress = true)
        {
            string str;
            if (ex.Response == null)
            {
                str = ex.Message();
            }
            else
            {
                var stream = ex.Response.GetResponseStream();
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    str = reader.ReadToEnd();
                    if (iDecompress) str = str.Decompress();
                }
            }
            $"WebApi错误：{str}".Log(LeveType.Error);
            return new Exception(str);
        }

        #endregion
    }
}
