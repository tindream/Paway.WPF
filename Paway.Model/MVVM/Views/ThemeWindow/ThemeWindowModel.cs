using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 主题设置窗体模型
    /// </summary>
    public class ThemeWindowModel : BaseWindowModel
    {
        private DependencyObject Root;

        private Color themeColor;
        private double themeSize;
        /// <summary>
        /// 主题颜色
        /// </summary>
        protected override void Action(ListViewCustom listView1)
        {
            if (listView1.SelectedItem is ListBoxItemEXT selectedItem)
            {
                var color = (Color)ColorConverter.ConvertFromString(selectedItem.Tag.ToStrings());
                PConfig.Color = color;
            }
        }

        private double _sizeValue;
        /// <summary>
        /// 字体大小
        /// </summary>
        public double SizeValue
        {
            get { return _sizeValue; }
            set { _sizeValue = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 字体大小事件
        /// </summary>
        public ICommand SizeChanged => new RelayCommand<SliderEXT>(slider =>
        {
            PConfig.FontSize = slider.Value;
        });

        /// <summary>
        /// 字体列表
        /// </summary>
        public List<FontInfo> FontList { get; } = new List<FontInfo>();
        private string fontFamily = "Microsoft YaHei";
        /// <summary>
        /// 文本字体
        /// </summary>
        [Text("字体")]
        public string FontFamily
        {
            get { return fontFamily; }
            set { fontFamily = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 取消修改
        /// </summary>
        protected override bool? OnCancel()
        {
            PConfig.FontSize = this.themeSize;
            PConfig.Color = this.themeColor;
            return base.OnCancel();
        }

        /// <summary>
        /// 主题设置窗体模型
        /// </summary>
        public ThemeWindowModel()
        {
            base.Title = "本地设置";
            Messenger.Default.Register<ThemeLoadMessage>(this, msg =>
            {
                this.Root = msg.Obj;
                this.themeSize = PConfig.FontSize;
                this.themeColor = PConfig.Color;

                this.SizeValue = PConfig.FontSize;
                var index = 0;
                foreach (var font in Fonts.SystemFontFamilies)
                {
                    var info = new FontInfo { Id = index++, Name = font.Source, FontFamily = font };
                    FontList.Add(info);
                }
            });
        }
    }
}