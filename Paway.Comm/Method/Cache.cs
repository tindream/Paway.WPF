using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paway.Comm
{
    public partial class Cache
    {
        /// <summary>
        /// 数据缓存列表
        /// </summary>
        public static Dictionary<Type, dynamic> Dic { get; private set; } = new Dictionary<Type, dynamic>();

        /// <summary>
        /// 初始化缓存，不排序
        /// </summary>
        public static void Init<T>(IDataService server, Expression<Func<T, bool>> action = null) where T : class, new()
        {
            var list = action == null ? server.Find<T>() : server.Find(action);
            //CMethod.Sorted(list);
            Dic[typeof(T)] = list;
        }
        /// <summary>
        /// 初始化缓存，不排序
        /// </summary>
        public static void Init<T>(IDataService server, List<T> list, Expression<Func<T, bool>> action = null) where T : class, new()
        {
            var dyList = list;
            if (server != null)
            {
                var tempList = action == null ? server.Find<T>() : server.Find(action);
                //CMethod.Sorted(tempList);
                dyList.AddRange(tempList);
            }
            Dic[typeof(T)] = dyList;
        }
        /// <summary>
        /// 缓存排序
        /// </summary>
        public static void Sorted()
        {
            foreach (var item in Dic) CMethod.Sorted(item.Value);
        }

        #region 内部查询
        /// <summary>
        /// 缓存原始列表
        /// <para>非线程安全队列，可能存在空值</para>
        /// </summary>
        public static dynamic AllList(Type type)
        {
            if (Dic.ContainsKey(type))
            {
                return Dic[type];
            }
            throw new WarningException($"{type.Description()}未配置缓存");
        }
        /// <summary>
        /// 缓存原始列表
        /// <para>非线程安全队列，可能存在空值</para>
        /// </summary>
        public static List<T> AllList<T>()
        {
            return AllList(typeof(T));
        }
        public static ParallelQuery<T> Query<T>(Func<T, bool> action, bool all = false)
        {
            Func<T, bool> enableFilter = null;
            if (!all && typeof(IEnable).IsAssignableFrom(typeof(T)))
            {
                enableFilter = c => ((IEnable)c).Enable == EnableType.Enable;
            }
            return AllList<T>().AsParallel().Where(c => c != null && enableFilter?.Invoke(c) != false && action?.Invoke(c) != false);
        }
        public static int Count<T>(Func<T, bool> action)
        {
            lock (AllList<T>()) return Query(action).Count();
        }
        public static int DistinctCount<T>(Func<T, bool> action, Func<T, T, bool> distinct)
        {
            lock (AllList<T>()) return Query(action).Distinct(distinct).Count();
        }
        public static int GroupCount<T, TKey>(Func<T, bool> action, Func<T, TKey> group)
        {
            lock (AllList<T>()) return Query(action).GroupBy(group).Count();
        }
        public static int Sum<T>(Func<T, bool> action, Func<T, int> sum)
        {
            lock (AllList<T>()) return Query(action).Sum(sum);
        }
        public static bool Any<T>(Func<T, bool> action)
        {
            lock (AllList<T>()) return Query(action).Any();
        }

        public static T Find<T>(Func<T, bool> action, bool all = false)
        {
            lock (AllList<T>()) return Query(action, all).FirstOrDefault();
        }
        public static T Find<T>(int id, bool all = false) where T : IId
        {
            return Find<T>(c => c.Id == id, all);
        }
        public static int FindId<T>(Func<T, bool> action) where T : IId
        {
            lock (AllList<T>()) return Query(action).FirstOrDefault()?.Id ?? 0;
        }
        /// <summary>
        /// 全列表查询指定类名下指定Id数据
        /// </summary>
        public static IOperateInfo Find(string name, int id)
        {
            foreach (var item in Dic)
            {
                if (item.Key.Name == name) return FindById(item.Value, id);
            }
            return default;
        }
        private static T FindById<T>(List<T> list, int id) where T : IOperateInfo
        {
            return list.Find(c => c.Id == id);
        }
        /// <summary>
        /// Id列表(并发，乱序)
        /// </summary>
        public static List<int> FindIds<T>(Func<T, bool> action) where T : IId
        {
            lock (AllList<T>()) return Query(action).Select(c => c.Id).ToList();
        }
        /// <summary>
        /// 父级Id列表(并发，乱序)
        /// </summary>
        public static List<int> FindParentIds<T>(Func<T, bool> action) where T : IParent
        {
            lock (AllList<T>()) return Query(action).Select(c => c.ParentId).ToList();
        }
        /// <summary>
        /// 查询列表(并发，乱序)
        /// </summary>
        public static List<T> QueryList<T>(Func<T, bool> action = null, bool all = false)
        {
            lock (AllList<T>()) return Query(action, all).ToList();
        }
        /// <summary>
        /// 查询列表
        /// <para>按Id倒序</para>
        /// </summary>
        public static List<T> QueryOrderByIdDesc<T>(Func<T, bool> action) where T : IId
        {
            lock (AllList<T>()) return Query(action).OrderByDescending(c => c.Id).ToList();
        }
        /// <summary>
        /// 查询列表(同步，顺序)
        /// </summary>
        public static List<T> FindAll<T>(Func<T, bool> action = null)
        {
            lock (AllList<T>()) return AllList<T>().FindAll(c => c != null && action?.Invoke(c) != false);
        }
        public static string Name<T>(int id, string notFound = null) where T : IName
        {
            return Find<T>(id)?.Name ?? notFound ?? CConfig.NotFound;
        }
        public static string CustomName<T>(int id, string notFound = null) where T : ICustomName
        {
            return Find<T>(id)?.CustomName ?? notFound ?? CConfig.NotFound;
        }
        /// <summary>
        /// 名称列表(同步，以保证排序问题)
        /// </summary>
        public static List<string> FindNames<T>(Func<T, bool> action) where T : IName
        {
            lock (AllList<T>()) return AllList<T>().FindAll(c => c != null && action?.Invoke(c) != false).Select(c => c.Name).Distinct().ToList();
        }
        /// <summary>
        /// 名称列表(同步，以保证排序问题)
        /// </summary>
        public static List<string> FindCustomNames<T>(Func<T, bool> action) where T : ICustomName
        {
            lock (AllList<T>()) return AllList<T>().FindAll(c => c != null && action?.Invoke(c) != false).Select(c => c.CustomName).Distinct().ToList();
        }

        #endregion
    }
}
