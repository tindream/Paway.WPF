using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using Paway.Helper;
using Paway.WPF;

namespace Paway.WPF
{
    /// <summary>
    /// 多语言处理
    /// </summary>
    public static class LanguageHelper
    {
        private static readonly object languageLock = new object();
        /// <summary>
        /// 重置(选择)语言
        /// </summary>
        public static void Reset<T>(string file, T languageInfo, string languageName) where T : LanguageBaseInfo
        {
            lock (languageLock)
            {
                Reset(languageInfo, languageName);
                if (File.Exists(file))
                {
                    try
                    {
                        var lan = XmlHelper.Load<T>(file);
                        lan.Clone(languageInfo);
                        PConfig.InitLanguageBase(lan);
                    }
                    catch (Exception ex)
                    {
                        ex.Log();
                    }
                }
                XmlHelper.Save(languageInfo, file);
            }
        }

        #region 注册文本特性事件，使用语言包
        private static object languageObj;
        private static string languageName;
        /// <summary>
        /// 枚举描述缓存
        /// </summary>
        private static readonly ConcurrentDictionary<string, string> LanguageCache = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 重置枚举
        /// </summary>
        private static void Reset<T>(T languageInfo, string languageName)
        {
            LanguageHelper.languageObj = languageInfo;
            LanguageHelper.languageName = languageName;
            EnumHelper.DescriptionAttrEvent -= ConverHelper_DescriptionAttrEvent;
            EnumHelper.DescriptionAttrEvent += ConverHelper_DescriptionAttrEvent;
            AttributeHelper.TextAttrEvent -= ConverHelper_DescriptionAttrEvent;
            AttributeHelper.TextAttrEvent += ConverHelper_DescriptionAttrEvent;
        }
        private static void ConverHelper_DescriptionAttrEvent(DescriptionEventArgs obj)
        {
            var key = $"{obj.Name}_{languageName}";
            if (!LanguageCache.ContainsKey(key))
            {
                if (languageObj.Property(obj.Name) != null && languageObj.GetValue(obj.Name) is string language)
                    LanguageCache.TryAdd(key, language);
                else LanguageCache.TryAdd(key, null);
            }
            obj.Result = LanguageCache[key];
        }

        #endregion
    }
}
