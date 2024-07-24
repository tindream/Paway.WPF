using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Model
{
    /// <summary>
    /// StatuItem.xaml 的交互逻辑
    /// </summary>
    public partial class StatuItem
    {
        #region 扩展参数
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IBackColorProperty =
            DependencyProperty.Register(nameof(IBackColor), typeof(bool), typeof(StatuItem), new PropertyMetadata(true));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IConnectProperty =
            DependencyProperty.Register(nameof(IConnect), typeof(bool), typeof(StatuItem), new PropertyMetadata(false));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty IConnect2Property =
            DependencyProperty.Register(nameof(IConnect2), typeof(bool), typeof(StatuItem), new PropertyMetadata(false));
        /// <summary>
        /// 消息变色应用到背景或文本，默认为背景
        /// </summary>
        [Category("扩展")]
        [Description("消息变色应用到背景或文本，默认为背景")]
        public bool IBackColor
        {
            get { return (bool)GetValue(IBackColorProperty); }
            set { SetValue(IBackColorProperty, value); }
        }
        /// <summary>
        /// 显示连接状态
        /// </summary>
        [Category("扩展")]
        [Description("显示连接状态")]
        public bool IConnect
        {
            get { return (bool)GetValue(IConnectProperty); }
            set { SetValue(IConnectProperty, value); }
        }
        /// <summary>
        /// 显示连接状态2
        /// </summary>
        [Category("扩展")]
        [Description("显示连接状态2")]
        public bool IConnect2
        {
            get { return (bool)GetValue(IConnect2Property); }
            set { SetValue(IConnect2Property, value); }
        }

        #endregion

        /// <summary>
        /// 状态栏
        /// </summary>
        public StatuItem()
        {
            DefaultStyleKey = typeof(StatuItem);
            InitializeComponent();
        }
    }
}
