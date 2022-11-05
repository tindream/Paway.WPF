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

        public static T GetInstance<T>()
        {
            var name = typeof(T).FullName;
            if (!instanceDic.ContainsKey(name))
            {
                var obj = Activator.CreateInstance<T>();
                instanceDic.Add(name, obj);
            }
            return instanceDic[name];
        }
    }
}