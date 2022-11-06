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
    /// <para>模型中的导入自动比较</para>
    /// </summary>
    public interface ICompare<T>
    {
        bool Compare(T item);
    }
}
