using Paway.Helper;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Paway.WPF
{
    /// <summary>
    /// 富文本扩展
    /// </summary>
    public class RichTextBoxEXT : RichTextBox
    {
        #region 依赖属性
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(RichTextBoxEXT), new PropertyMetadata(new CornerRadius(3d)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(RichTextBoxEXT),
                new PropertyMetadata(new BrushEXT()));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// </summary>
        [Category("扩展")]
        [Description("自定义边框圆角")]
        public CornerRadius Radius
        {
            get { return (CornerRadius)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }
        /// <summary>
        /// 边框颜色
        /// </summary>
        [Category("扩展")]
        [Description("边框颜色")]
        public BrushEXT ItemBrush
        {
            get { return (BrushEXT)GetValue(ItemBrushProperty); }
            set { SetValue(ItemBrushProperty, value); }
        }

        #endregion

        /// <summary>
        /// 滚动条
        /// </summary>
        [Browsable(false)]
        public ScrollViewer ScrollViewer { get; set; }
        /// <summary>
        /// </summary>
        public RichTextBoxEXT()
        {
            DefaultStyleKey = typeof(RichTextBoxEXT);
            Loaded += delegate
            {
                SetColor();
                SetSize();
            };
            SizeChanged += delegate
            {
                SetSize();
            };
        }
        /// <summary>
        /// 获取滚动条
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.ScrollViewer = this.GetTemplateChild("ScrollViewer") as ScrollViewer;
        }
        private void SetColor()
        {
            CaretBrush = Foreground;
            SelectionBrush = Foreground;
        }
        private void SetSize()
        {
            var w = ActualWidth - BorderThickness.Left - BorderThickness.Right - Padding.Left - Padding.Right - 2;
            Document.MaxPageWidth = w > 0 ? w : Document.MaxPageWidth;
        }

        #region 公共方法
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            Document.Blocks.Clear();
        }
        /// <summary>
        /// 获取文本
        /// </summary>
        public string Text
        {
            get
            {
                var sb = new StringBuilder();
                var isFirst = true;
                foreach (var block in Document.Blocks)
                {
                    if (isFirst) isFirst = false;
                    else sb.AppendLine();
                    if (block is Paragraph paragraph)
                    {
                        foreach (var inline in paragraph.Inlines)
                        {
                            if (inline is Run run) sb.Append(run.Text);
                            else if (inline is LineBreak) sb.AppendLine();
                        }
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 添加文本并换行
        /// </summary>
        public void AddLine(string content, Action action)
        {
            AddLine(content, null, action);
        }
        /// <summary>
        /// 添加URL并换行
        /// </summary>
        public void AddLine(string title, string url)
        {
            AddLine(title, () =>
            {
                if (string.IsNullOrEmpty(url)) return;
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            });
        }
        /// <summary>
        /// 添加文本并换行
        /// </summary>
        public void AddLine(string content = null, Color? color = null, Action action = null)
        {
            var range = new TextRange(this.CaretPosition, this.Document.ContentEnd);
            var isLast = range.Text.Replace("\r", "").Replace("\n", "").IsEmpty() && (range.Text.IndexOf('\n') == range.Text.LastIndexOf('\n'));
            if (Document.Blocks.Count <= 0)
            {
                Document.Blocks.Add(new Paragraph());
            }
            Paragraph block = (Paragraph)Document.Blocks.LastBlock;
            Run run = new Run(content);
            if (action == null)
            {
                if (color != null) { run.Foreground = new SolidColorBrush(color.Value); }
                block.Inlines.Add(run);
            }
            else
            {
                Hyperlink hl = new Hyperlink(run);
                if (color != null) { hl.Foreground = new SolidColorBrush(color.Value); }
                hl.Click += delegate { action(); };
                hl.MouseLeftButtonDown += delegate { action(); };
                block.Inlines.Add(hl);
            }
            Document.Blocks.Add(new Paragraph());
            AutoShow(isLast);
        }
        private void AutoShow(bool isLast)
        {
            if (isLast)
            {
                this.ScrollViewer.ScrollToEnd();
                this.CaretPosition = this.Document.ContentEnd;
            }
            else
            {//无效
                var currObj = this.CaretPosition.Parent;
                if (currObj is FrameworkElement fe)
                {
                    fe.BringIntoView();
                }
                else if (currObj is FrameworkContentElement fce)
                {
                    fce.BringIntoView();
                }
            }
        }

        /// <summary>
        /// 添加文本
        /// </summary>
        public void Add(string content, Action action)
        {
            Add(content, null, action);
        }
        /// <summary>
        /// 添加URL
        /// </summary>
        public void Add(string title, string url)
        {
            Add(title, () =>
            {
                if (string.IsNullOrEmpty(url)) return;
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            });
        }
        /// <summary>
        /// 添加文本
        /// </summary>
        public void Add(string content = null, Color? color = null, Action action = null)
        {
            var range = new TextRange(this.CaretPosition, this.Document.ContentEnd);
            var isLast = range.Text.Replace("\r", "").Replace("\n", "").IsEmpty() && (range.Text.IndexOf('\n') == range.Text.LastIndexOf('\n'));
            if (Document.Blocks.Count <= 0)
            {
                Document.Blocks.Add(new Paragraph());
            }
            Paragraph block = (Paragraph)Document.Blocks.LastBlock;
            Run run = new Run(content);
            if (action == null)
            {
                if (color != null) { run.Foreground = new SolidColorBrush(color.Value); }
                block.Inlines.Add(run);
            }
            else
            {
                Hyperlink hl = new Hyperlink(run);
                if (color != null) { hl.Foreground = new SolidColorBrush(color.Value); }
                hl.Click += delegate { action(); };
                hl.MouseLeftButtonDown += delegate { action(); };
                block.Inlines.Add(hl);
            }
            AutoShow(isLast);
        }

        #endregion
    }
}
