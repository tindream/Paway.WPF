using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// TreeView扩展
    /// </summary>
    public partial class TreeViewEXT : TreeView
    {
        #region 属性
        /// <summary>
        /// 选中项列表
        /// </summary>
        [Browsable(false)]
        public List<ITreeView> ChekedItems
        {
            get
            {
                var list = new List<ITreeView>();
                if (this.ItemsSource is List<ITreeView> modelList)
                {
                    foreach (var item in modelList)
                    {
                        if (item.IsChecked == true) list.Add(item);
                    }
                }
                return list;
            }
        }

        #endregion

        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty CheckBoxProperty =
            DependencyProperty.RegisterAttached(nameof(CheckBox), typeof(Visibility), typeof(TreeViewEXT),
                new PropertyMetadata(Visibility.Collapsed));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(TreeViewEXT),
                new PropertyMetadata(new BrushEXT(Colors.LightGray, Color.FromArgb(170, Config.Color.R, Config.Color.G, Config.Color.B), null, 85)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBackgroundProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBackground), typeof(BrushEXT), typeof(TreeViewEXT),
            new PropertyMetadata(new BrushEXT(null, Color.FromArgb(85, Config.Color.R, Config.Color.G, Config.Color.B), alpha: 85)));

        #endregion

        #region 扩展
        /// <summary>
        /// 选择框
        /// </summary>
        [Category("扩展")]
        [Description("选择框")]
        public Visibility CheckBox
        {
            get { return (Visibility)GetValue(CheckBoxProperty); }
            set { SetValue(CheckBoxProperty, value); }
        }
        /// <summary>
        /// 项边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("项边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }
        /// <summary>
        /// 项背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("项背景颜色")]
        public BrushEXT ItemBackground
        {
            get { return (BrushEXT)GetValue(ItemBackgroundProperty); }
            set { SetValue(ItemBackgroundProperty, value); }
        }

        #endregion

        /// <summary>
        /// </summary>
        public TreeViewEXT()
        {
            DefaultStyleKey = typeof(TreeViewEXT);
        }
    }
}
