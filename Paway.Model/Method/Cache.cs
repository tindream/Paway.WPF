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
        public static List<T> List<T>()
        {
            var type = typeof(T);
            if (Cache.Dic.ContainsKey(type))
            {
                return Cache.Dic[type];
            }
            throw new WarningException($"类型({type.Name})未配置");
        }
    }
}
