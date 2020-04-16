using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Paway.WPF
{
    public partial class PanelListBase : BorderControlBase
    {
        private Panel panel;
        public PanelItem Current { get; private set; }
        public event Action<PanelItem> SelectedEvent;

        #region 公开属性
        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty =
                DependencyProperty.RegisterAttached("HorizontalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(PanelListBase),
                new PropertyMetadata(ScrollBarVisibility.Disabled, OnHorizontalScrollBarVisibilityChanged));
        private static void OnHorizontalScrollBarVisibilityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PanelListBase panel)
            {
                if (Method.Child(panel, out ScrollViewerRound scrollViewer))
                {
                    scrollViewer.HorizontalScrollBarVisibility = (ScrollBarVisibility)e.NewValue;
                }
            }
        }

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty =
                DependencyProperty.RegisterAttached("VerticalScrollBarVisibility", typeof(ScrollBarVisibility), typeof(PanelListBase),
                new PropertyMetadata(ScrollBarVisibility.Visible, OnVerticalScrollBarVisibilityChanged));
        private static void OnVerticalScrollBarVisibilityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is PanelListBase panel)
            {
                if (Method.Child(panel, out ScrollViewerRound scrollViewer))
                {
                    scrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)e.NewValue;
                }
            }
        }

        [Category("扩展")]
        public ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty); }
            set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
        }
        [Category("扩展")]
        public ScrollBarVisibility VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); }
            set { SetValue(VerticalScrollBarVisibilityProperty, value); }
        }

        #endregion

        #region 本地操作
        protected void Init(Panel panel, ScrollViewerRound scrollViewer)
        {
            this.panel = panel;
            scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
        }
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (scrollviewer.ComputedVerticalScrollBarVisibility == Visibility.Visible || scrollviewer.ComputedHorizontalScrollBarVisibility != Visibility.Visible)
                return;
            if (e.Delta > 0)
                scrollviewer.LineLeft();
            else
                scrollviewer.LineRight();
            e.Handled = true;
        }
        private void Item_SelectedEvent(PanelItem item)
        {
            this.Current = item;
            SelectedEvent?.Invoke(item);
        }
        private void Selected(PanelItem item)
        {
            foreach (PanelItem temp in this.panel.Children)
            {
                temp.Selected = false;
            }
            item.Selected = true;
        }
        protected override void ClickHandle(object sender)
        {
            if (sender is PanelItem item)
            {
                Selected(item);
            }
            base.ClickHandle(sender);
        }

        #endregion

        #region public
        public void Clear()
        {
            this.panel.Children.Clear();
            this.Current = null;
        }
        public void SelectIndex(int index = 0)
        {
            if (index >= this.panel.Children.Count) index = this.panel.Children.Count - 1;
            if (index >= 0) Selected(this.panel.Children[index] as PanelItem);
        }
        public bool Add(string text, bool iSelect)
        {
            return Add(text, null, null, iSelect);
        }
        public bool Add(string text, object tag = null, bool iSelect = true)
        {
            return Add(text, null, tag, iSelect);
        }
        public bool Add(string text, string desc, object tag = null, bool iSelect = true)
        {
            PanelItem item = new PanelItem(text, desc, tag);
            item.MouseDown += Border_MouseDown;
            item.MouseUp += Border_MouseUp;
            item.SelectedEvent += Item_SelectedEvent;
            this.panel.Children.Add(item);
            if (iSelect) Selected(item);
            return true;
        }
        public bool Update(string text, string desc = null)
        {
            if (this.Current == null)
            {
                Method.Warning(this, "请选择项");
                return false;
            }
            this.Current.Text = text;
            this.Current.Desc = desc;
            return true;
        }
        public bool Delete(bool selectNext = true)
        {
            if (this.Current == null)
            {
                Method.Warning(this, "请选择项");
                return false;
            }
            for (int i = this.panel.Children.Count - 1; i >= 0; i--)
            {
                var child = this.panel.Children[i] as PanelItem;
                if (child == this.Current)
                {
                    this.panel.Children.RemoveAt(i);
                    if (this.panel.Children.Count > i)
                    {
                        (this.panel.Children[i] as PanelItem).Selected = selectNext;
                    }
                    else if (i > 0)
                    {
                        (this.panel.Children[i - 1] as PanelItem).Selected = selectNext;
                    }
                    else if (this.panel.Children.Count > 0)
                    {
                        (this.panel.Children[0] as PanelItem).Selected = selectNext;
                    }
                    else
                    {
                        this.Current = null;
                    }
                    break;
                }
            }
            if (!selectNext) this.Current = null;
            return true;
        }

        #endregion
    }
}
