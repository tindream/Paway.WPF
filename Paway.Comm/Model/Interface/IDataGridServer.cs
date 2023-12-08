using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// DataGrid数据操作接口
    /// </summary>
    public interface IDataGridServer
    {
        /// <summary>
        /// 插入项
        /// </summary>
        int Insert<T>(T t, DbCommand cmd = null) where T : class;
        /// <summary>
        /// 插入列表
        /// </summary>
        int Insert<T>(List<T> list, DbCommand cmd = null) where T : class;
        /// <summary>
        /// 删除项
        /// </summary>
        int Delete<T>(T t, DbCommand cmd = null) where T : class;
        /// <summary>
        /// 删除列表
        /// </summary>
        int Delete<T>(List<T> list, DbCommand cmd = null) where T : class;
        /// <summary>
        /// 更新项
        /// </summary>
        int Update<T>(T t, DbCommand cmd = null, params string[] args) where T : class;
        /// <summary>
        /// 更新列表
        /// </summary>
        int Update<T>(List<T> list, DbCommand cmd = null, params string[] args) where T : class;
        /// <summary>
        /// 替换,由insert/Update自动替换
        /// </summary>
        int Replace<T>(T t, params string[] args) where T : class;
        /// <summary>
        /// 替换,由insert/Update自动替换
        /// </summary>
        int Replace<T>(List<T> list, params string[] args) where T : class;
        /// <summary>
        /// 查找指定类型数据库所有数据列表(按指定条件过滤)
        /// </summary>
        List<T> Find<T>(string find = null, DbCommand cmd = null, params string[] args) where T : class, new();
    }
}
