using log4net;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Paway.Helper;
using Paway.Utils;
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

namespace Paway.Model
{
    public partial class Method : PMethod
    {
        #region HTTP同步
        /// <summary>
        /// 接收数据，JSON转实体，更新数据
        /// </summary>
        //public static void Sync(SyncMessage msg)
        //{
        //    if (msg.IType == null || msg.List == null) return;
        //    if (msg.List.Count == 0 && msg.OperType != OperType.Reset) return;
        //    bool isJson = msg.List.GenericType() == typeof(object) || msg.List.GenericType() == typeof(JObject);
        //    {
        //        var list = !isJson ? msg.List.Clone(true) : JsonToIList(msg.List, msg.IType);
        //        if (Cache.Dic.ContainsKey(msg.IType))
        //        {
        //            lock (Cache.Dic[msg.IType])
        //            {
        //                Method.Update(msg.OperType, Cache.Dic[msg.IType], list);
        //                Method.Sorted(Cache.Dic[msg.IType]);
        //            }
        //        }
        //    }
        //}
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

        #region 导入导出
        public static bool Import(string title, out string file, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            file = null;
            var ofd = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
            };
            if (ofd.ShowDialog() == true)
            {
                file = ofd.FileName;
                return true;
            }
            return false;
        }
        public static bool Imports(string title, out string[] file, string filter = "Excel 工作簿|*.xls;*.xlsx")
        {
            file = null;
            var ofd = new OpenFileDialog
            {
                Title = title,
                Filter = filter,
                Multiselect = true,
            };
            if (ofd.ShowDialog() == true)
            {
                file = ofd.FileNames;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 导入后更新列表
        /// </summary>
        public static List<T> Import<T>(List<T> tList, List<T> fList) where T : class, IId, ICompare<T>
        {
            var updateList = new List<T>();
            foreach (var item in fList)
            {
                var normal = tList.Find(c => c.Compare(item));
                if (normal != null)
                {
                    var id = normal.Id;
                    item.Clone(normal);
                    normal.Id = id;
                    updateList.Add(normal);
                }
                else
                {
                    updateList.Add(item);
                }
            }
            return updateList;
        }
        public static bool Export(string name, out string file, string filter = "Excel 工作簿|*.xlsx|Excel 97-2003 工作簿|*.xls")
        {
            file = null;
            var sfd = new SaveFileDialog()
            {
                Title = $"选择要导出的文件位置",
                Filter = filter,
                FileName = name,
            };
            if (sfd.ShowDialog() == true)
            {
                file = sfd.FileName;
                return true;
            }
            return false;
        }
        public static Task<List<T>> FromExcel<T>(string fileName, int start = 0, int end = 0) where T : class, new()
        {
            var dt = ExcelHelper.ToDataTable(fileName);
            var list = dt.ToList<T>();
            return Task.FromResult(list.Skip(start).Take(list.Count - start - end).ToList());
        }

        #endregion

        #region 同步
        /// <summary>
        /// 同步更新
        /// </summary>
        public static void Update<T>(T info) where T : class, IId
        {
            Update(new List<T> { info });
        }
        /// <summary>
        /// 同步更新
        /// </summary>
        public static void Update<T>(List<T> fList) where T : class, IId
        {
            var tList = Cache.List<T>();
            Method.Update(OperType.Update, tList, fList);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete<T>(T info) where T : class, IId
        {
            var tList = Cache.List<T>();
            Method.Update(OperType.Delete, tList, info);
        }
        /// <summary>
        /// 同步删除项
        /// </summary>
        public static void Delete<T>(List<T> fList) where T : class, IId
        {
            var tList = Cache.List<T>();
            Method.Update(OperType.Delete, tList, fList);
        }

        #endregion
    }
}
