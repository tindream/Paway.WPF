using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Paway.Test
{
    /// <summary>
    /// 比较
    /// </summary>
    public interface ICompare<T>
    {
        bool Compare(T item);
    }
}
