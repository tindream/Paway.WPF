using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Comm
{
    /// <summary>
    /// 通讯相关的一些常用方法
    /// </summary>
    public partial class CMethod : TMethod
    {
        #region HTTP同步
        /// <summary>
        /// 接收数据，JSON转实体，更新数据
        /// </summary>
        public static IList Sync(SyncMessage msg)
        {
            if (msg.IType == null || msg.List == null) return null;
            if (msg.List.Count == 0 && msg.OperType != OperType.Reset) return null;
            bool isJson = msg.List.GenericType() == typeof(object) || msg.List.GenericType() == typeof(JObject);
            {
                var list = !isJson ? msg.List.Clone(true) : JsonToIList(msg.List, msg.IType);
                if (Cache.Dic.ContainsKey(msg.IType))
                {
                    CMethod.Update(msg.OperType, Cache.Dic[msg.IType], list);
                }
                return list;
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
        /// <summary>
        /// HTTP通讯错误解析
        /// </summary>
        public static Exception HttpError(WebException ex, bool iDecompress = false, [CallerMemberName] string memberName = null)
        {
            var msg = HttpErrorMessage(ex, iDecompress, memberName);
            return new WebException($"{memberName}失败：{msg}");
        }
        private static string HttpErrorMessage(WebException ex, bool iDecompress, string memberName)
        {
            string msg;
            if (ex.Response == null)
            {
                msg = ex.Message();
            }
            else
            {
                var stream = ex.Response.GetResponseStream();
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    msg = reader.ReadToEnd();
                    if (iDecompress) msg = msg.Decompress();
                }
            }
            $"[{memberName}]WebApi错误：{msg}".Log(LeveType.Error);
            return msg;
        }

        #endregion

        #region 缓存同步
        /// <summary>
        /// 同步更新
        /// <para>插入、更新操作后会自动排序</para>
        /// </summary>
        public static T Update<T>(T info) where T : class, IId
        {
            Update(new List<T> { info });
            return Cache.Find<T>(info.Id);
        }
        /// <summary>
        /// 同步更新
        /// <para>插入、更新操作后会自动排序</para>
        /// </summary>
        public static void Update<T>(List<T> fList) where T : class, IId
        {
            Update(typeof(T), fList);
        }
        /// <summary>
        /// 同步更新
        /// <para>插入、更新操作后会自动排序</para>
        /// </summary>
        public static void Update(Type type, IList fList)
        {
            CMethod.Update(OperType.Update, Cache.AllList(type), fList);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete<T>(T info) where T : class, IId
        {
            CMethod.Update(OperType.Delete, Cache.AllList<T>(), info);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete<T>(List<T> fList) where T : class, IId
        {
            Delete(typeof(T), fList);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete(Type type, IList fList)
        {
            CMethod.Update(OperType.Delete, Cache.AllList(type), fList);
        }

        #endregion

        #region File
        /// <summary>
        /// 读取文件，返回 Base64 数字编码的等效字符串表示形式。
        /// </summary>
        public static string ReadFile(string file, out int length)
        {
            var buffer = ReadFileBuffer(file, out length);
            var str = Convert.ToBase64String(buffer);
            return str;
        }
        /// <summary>
        /// 读取文件，返回流字节列表
        /// </summary>
        public static byte[] ReadFileBuffer(string file, out int length)
        {
            using (var fs = File.OpenRead(file))
            {
                var buffer = new byte[fs.Length];
                length = fs.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
        /// <summary>
        /// 将 Base64 字符串 保存到文件。
        /// </summary>
        public static void SaveFile(string file, string str)
        {
            var buffer = Convert.FromBase64String(str);
            SaveFile(file, buffer);
        }
        /// <summary>
        /// 将 byte[] 字节流 保存到文件。
        /// </summary>
        public static void SaveFile(string file, byte[] buffer)
        {
            using (var fs = new FileStream(file, FileMode.OpenOrCreate))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion

        #region 去除HTML标记
        private static string[] AryReg ={
                @"<style[^>]*?>.*?</style>",
                @"<script[^>]*?>.*?</script>",
                @"<(\/\s*)?!?((\w+:)?\w+)(\w+[-]?\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
                @"([\r\n])[\s]+",
                @"&(ldquo|rdquo|quot|#34);",
                @"&(amp|#38);",
                @"&(lt|#60);",
                @"&(gt|#62);",
                @"&(nbsp|#160);",
                @"&(iexcl|#161);",
                @"&(cent|#162);",
                @"&(pound|#163);",
                @"&(copy|#169);",
                @"&#(\d+);",
                @"-->",
                @"<!--.*\n"
            };
        private static string[] AryRep = {
                "",
                "",
                "",
                "",
                "\"",
                "&",
                "<",
                ">",
                " ",
                "\xa1",//chr(161),
                "\xa2",//chr(162),
                "\xa3",//chr(163),
                "\xa9",//chr(169),
                "",
                "\r\n",
                ""
            };
        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="html">包括HTML的源码</param>
        /// <returns>已经去除后的文字</returns>
        public static string HTMLString(string html)
        {
            if (html.IsEmpty()) return html;
            string strOutput = html;
            for (int i = 0; i < AryReg.Length; i++)
            {
                var regex = new Regex(AryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, AryRep[i]);
            }
            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");

            return strOutput;
        }

        #endregion
    }
}
