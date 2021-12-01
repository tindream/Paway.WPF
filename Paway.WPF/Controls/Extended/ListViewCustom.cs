using Paway.Helper;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Paway.WPF
{
    /// <summary>
    /// ListView扩展外部自定义样式
    /// </summary>
    public partial class ListViewCustom : ListViewEXT
    {
        /// <summary>
        /// </summary>
        public ListViewCustom()
        {
            DefaultStyleKey = typeof(ListViewCustom);
        }
    }
}
