using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
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
        /// <summary>
        /// 视图-模型管理器
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }
        internal static ViewModelLocator Default => new ViewModelLocator();

        /// <summary>
        /// 模型-主题设置
        /// </summary>
        public ThemeViewModel ThemeView => GetModelInstance<ThemeViewModel>();
        /// <summary>
        /// 模型-颜色搭取器
        /// </summary>
        public SelectColorWindowModel SelectColor => GetModelInstance<SelectColorWindowModel>();
        /// <summary>
        /// 模型-单输入框通用窗体
        /// </summary>
        public NameWindowModel NameWindow => GetModelInstance<NameWindowModel>();
        /// <summary>
        /// 模型-双输入框通用窗体
        /// </summary>
        public ValueWindowModel ValueWindow => GetModelInstance<ValueWindowModel>();
        /// <summary>
        /// 模型-状态栏
        /// </summary>
        public StatuItemModel StatuItem => GetModelInstance<StatuItemModel>();
        /// <summary>
        /// 模型-欢迎页
        /// </summary>
        public WelcomePageModel Welcome => GetModelInstance<WelcomePageModel>();

        /// <summary>
        /// 单实例模型已注册列表
        /// </summary>
        private static readonly List<string> viewModelList = new List<string>();
        /// <summary>
        /// 创建单实例模型，如已存在时，从缓存列表获取
        /// </summary>
        public static T GetModelInstance<T>(string key = null) where T : class
        {
            var name = typeof(T).FullName;
            if (!viewModelList.Contains(name))
            {
                SimpleIoc.Default.Register<T>();
                viewModelList.Add(name);
            }
            return ServiceLocator.Current.GetInstance<T>(key);
        }

        /// <summary>
        /// 单实例视图列表
        /// </summary>
        private static readonly Dictionary<string, dynamic> dicView = new Dictionary<string, dynamic>();
        /// <summary>
        /// 创建单实例视图，如已存在时，从缓存列表获取
        /// </summary>
        public T GetViewInstance<T>() where T : FrameworkElement
        {
            var type = typeof(T);
            var name = type.FullName;
            if (!dicView.ContainsKey(name))
            {
                var obj = Activator.CreateInstance<T>();
                if (obj.DataContext is IPageReload pageReload) obj.Loaded += (sender, e) =>
                {
                    if (!pageReload.ILoad)
                    {
                        pageReload.ILoad = true;
                        pageReload.PageReload();
                    }
                };
                dicView.Add(name, obj);
            }
            else if (((FrameworkElement)dicView[name]).DataContext is IPageReload pageReload)
            {
                pageReload.PageReload();
            }
            return dicView[name];
        }
    }
}