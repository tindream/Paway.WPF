using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Paway.Model
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public StatuItemModel StatuItem => GetModelInstance<StatuItemModel>();
        public NameWindowModel Name => GetModelInstance<NameWindowModel>();
        public WelcomePageModel Welcome => GetModelInstance<WelcomePageModel>();

        /// <summary>
        /// 单实例模型已注册列表
        /// </summary>
        private static readonly List<string> viewModelList = new List<string>();
        protected T GetModelInstance<T>(string key = null) where T : class
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