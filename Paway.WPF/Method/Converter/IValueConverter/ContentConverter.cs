using Paway.Helper;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// DataGridCell.ToolTip自动绑定
    /// </summary>
    internal class ContentTextConverter : IMultiValueConverter
    {
        /// <summary>
        /// DataGridCell.ToolTip自动绑定
        /// </summary>
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value[0] == null) return null;
            if (value[1] is DataGridCell gridCell)
            {
                if (gridCell.Content is TextBlock textBlock)
                {
                    var tipText = new TextBlock();
                    Binding binding = new Binding(nameof(textBlock.Text));
                    binding.Source = textBlock;
                    tipText.SetBinding(TextBlock.TextProperty, binding);
                    return tipText;
                }
                return gridCell.Content;
            }
            return null;
        }

        private void TextBlock_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
