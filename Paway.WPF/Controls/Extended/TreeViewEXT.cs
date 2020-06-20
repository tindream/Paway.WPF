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
        public static readonly DependencyProperty BorderFocusedBrushProperty =
            DependencyProperty.RegisterAttached(nameof(BorderFocusedBrush), typeof(BrushEXT), typeof(TreeViewEXT),
                new PropertyMetadata(new BrushEXT(Colors.LightGray, Color.FromArgb(170, 35, 175, 255), null, 85)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty BackgroundRoundProperty =
            DependencyProperty.RegisterAttached(nameof(BackgroundRound), typeof(BrushEXT), typeof(TreeViewEXT),
            new PropertyMetadata(new BrushEXT(null, Color.FromArgb(85, 35, 175, 255), alpha: 85)));

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
        /// 边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT BorderFocusedBrush
        {
            get { return (BrushEXT)GetValue(BorderFocusedBrushProperty); }
            set { SetValue(BorderFocusedBrushProperty, value); }
        }
        /// <summary>
        /// 项的边框背景颜色
        /// </summary>
        [Category("扩展")]
        [Description("项的边框背景颜色")]
        public BrushEXT BackgroundRound
        {
            get { return (BrushEXT)GetValue(BackgroundRoundProperty); }
            set { SetValue(BackgroundRoundProperty, value); }
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
