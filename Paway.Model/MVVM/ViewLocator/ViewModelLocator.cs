using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Paway.Model
{
    /// <summary>
    /// 视图-模型管理器
    /// </summary>
    public class ViewModelLocator
    {
        private static ViewModelLocator _default;
        private static StatuItemModel _statuItem;
        static ViewModelLocator()
        {
            _default = new ViewModelLocator();
            _statuItem = new StatuItemModel();
            _statuItem.Init();
        }
        internal static ViewModelLocator Default => _default;

        /// <summary>
        /// 模型-状态栏
        /// <para>进程级单例，被 XAML 直接绑定，不参与 Clear 重置</para>
        /// </summary>
        public StatuItemModel StatuItem => _statuItem;
        /// <summary>
        /// 模型-登录页
        /// </summary>
        public virtual LoginPageModel Login => GetModelInstance<LoginPageModel>();
        /// <summary>
        /// 模型-单输入框通用窗体
        /// </summary>
        public NameWindowModel NameWindow => GetModelInstance<NameWindowModel>();
        /// <summary>
        /// 模型-颜色搭取器
        /// </summary>
        public SelectColorWindowModel SelectColor => GetModelInstance<SelectColorWindowModel>();
        /// <summary>
        /// 模型-主题设置
        /// </summary>
        public ThemeViewModel ThemeView => GetModelInstance<ThemeViewModel>();
        /// <summary>
        /// 模型-操作栏
        /// </summary>
        public OperateItemModel OperateItem => GetModelInstance<OperateItemModel>();
        /// <summary>
        /// 模型-双输入框通用窗体
        /// </summary>
        public ValueWindowModel ValueWindow => GetModelInstance<ValueWindowModel>();
        /// <summary>
        /// 模型-欢迎页
        /// </summary>
        public WelcomePageModel Welcome => GetModelInstance<WelcomePageModel>();

        /// <summary>
        /// 单实例模型已注册列表
        /// <para>用 object 而非 dynamic：dynamic 每次取值都要走 DLR 绑定，这里只有装箱后的引用</para>
        /// </summary>
        private static readonly Dictionary<string, object> viewModelList = new Dictionary<string, object>();
        /// <summary>
        /// 缓存字典的同步锁
        /// <para>GetModelInstance/GetViewInstance 是 public static，可能被多线程同时调用；
        /// Dictionary 并发写入会抛“已添加了具有相同键的项”，甚至在扩容时读到损坏的桶结构</para>
        /// </summary>
        private static readonly object cacheLock = new object();
        /// <summary>
        /// 创建单实例模型，如已存在时，从缓存列表获取
        /// </summary>
        public static T GetModelInstance<T>(string key = null) where T : class
        {
            //key 原实现未参与缓存键，传不同 key 会拿到同一个实例
            var name = $"{typeof(T).FullName}_{key}";
            lock (cacheLock)
            {
                if (!viewModelList.TryGetValue(name, out object obj))
                {
                    obj = Activator.CreateInstance<T>();
                    viewModelList.Add(name, obj);
                }
                return (T)obj;
            }
        }

        /// <summary>
        /// 单实例视图列表
        /// </summary>
        private static readonly Dictionary<string, object> dicView = new Dictionary<string, object>();
        /// <summary>
        /// 创建单实例视图，如已存在时，从缓存列表获取
        /// </summary>
        public static T GetViewInstance<T>(object key = null) where T : FrameworkElement
        {
            var name = $"{typeof(T).FullName}_{key}";
            lock (cacheLock)
            {
                if (dicView.TryGetValue(name, out object value))
                {
                    if (((FrameworkElement)value).DataContext is IPageReload pageReload) pageReload.PageReload();
                    return (T)value;
                }
                var obj = Activator.CreateInstance<T>();
                if (obj.DataContext is IPageReload reload) obj.Loaded += (sender, e) =>
                {
                    if (!reload.ILoad)
                    {
                        reload.ILoad = true;
                        reload.PageReload();
                    }
                };
                dicView.Add(name, obj);
                return obj;
            }
        }
        /// <summary>
        /// 清除所有实例与注册
        /// </summary>
        public static void Clear()
        {
            lock (cacheLock)
            {
                foreach (var model in viewModelList)
                {
                    WeakReferenceMessenger.Default.UnregisterAll(model.Value);
                }
                foreach (var view in dicView)
                {
                    WeakReferenceMessenger.Default.UnregisterAll(view.Value);
                }
                viewModelList.Clear();
                dicView.Clear();
            }
            //Statuitem 模型被状态栏 XAML 直接绑定且不在上述缓存中，这里不重置，
            //否则清除后仍在显示的状态栏将收不到消息
        }
    }
}