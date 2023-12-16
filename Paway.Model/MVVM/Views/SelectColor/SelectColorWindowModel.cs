using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Paway.Helper;
using Paway.Model;
using Paway.WPF;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.Model
{
    /// <summary>
    /// 颜色搭取器模型
    /// </summary>
    public class SelectColorWindowModel : BaseWindowModel
    {
        #region 属性
        private Color _color = Colors.Red;
        /// <summary>
        /// 选中的颜色
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = Color.FromArgb((byte)_a, value.R, value.G, value.B); OnPropertyChanged(); }
        }
        private int _a = 255;
        /// <summary>
        /// 颜色值-透明度
        /// </summary>
        public int A
        {
            get { return _a; }
            set { _a = value; OnPropertyChanged(); Color = Color; }
        }
        private double _value = 5.59;
        /// <summary>
        /// 颜色条-值
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); this.Color = PMethod.ColorSelector(value / 7); }
        }

        #endregion

        /// <summary>
        /// 选择颜色窗体模型
        /// </summary>
        public SelectColorWindowModel()
        {
            this.Title = "选取颜色";
        }
    }
}