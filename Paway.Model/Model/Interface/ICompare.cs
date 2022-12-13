using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 比较
    /// </summary>
    public interface ICompare<T>
    {
        /// <summary>
        /// 比较
        /// <para>模型中的导入自动比较</para>
        /// <para>如实现ICheck接口，则优先使用ICheck。未实现时自动使用本接口进行验证</para>
        /// <para>返回true，表示已存在</para>
        /// </summary>
        bool Compare(T item);
    }
}
