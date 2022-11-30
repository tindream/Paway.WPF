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
    /// 加载子级接口
    /// </summary>
    public interface ILoad<Child>
    {
        List<Child> DetailList { get; }
        void Load();
    }
}
