using Newtonsoft.Json;
using Paway.Helper;
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
    /// <summary>
    /// HTTP通讯客户端通用定义
    /// </summary>
    public partial class WebApiClient
    {
        /// <summary>
        /// 主机根地址
        /// </summary>
        protected static string httpUrl;
        /// <summary>
        /// 初始化主机根地址
        /// </summary>
        public static void Init(string url)
        {
            httpUrl = url;
        }

        #region 保存、同步、发送
        /// <summary>
        /// 插入项
        /// </summary>
        public static void Insert<T>(T obj) where T : class
        {
            if (obj == null) return;
            var list = Insert(new List<T> { obj });
            if (obj is IId id) id.Id = (list[0] as IId).Id;
        }
        /// <summary>
        /// 插入列表
        /// </summary>
        public static List<T> Insert<T>(List<T> list) where T : class
        {
            if (list.Count == 0) return list;
            var sync = new SyncInsertMessage(list);
            var result = Send(sync);
            var resultList = JsonConvert.DeserializeObject<List<T>>(result.data.ToString());
            for (int i = 0; i < list.Count; i++) resultList[i].Clone(list[i], true);
            return list;
        }
        /// <summary>
        /// 更新项
        /// </summary>
        public static int Update<T>(T obj, params string[] args) where T : class
        {
            if (obj == null) return 0;
            var list = new List<T> { obj };
            Update(list, args);
            list[0].Clone(obj);
            return 0;
        }
        /// <summary>
        /// 更新列表
        /// </summary>
        public static int Update<T>(List<T> list, params string[] args) where T : class
        {
            if (list.Count == 0) return 0;
            var sync = new SyncUpdateMessage(list, args);
            var result = Send(sync);
            var resultList = JsonConvert.DeserializeObject<List<T>>(result.data.ToString());
            resultList.Clone(list, true);
            return 0;
        }
        /// <summary>
        /// 删除项
        /// </summary>
        public static void Delete<T>(T obj)
        {
            if (obj == null) return;
            Delete(new List<T> { obj });
        }
        /// <summary>
        /// 删除列表
        /// </summary>
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
            var list = JsonConvert.DeserializeObject<List<T>>(result.data.ToString());
            CMethod.Sorted(list);
            return list;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        public static List<T> ExecuteList<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var sql = predicate.SQL();
            Trace.WriteLine(sql);
            return ExecuteList<T>(sql);
        }
        /// <summary>
        /// 查询列表
        /// </summary>
        public static List<T> ExecuteList<T>(string sql) where T : new()
        {
            Type type = typeof(T);
            var sync = new SyncExecuteListMessage(type, sql);
            var result = Send(sync);
            var list = JsonConvert.DeserializeObject<List<T>>(result.data.ToString());
            return list;
        }
        /// <summary>
        /// 执行SQL，并返回结果
        /// </summary>
        public static object ExecuteScalar<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var sql = predicate.SQL();
            Trace.WriteLine(sql);
            return ExecuteScalar<T>(sql);
        }
        /// <summary>
        /// 执行SQL，并返回结果
        /// </summary>
        public static object ExecuteScalar<T>(string sql) where T : class
        {
            Type type = typeof(T);
            var sync = new SyncExecuteScalarMessage($"select Count(0) from ({type.Select()} where {sql ?? "1=1"}) A");
            var result = Send(sync);
            return result.data;
        }

        /// <summary>
        /// 发送请求，并等待结果
        /// </summary>
        public static HttpResponseMessage Send(IMessage sync, int timeout = 30)
        {
            var task = SendAsync(sync, timeout);
            if (task.Result.code != 200) throw new Exception(task.Result.msg);
            return task.Result;
        }
        /// <summary>
        /// 异步发送请求，并等待结果
        /// </summary>
        private static Task<HttpResponseMessage> SendAsync(IMessage sync, int timeout = 30)
        {
            return Task.Run(() =>
            {
                try
                {
                    var url = $"{httpUrl}/sync";
                    var json = JsonConvert.SerializeObject(sync).CompressBase64();
                    using (var client = new WebClientPro(CConfig.User, timeout))
                    {
                        string response = client.UploadString(url, "POST", json).Decompress();
                        return JsonConvert.DeserializeObject<HttpResponseMessage>(response);
                    }
                }
                catch (WebException ex)
                {
                    throw CMethod.HttpError(ex, true);
                }
            });
        }

        #endregion

        #region 文件上传下载
        /// <summary>
        /// 文件上传
        /// </summary>
        public static string UpFile(string remoteFile, string localFile, double max = 0, Action<double> percentage = null, Action completed = null)
        {
            var task = UpFileAsync(remoteFile, localFile, max, percentage, completed);
            if (task.Result.code != 200) throw new Exception(task.Result.msg);
            return task.Result.msg;
        }
        /// <summary>
        /// 异步文件上传
        /// </summary>
        public static Task<HttpResponseMessage> UpFileAsync(string remoteFile, string localFile, double max = 0, Action<double> percentage = null, Action completed = null)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var client = new WebClientPro(CConfig.User, 2 * 60))
                    {
                        string response = client.UpFileAsync($"{httpUrl}/{CConfig.UploadPath}", remoteFile, localFile, max, percentage);
                        var result = JsonConvert.DeserializeObject<HttpResponseMessage>(response);
                        completed?.Invoke();
                        return result;
                    }
                }
                catch (WebException ex)
                {
                    throw CMethod.HttpError(ex, true);
                }
            });
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        public static void DownFile(string remoteFile, string localFile, Action<double> percentage = null, Action completed = null)
        {
            var task = DownFileAsync(remoteFile, localFile, percentage, completed);
            if (task.Result.code != 200) throw new Exception(task.Result.msg);
        }
        /// <summary>
        /// 异步文件下载
        /// </summary>
        public static Task<HttpResponseMessage> DownFileAsync(string remoteFile, string localFile, Action<double> percentage = null, Action completed = null)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var client = new WebClientPro(CConfig.User, 2 * 60))
                    {
                        string response = client.DownFileAsync($"{httpUrl}/{CConfig.UploadPath}", remoteFile, percentage);
                        var result = JsonConvert.DeserializeObject<HttpResponseMessage>(response);
                        if (result.code == 200)
                        {
                            var toPath = Path.GetDirectoryName(localFile);
                            if (!Directory.Exists(toPath)) Directory.CreateDirectory(toPath);
                            CMethod.SaveFile(localFile, result.msg);
                        }
                        completed?.Invoke();
                        return result;
                    }
                }
                catch (WebException ex)
                {
                    throw CMethod.HttpError(ex, true);
                }
            });
        }

        #endregion
    }
}
