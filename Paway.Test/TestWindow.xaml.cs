using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : WindowEXT
    {
        public TestWindow()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Config.LanguageList.Clear();
            var directoryInfo = new System.IO.DirectoryInfo(Config.LanguagePath);
            var fileInfos = directoryInfo.GetFiles("*.xml");
            foreach (var fileInfo in fileInfos) Config.LanguageList.Add(System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name));

            PMethod.LanguageMenuBinding(menu, Config.LanguageList, ViewModelLocator.Default.TestWindow, ViewModelLocator.Default.TestWindow.ItemClickCommand, nameof(ViewModelLocator.Default.TestWindow.Language));

            Messenger.Default.Send(new TestLoadMessage() { Obj = Root });
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            ViewModelLocator.Default.TestWindow.Close();
            base.OnClosing(e);
        }
    }
}
