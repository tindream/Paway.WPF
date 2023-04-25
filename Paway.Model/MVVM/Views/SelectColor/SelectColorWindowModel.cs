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
    public class SelectColorWindowModel : BaseWindowModel
    {
        #region 属性
        private Color _color = Colors.Red;
        public Color Color
        {
            get { return _color; }
            set { _color = Color.FromArgb((byte)_a, value.R, value.G, value.B); OnPropertyChanged(); }
        }
        private int _a = 255;
        public int A
        {
            get { return _a; }
            set { _a = value; OnPropertyChanged(); Color = Color; }
        }
        private double _value = 5.59;
        public double Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(); this.Color = Method.ColorSelector(value / 7); }
        }

        #endregion

        public SelectColorWindowModel()
        {
            this.Title = "选取颜色";
        }
    }
}