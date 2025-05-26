using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
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
            Config.Window = this;
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Config.LanguageList.Clear();
            var directoryInfo = new System.IO.DirectoryInfo(Config.LanguagePath);
            var fileInfos = directoryInfo.GetFiles("*.xml");
            foreach (var fileInfo in fileInfos) Config.LanguageList.Add(System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name));

            PMethod.LanguageMenuBinding(menu, Config.LanguageList, ViewModelLocator.Default.Test, ViewModelLocator.Default.Test.ItemClickCommand, nameof(ViewModelLocator.Default.Test.Language));

            Messenger.Default.Send(new TestLoadMessage() { Obj = Root });
        }
    }
}
