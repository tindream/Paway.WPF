using Paway.Helper;
using Paway.Utils;
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

namespace Paway.Model
{
    public partial class Cache
    {
        /// <summary>
        /// 数据缓存列表
        /// </summary>
        public static Dictionary<Type, dynamic> Dic { get; private set; } = new Dictionary<Type, dynamic>();

        public static void Init<T>(IDataService server, List<T> list = null, Expression<Func<T, bool>> action = null) where T : class, new()
        {
            var dyList = list ?? new List<T>();
            if (server != null)
            {
                var tempList = server.Find(action);
                Method.Sorted(tempList);
                dyList.AddRange(tempList);
            }
            Dic[typeof(T)] = dyList;
        }

        #region 内部查询
        internal static dynamic List(Type type)
        {
            if (Dic.ContainsKey(type))
            {
                return Dic[type];
            }
            throw new WarningException($"{type.Description()}未配置缓存");
        }
        /// <summary>
        /// 非线程安全队列，可能存在空值
        /// </summary>
        public static List<T> List<T>()
        {
            return List(typeof(T));
        }
        public static ParallelQuery<T> Query<T>(Func<T, bool> action)
        {
            return List<T>().AsParallel().Where(c => c != null && action?.Invoke(c) != false);
        }
        public static int Count<T>(Func<T, bool> action)
        {
            return Query(action).Count();
        }
        public static T Find<T>(Func<T, bool> action)
        {
            return Query(action).FirstOrDefault();
        }
        public static T Find<T>(int id) where T : IId
        {
            return Find<T>(c => c.Id == id);
        }
        public static string Name<T>(int id) where T : IName
        {
            return Find<T>(id)?.Name ?? Config.NoFound;
        }
        public static string CustomName<T>(int id) where T : ICustomName
        {
            return Find<T>(id)?.CustomName ?? Config.NoFound;
        }
        public static bool Any<T>(Func<T, bool> action)
        {
            return Query(action).Any();
        }
        public static int FindId<T>(Func<T, bool> action) where T : IId
        {
            return Query(action).FirstOrDefault()?.Id ?? 0;
        }
        /// <summary>
        /// Id列表(并发，乱序)
        /// </summary>
        public static List<int> FindIds<T>(Func<T, bool> action) where T : IId
        {
            return Query(action).Select(c => c.Id).ToList();
        }
        /// <summary>
        /// 父级Id列表(并发，乱序)
        /// </summary>
        public static List<int> FindParentIds<T>(Func<T, bool> action) where T : IParent
        {
            return Query(action).Select(c => c.ParentId).ToList();
        }
        /// <summary>
        /// 查询列表(并发，乱序)
        /// </summary>
        public static List<T> QueryList<T>(Func<T, bool> action = null)
        {
            return Query(action).ToList();
        }
        /// <summary>
        /// 查询列表(同步，顺序)
        /// </summary>
        public static List<T> FindAll<T>(Func<T, bool> action = null)
        {
            return List<T>().FindAll(c => c != null && action?.Invoke(c) != false);
        }
        /// <summary>
        /// 名称列表(同步，以保证排序问题)
        /// </summary>
        public static List<string> FindNames<T>(Func<T, bool> action) where T : IName
        {
            return FindAll<T>(c => action?.Invoke(c) != false).Select(c => c.Name).Distinct().ToList();
        }
        /// <summary>
        /// 名称列表(同步，以保证排序问题)
        /// </summary>
        public static List<string> FindCustomNames<T>(Func<T, bool> action) where T : ICustomName
        {
            return FindAll<T>(c => action?.Invoke(c) != false).Select(c => c.CustomName).Distinct().ToList();
        }

        #endregion
    }
}
