using log4net;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Paway.Test
{
    public class Method : PMethod
    {
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
            Method.Update(OperType.Update, tList, info);
        }

        #endregion
    }
}
