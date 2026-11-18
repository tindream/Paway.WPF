using System.Windows;

namespace Paway.WPF
{
    /// <summary>
    /// 把"最近的外层 EXT 控件"沿树向下传递，用来替代样式表里那些
    /// <c>{Binding Path=XXX, RelativeSource={RelativeSource AncestorType={x:Type local:XxxEXT}}}</c>。
    /// <para>
    /// 为什么不能继续用 FindAncestor：ItemContainerStyle / RowStyle 里的 Setter 是在
    /// 容器刚被创建、还没挂进可视树的时候求值的，这时祖先链是断的，绑定必然失败，
    /// WPF 会为每个容器刷 "Data Error 4 : Cannot find source for binding"。
    /// 配置下拉动辄几百项，一次切换标签就是上千条错误，光是写调试输出就够把 UI 拖卡。
    /// </para>
    /// <para>
    /// 用法：在宿主控件的样式里挂一行
    /// <c>&lt;Setter Property="local:AncestorBridge.Host" Value="{Binding RelativeSource={RelativeSource Self}}"/&gt;</c>，
    /// 子元素改成 <c>{Binding Path=(local:AncestorBridge.Host).Xxx, RelativeSource={RelativeSource Self}}</c>。
    /// Self 永远能解析到（元素自己），Host 还没继承到值时只是取不到值，不会报错；
    /// 等元素挂进树、继承生效，绑定自动拿到正确值，行为与原来一致。
    /// </para>
    /// </summary>
    public static class AncestorBridge
    {
        /// <summary>
        /// 最近的外层宿主控件。带 Inherits 标记，沿继承链自动往下传，
        /// 嵌套控件自己再设一次就会顶掉外层的值，不会串色。
        /// </summary>
        public static readonly DependencyProperty HostProperty =
            DependencyProperty.RegisterAttached("Host", typeof(FrameworkElement), typeof(AncestorBridge),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>取宿主控件</summary>
        public static FrameworkElement GetHost(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(HostProperty);
        }
        /// <summary>设置宿主控件</summary>
        public static void SetHost(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(HostProperty, value);
        }
    }
}
