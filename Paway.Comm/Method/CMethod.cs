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
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Comm
{
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
            CMethod.Update(OperType.Update, Cache.List(type), fList);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete<T>(T info) where T : class, IId
        {
            CMethod.Update(OperType.Delete, Cache.List<T>(), info);
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
            CMethod.Update(OperType.Delete, Cache.List(type), fList);
        }

        #endregion

        #region File
        public static string ReadFile(string file, out int length)
        {
            var buffer = ReadFileBuffer(file, out length);
            var str = Convert.ToBase64String(buffer);
            return str;
        }
        public static byte[] ReadFileBuffer(string file, out int length)
        {
            using (var fs = File.OpenRead(file))
            {
                var buffer = new byte[fs.Length];
                length = fs.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
        public static void SaveFile(string file, string str)
        {
            var buffer = Convert.FromBase64String(str);
            SaveFile(file, buffer);
        }
        public static void SaveFile(string file, byte[] buffer)
        {
            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(buffer, 0, buffer.Length);
            }
        }

        #endregion
    }
}
