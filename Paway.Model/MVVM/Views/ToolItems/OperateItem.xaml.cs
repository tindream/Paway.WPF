using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
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
    /// OperateItem.xaml 的交互逻辑
    /// </summary>
    public partial class OperateItem
    {
        #region 快捷键
        private bool iExit = false;
        private DateTime exitTime = DateTime.MinValue;
        protected void Action(KeyMessage msg)
        {
            if (!(this.DataContext is OperateItemModel operate) || operate.Menu != Config.Menu) return;
            switch (msg.Key)
            {
                case Key.Escape:
                    if (iExit && DateTime.Now.Subtract(exitTime).TotalMilliseconds < Config.DoubleInterval)
                    {
                        this.tbSearch.Text = null;
                        return;
                    }
                    iExit = true;
                    exitTime = DateTime.Now;
                    break;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                switch (msg.Key)
                {
                    case Key.F: tbSearch.Focus(); break;
                }
            }
        }

        #endregion

        public OperateItem()
        {
            Messenger.Default.Register<KeyMessage>(this, msg => Action(msg));
            InitializeComponent();
        }
    }
}
