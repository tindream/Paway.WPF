using Paway.Helper;
using Paway.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Paway.WPF
{
    /// <summary>
    /// 语言包定义
    /// </summary>
    [Serializable]
    public class LanguageBaseInfo : BaseModelInfo
    {
        #region 输入
        /// <summary>
        /// 请输入
        /// </summary>
        [Description("请输入")]
        public virtual string PleaseInput { get; set; } = "请输入";
        /// <summary>
        /// 请输入
        /// </summary>
        [Description("请输入(水印)")]
        public virtual string PleaseInputWater { get; set; } = "请输入..";
        /// <summary>
        /// 请输入用户名
        /// </summary>
        [Description("请输入用户名")]
        public virtual string PleaseInputUserNameWater { get; set; } = "请输入用户名";
        /// <summary>
        /// 请输入密码
        /// </summary>
        [Description("请输入密码")]
        public virtual string PleaseInputPasswordWater { get; set; } = "请输入密码";
        /// <summary>
        /// 登录
        /// </summary>
        [Description("登录")]
        public virtual string Login { get; set; } = "登录";
        /// <summary>
        /// 欢迎使用
        /// </summary>
        [Description("欢迎使用")]
        public virtual string Welcome { get; set; } = "欢迎使用";

        /// <summary>
        /// 不可为空
        /// </summary>
        [Description("不可为空")]
        public virtual string CannotBeEmpty { get; set; } = "不可为空";
        /// <summary>
        /// 最小输入限制: {0}位
        /// </summary>
        [Description("最小输入限制: {0}位")]
        public virtual string MinimumInputLimit { get; set; } = "最小输入限制: {0}位";
        /// <summary>
        /// 最大输入限制: {0}位
        /// </summary>
        [Description("最大输入限制: {0}位")]
        public virtual string MaximumInputLimit { get; set; } = "最大输入限制: {0}位";

        #endregion

        #region 日历
        /// <summary>
        /// 今天
        /// </summary>
        [Description("今天")]
        public virtual string Today { get; set; } = "今天";
        /// <summary>
        /// 清空
        /// </summary>
        [Description("清空")]
        public virtual string Clear { get; set; } = "清空";

        #endregion

        #region 系统
        /// <summary>
        /// 关于
        /// </summary>
        [Description("关于")]
        public virtual string About { get; set; } = "关于";
        /// <summary>
        /// 主题
        /// </summary>
        [Description("主题")]
        public virtual string Theme { get; set; } = "主题";
        /// <summary>
        /// 颜色
        /// </summary>
        [Description("颜色")]
        public virtual string Color { get; set; } = "颜色";

        #endregion

        #region 其它
        /// <summary>
        /// 添加
        /// </summary>
        [Description("添加")]
        public virtual string Add { get; set; } = "添加";
        /// <summary>
        /// 已存在
        /// </summary>
        [Description("已存在")]
        public virtual string Exist { get; set; } = "已存在";
        /// <summary>
        /// 已存在
        /// </summary>
        [Description("请选择")]
        public virtual string PleaseSelect { get; set; } = "请选择";
        /// <summary>
        /// 确认删除
        /// </summary>
        [Description("确认删除")]
        public virtual string ConfirmDelete { get; set; } = "确认删除";
        /// <summary>
        /// 查询
        /// </summary>
        [Description("查询")]
        public virtual string Query { get; set; } = "查询";
        /// <summary>
        /// 取消查询
        /// </summary>
        [Description("取消查询")]
        public virtual string QueryCancel { get; set; } = "取消查询";
        /// <summary>
        /// 再按一次取消查询
        /// </summary>
        [Description("再按一次取消查询")]
        public virtual string QueryCancelAgain { get; set; } = "再按一次取消查询";
        /// <summary>
        /// 选择要导入的文件
        /// </summary>
        [Description("选择要导入的文件")]
        public virtual string SelectImportFile { get; set; } = "选择要导入的文件";
        /// <summary>
        /// 正在导入..
        /// </summary>
        [Description("正在导入..")]
        public virtual string Importing { get; set; } = "正在导入..";
        /// <summary>
        /// 导入失败
        /// </summary>
        [Description("导入失败")]
        public virtual string ImportError { get; set; } = "导入失败";
        /// <summary>
        /// 导入成功
        /// </summary>
        [Description("导入成功")]
        public virtual string ImportSuccess { get; set; } = "导入成功";

        /// <summary>
        /// /选择要保存的文件位置
        /// </summary>
        [Description("选择要保存的文件位置")]
        public virtual string SelectFileLocation { get; set; } = "选择要保存的文件位置";
        /// <summary>
        /// 正在导出..
        /// </summary>
        [Description("正在导出..")]
        public virtual string Exporting { get; set; } = "正在导出..";
        /// <summary>
        /// 导出成功
        /// </summary>
        [Description("导出成功")]
        public virtual string ExportSuccess { get; set; } = "导出成功";
        /// <summary>
        /// 导出成功, 是否打开文件?
        /// </summary>
        [Description("导出成功, 是否打开文件?")]
        public virtual string ExportSuccessAndOpen { get; set; } = "导出成功, 是否打开文件?";

        #endregion
    }
    /// <summary>
    /// 注入拦截器类-触发更新事件
    /// </summary>
    public class InterceptorNotify
    {
        /// <summary>
        /// 注入拦截器类-触发更新事件
        /// </summary>
        public object Invoke(object obj, string method, object[] parameters)
        {
            var retobj = obj.GetType().GetMethod(method + "_Base").Invoke(obj, parameters);
            if (method.StartsWith("set_"))
            {
                if (obj is IPropertyChanged notify) notify.OnPropertyChanged(method.Substring(4));
            }
            return retobj;
        }
    }
}
