﻿using GalaSoft.MvvmLight.Messaging;
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
        /// <summary>
        /// 通用工具栏
        /// </summary>
        public OperateItem()
        {
            InitializeComponent();
            Messenger.Default.Send(new OperateLoadMessage() { Obj = dpOperateItem });
        }
    }
}
