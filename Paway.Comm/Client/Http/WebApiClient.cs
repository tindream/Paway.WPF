using Newtonsoft.Json;
using Paway.Helper;
using Paway.Model;
using Paway.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Paway.Comm
{
    public partial class WebApiClient
    {
        protected static string httpUrl;
        public static void Init(string url)
        {
            httpUrl = url;
        }

        #region 保存、同步、发送
        public static void Insert<T>(T obj) where T : class
        {
            if (obj == null) return;
            var list = Insert(new List<T> { obj });
            if (obj is IId id) id.Id = (list[0] as IId).Id;
        }
        public static List<T> Insert<T>(List<T> list) where T : class
        {
            if (list.Count == 0) return list;
            var sync = new SyncInsertMessage(list);
            var result = Send(sync);
            var resultList = JsonConvert.DeserializeObject<List<T>>(result.Data.ToString());
            for (int i = 0; i < list.Count; i++) resultList[i].Clone(list[i], true);
            return list;
        }
        public static int Update<T>(T obj, params string[] args) where T : class
        {
            if (obj == null) return 0;
            var list = new List<T> { obj };
            Update(list, args);
            list[0].Clone(obj);
            return 0;
        }
        public static int Update<T>(List<T> list, params string[] args) where T : class
        {
            if (list.Count == 0) return 0;
            var sync = new SyncUpdateMessage(list, args);
            var result = Send(sync);
            var resultList = JsonConvert.DeserializeObject<List<T>>(result.Data.ToString());
            resultList.Clone(list, true);
            return 0;
        }
        public static void Delete<T>(T obj)
        {
            if (obj == null) return;
            Delete(new List<T> { obj });
        }
        public static void Delete<T>(List<T> list)
        {
            if (list.Count == 0) return;
            var sync = new SyncDeleteMessage(list);
            Send(sync);
        }

        /// <summary>
        /// 查询后自动排序
        /// </summary>
        public static List<T> Find<T>(Expression<Func<T, bool>> predicate, params string[] args) where T : class, new()
        {
            return Find(predicate, 0, args);
        }
        /// <summary>
        /// 查询后自动排序
        /// </summary>
        public static List<T> Find<T>(Expression<Func<T, bool>> predicate, int count, params string[] args) where T : class, new()
        {
            var sql = predicate.SQL();
            Trace.WriteLine(sql);
            return Find<T>(sql, count, args);
        }
        /// <summary>
        /// 查询后自动排序
        /// </summary>
        public static List<T> Find<T>(string find = null, params string[] args) where T : class, new()
        {
            return Find<T>(find, 0, args);
        }
        /// <summary>
        /// 查询后自动排序
        /// </summary>
        public static List<T> Find<T>(string find, int count, params string[] args) where T : class, new()
        {
            Type type = typeof(T);
            var sync = new SyncFindMessage(type, find, count, args);
            var result = Send(sync);
            var list = JsonConvert.DeserializeObject<List<T>>(result.Data.ToString());
            Method.Sorted(list);
            return list;
        }

        public static List<T> ExecuteList<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var sql = predicate.SQL();
            Trace.WriteLine(sql);
            return ExecuteList<T>(sql);
        }
        public static List<T> ExecuteList<T>(string sql) where T : new()
        {
            Type type = typeof(T);
            var sync = new SyncExecuteListMessage(type, sql);
            var result = Send(sync);
            var list = JsonConvert.DeserializeObject<List<T>>(result.Data.ToString());
            return list;
        }
        public static object ExecuteScalar<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var sql = predicate.SQL();
            Trace.WriteLine(sql);
            return ExecuteScalar<T>(sql);
        }
        public static object ExecuteScalar<T>(string sql) where T : class
        {
            Type type = typeof(T);
            var sync = new SyncExecuteScalarMessage($"select Count(0) from ({type.Select()} where {sql ?? "1=1"}) A");
            var result = Send(sync);
            return result.Data;
        }

        private static HttpResponseMessage Send(CommMessage sync, int timeout = 30)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    var url = $"{httpUrl}/sync";
                    var json = JsonConvert.SerializeObject(sync).CompressBase64();
                    using (var client = new WebClientPro(Config.User, timeout))
                    {
                        string response = client.UploadString(url, "POST", json).Decompress();
                        return JsonConvert.DeserializeObject<HttpResponseMessage>(response);
                    }
                }
                catch (WebException ex)
                {
                    throw Method.HttpError(ex);
                }
            });
            if (task.Result.Code != 200) throw new Exception(task.Result.Msg);
            return task.Result;
        }

        #endregion

        #region 文件上传下载
        public static string UpFile(string toFile, string file, double max = 0, Action<double> percentage = null, Action completed = null)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    using (var client = new WebClientPro(Config.User, 2 * 60))
                    {
                        string response = client.UpFileAsync(httpUrl, toFile, file, max, percentage);
                        completed?.Invoke();
                        return JsonConvert.DeserializeObject<HttpResponseMessage>(response);
                    }
                }
                catch (WebException ex)
                {
                    throw Method.HttpError(ex);
                }
            });
            if (task.Result.Code != 200) throw new Exception(task.Result.Msg);
            return task.Result.Msg;
        }
        public static void DownFile(string fromFile, string file, Action<double> percentage = null, Action completed = null)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    using (var client = new WebClientPro(Config.User, 2 * 60))
                    {
                        string response = client.DownFileAsync(httpUrl, fromFile, file, percentage, completed);
                        var result = JsonConvert.DeserializeObject<HttpResponseMessage>(response);
                        if (result.Code == 200) Method.SaveFile(file, result.Msg);
                        return result;
                    }
                }
                catch (WebException ex)
                {
                    throw Method.HttpError(ex);
                }
            });
            if (task.Result.Code != 200) throw new Exception(task.Result.Msg);
        }

        #endregion
    }
}
