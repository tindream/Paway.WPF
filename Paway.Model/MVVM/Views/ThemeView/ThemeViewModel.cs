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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 主题设置模型
    /// </summary>
    public class ThemeViewModel : BaseWindowModel
    {
        private bool iSave;

        private Color themeColor;
        private double themeFontSize;
        private string themeFontFamily;

        /// <summary>
        /// 主题颜色
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// 主题颜色
        /// </summary>
        protected override void Action(ListViewCustom listView1)
        {
            if (listView1.SelectedItem is ListBoxItemEXT selectedItem)
            {
                this.Color = (Color)ColorConverter.ConvertFromString(selectedItem.Tag.ToStrings());
                PConfig.Color = this.Color;
            }
        }

        private double _fontSize;
        /// <summary>
        /// 字体大小
        /// </summary>
        public double FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value; OnPropertyChanged();
                    PConfig.FontSize = _fontSize;
                }
            }
        }

        /// <summary>
        /// 字体列表
        /// </summary>
        public List<FontInfo> FontList { get; } = new List<FontInfo>();
        private string fontFamily = "Microsoft YaHei";
        /// <summary>
        /// 主题文本字体
        /// </summary>
        public string FontFamily
        {
            get { return fontFamily; }
            set
            {
                if (fontFamily != value)
                {
                    fontFamily = value; OnPropertyChanged();
                    PConfig.FontFamily = fontFamily;
                }
            }
        }

        /// <summary>
        /// 确认修改
        /// </summary>
        protected override bool? OnCommit(Window wd)
        {
            this.Saved();
            return base.OnCommit(wd);
        }
        /// <summary>
        /// 确认修改
        /// </summary>
        public void Saved()
        {
            this.iSave = true;
        }
        /// <summary>
        /// 取消修改
        /// </summary>
        protected override bool? OnCancel()
        {
            this.iSave = false;
            return base.OnCancel();
        }
        /// <summary>
        /// 取消还原主题设置
        /// </summary>
        public void Restore()
        {
            if (!this.iSave)
            {
                PConfig.Color = this.themeColor;
                PConfig.FontSize = this.themeFontSize;
                PConfig.FontFamily = this.themeFontFamily;
            }
        }

        /// <summary>
        /// 主题设置窗体模型
        /// </summary>
        public ThemeViewModel()
        {
            base.Title = "本地设置";
            var index = 0;
            foreach (var font in Fonts.SystemFontFamilies)
            {
                var info = new FontInfo { Id = index++, Name = font.Source, FontFamily = font };
                FontList.Add(info);
            }
            Messenger.Default.Register<ThemeLoadMessage>(this, msg =>
            {
                this.themeColor = PConfig.Color;
                this.themeFontSize = PConfig.FontSize;
                this.themeFontFamily = PConfig.FontFamily;

                this.Color = PConfig.Color;
                this.FontSize = PConfig.FontSize;
                this.FontFamily = PConfig.FontFamily;
            });
        }
    }
}