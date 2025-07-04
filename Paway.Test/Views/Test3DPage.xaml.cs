using GalaSoft.MvvmLight.Messaging;
using HelixToolkit.Wpf;
using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Paway.Test
{
    /// <summary>
    /// Test3DPage.xaml 的交互逻辑
    /// <para>HelixToolkit.Wpf需要框架4.62</para>
    /// </summary>
    public partial class Test3DPage : Page
    {
        public Test3DPage()
        {
            InitializeComponent();
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ////导入mtl，obj 和纹理图片
            //var objFile = @"F:\IronMan.obj";
            //if (File.Exists(objFile))
            //{
            //    Model3DGroup model1 = new ObjReader().Read(objFile);
            //    model.Content = model1;
            //}

            ////导入3ds格式模型
            //Model3DGroup model2 = new ModelImporter().Load(@"C:\Users\Administrator\Desktop\test\file.3ds");
            //model.Content = model2;

            //Messenger.Default.Send(new MainLoadMessage() { });
        }
        private void ModelUIElement3D_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ModelUIElement3D mui3d = sender as ModelUIElement3D;
            var model = mui3d.Model as GeometryModel3D;
            (model.Material as DiffuseMaterial).Brush = Brushes.Orange;
        }
        private void ButtonEXT_Click(object sender, RoutedEventArgs e)
        {
            if (PMethod.OpenFile("选择模型文件", out string file, "模型文件|*.3ds;*.obj;*.stl;*.lwo;*.off;*.ply"))
            {
                Model3DGroup model2 = new ModelImporter().Load(file);
                model.Content = model2;
            }
        }
    }
}
