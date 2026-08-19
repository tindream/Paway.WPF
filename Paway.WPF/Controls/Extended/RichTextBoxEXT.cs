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
            DependencyProperty.RegisterAttached(nameof(Radius), typeof(CornerRadius), typeof(RichTextBoxEXT), new PropertyMetadata(new CornerRadius(3)));
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty ItemBrushProperty =
            DependencyProperty.RegisterAttached(nameof(ItemBrush), typeof(BrushEXT), typeof(RichTextBoxEXT),
                new PropertyMetadata(new BrushEXT()));

        #endregion

        #region 扩展
        /// <summary>
        /// 自定义边框圆角
        /// <para>默认值：3</para>
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
        /// <para>默认值：默认</para>
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
        public ScrollViewer ScrollViewer { get; private set; }
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
            this.ScrollViewer = this.GetTemplateChild("Part_ScrollViewer") as ScrollViewer;
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
        /// 添加文本段落并换行
        /// </summary>
        public void AddLine(string content, Action custom)
        {
            AddLine(content, null, custom);
        }
        /// <summary>
        /// 添加URL段落
        /// <para>iAppend=true:追加URL到现有段落</para>
        /// </summary>
        public void AddLine(string title, string url, Color? color = null, Action custom = null, bool line = true, bool iAppend = false)
        {
            AddLine(title, color, () =>
            {
                if (string.IsNullOrEmpty(url)) return;
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
                {
                    UseShellExecute = false,
                    CreateNoWindow = true
                });
            }, line, iAppend);
        }
        /// <summary>
        /// 添加文本并换行
        /// <para>iAppend=true:追加文本到现有段落</para>
        /// </summary>
        public void AddLine(string content = null, Color? color = null, Action custom = null, bool line = true, bool iAppend = false)
        {
            if (!content.IsEmpty())
            {
                Paragraph block;
                if (iAppend)
                {
                    if (Document.Blocks.Count <= 0) Document.Blocks.Add(new Paragraph());
                    block = (Paragraph)Document.Blocks.LastBlock;
                }
                else
                {
                    block = new Paragraph();
                    Document.Blocks.Add(block);
                }
                var run = new Run(content);
                if (custom == null)
                {
                    if (color != null) { run.Foreground = color.Value.ToBrush(); }
                    block.Inlines.Add(run);
                }
                else
                {
                    Hyperlink hl = new Hyperlink(run);
                    if (color != null) { hl.Foreground = color.Value.ToBrush(); }
                    hl.Click += delegate { custom(); };
                    hl.MouseLeftButtonDown += delegate { custom(); };
                    block.Inlines.Add(hl);
                }
            }
            if (line) Document.Blocks.Add(new Paragraph());
            AutoLast();
        }
        /// <summary>
        /// 插入水平分割线并换行
        /// </summary>
        public void AddLine(Color color)
        {
            if (Document.Blocks.Count <= 0) Document.Blocks.Add(new Paragraph());
            var block = (Paragraph)Document.Blocks.LastBlock;
            block.Padding = new Thickness(0, 4, 0, 4);

            //底部边框 = 分割线
            block.BorderThickness = new Thickness(0, 0, 0, 1);
            block.BorderBrush = new SolidColorBrush(color);

            Document.Blocks.Add(new Paragraph());
        }
        /// <summary>
        /// 滚动到最后或显示
        /// </summary>
        public void AutoLast()
        {
            this.CaretPosition = this.Document.ContentEnd;
            if (this.ScrollViewer == null) PMethod.BeginInvoke(() => { this.ScrollViewer?.ScrollToEnd(); });
            else this.ScrollViewer.ScrollToEnd();
        }

        #endregion
    }
}
