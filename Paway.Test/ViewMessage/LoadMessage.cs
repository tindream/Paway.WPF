using Paway.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Paway.Test
{
    public class TipLoadMessage : LoadMessage { }
    /// <summary>
    /// Tip窗体切换
    /// </summary>
    public class TipStateMessage { }
    /// <summary>
    /// Tip窗体关闭
    /// </summary>
    public class TipCloseMessage { }
    public class MainLoadMessage : LoadMessage { }
    public class TestLoadMessage : LoadMessage { }
    public class TestDataGridLoadMessage : LoadMessage { }
}
