using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Paway.Model
{
    /// <summary>
    /// 单实例
    /// </summary>
    public class ViewlLocator
    {
        /// <summary>
        /// 单实例列表
        /// </summary>
        private static readonly Dictionary<string, dynamic> instanceDic = new Dictionary<string, dynamic>();

        /// <summary>
        /// 创建单实例，如存存时从缓存列表获取
        /// </summary>
        public static T GetInstance<T>(object key = null)
        {
            var name = $"{typeof(T).FullName}_{key}";
            if (!instanceDic.ContainsKey(name))
            {
                var obj = Activator.CreateInstance<T>();
                instanceDic.Add(name, obj);
            }
            return instanceDic[name];
        }
    }
}