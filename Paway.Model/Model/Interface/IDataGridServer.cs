using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// DataGrid数据操作
    /// </summary>
    public interface IDataGridServer
    {
        int Insert<T>(T t, DbCommand cmd = null) where T : class;
        int Delete<T>(T t, DbCommand cmd = null) where T : class;
        int Delete<T>(List<T> list, DbCommand cmd = null) where T : class;
        int Update<T>(T t, DbCommand cmd = null, params string[] args) where T : class;
        int Update<T>(List<T> list, DbCommand cmd = null, params string[] args) where T : class;
        int Replace<T>(T t, params string[] args) where T : class;
        int Replace<T>(List<T> list, params string[] args) where T : class;
        List<T> Find<T>(string find = null, DbCommand cmd = null, params string[] args) where T : class, new();
    }
}
