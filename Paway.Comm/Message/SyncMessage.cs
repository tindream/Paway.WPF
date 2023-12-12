using Paway.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Paway.Comm
{
    /// <summary>
    /// 客户端基础消息(不回送到本客户端)
    /// </summary>
    [Serializable]
    public class ClientMessage : CommMessage
    {
        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 客户端基础消息(不回送到本客户端)
        /// </summary>
        public ClientMessage() { }
        /// <summary>
        /// 客户端基础消息(不回送到本客户端)
        /// </summary>
        public ClientMessage(CommType type) : base(type)
        {
            if (CConfig.MQClient != null)
            {
                this.ClientId = CConfig.MQClient.ClientId;
            }
        }
    }
    /// <summary>
    /// 同步基础消息
    /// </summary>
    [Serializable]
    public class SyncBaseMessage : ClientMessage
    {
        /// <summary>
        /// 查询类型
        /// </summary>
        public OperType OperType { get; set; }
        /// <summary>
        /// 同步基础消息
        /// </summary>
        public SyncBaseMessage() : base(CommType.Sync) { }
    }
    /// <summary>
    /// 同步消息
    /// </summary>
    [Serializable]
    public class SyncMessage : SyncBaseMessage
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type IType { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        public IList List { get; set; }

        /// <summary>
        /// 同步消息
        /// </summary>
        public SyncMessage() : base() { }
        /// <summary>
        /// 同步消息
        /// </summary>
        public SyncMessage(object obj, OperType type) : this()
        {
            if (obj == null) throw new WarningException("参数为空");
            if (obj is IList list)
            {
                this.List = list;
                this.IType = this.List.GenericType();
            }
            else if (obj != null)
            {
                this.IType = obj.GetType();
                this.List = this.IType.GenericList();
                this.List.Add(obj);
            }
            this.OperType = type;
        }
    }
    /// <summary>
    /// 同步插入操作
    /// </summary>
    [Serializable]
    public class SyncInsertMessage : SyncMessage
    {
        /// <summary>
        /// 同步插入操作
        /// </summary>
        public SyncInsertMessage() : base() { }
        /// <summary>
        /// 同步插入操作
        /// </summary>
        public SyncInsertMessage(object obj) : base(obj, OperType.Insert) { }
    }
    /// <summary>
    /// 同步更新操作
    /// </summary>
    [Serializable]
    public class SyncUpdateMessage : SyncMessage
    {
        /// <summary>
        /// 更新字段参数列表
        /// </summary>
        public string[] Args { get; set; }
        /// <summary>
        /// 同步更新操作
        /// </summary>
        public SyncUpdateMessage() : base() { }
        /// <summary>
        /// 同步更新操作
        /// </summary>
        public SyncUpdateMessage(object obj, params string[] args) : base(obj, OperType.Update)
        {
            this.Args = args;
        }
    }
    /// <summary>
    /// 同步删除操作
    /// </summary>
    [Serializable]
    public class SyncDeleteMessage : SyncMessage
    {
        /// <summary>
        /// 同步删除操作
        /// </summary>
        public SyncDeleteMessage() : base() { }
        /// <summary>
        /// 同步删除操作
        /// </summary>
        public SyncDeleteMessage(object obj) : base(obj, OperType.Delete) { }
    }

    /// <summary>
    /// 同步查询操作
    /// </summary>
    [Serializable]
    public class SyncFindMessage : SyncBaseMessage
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public Type IType { get; set; }
        /// <summary>
        /// 查询数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public string Find { get; set; }
        /// <summary>
        /// 查询字段参数列表
        /// </summary>
        public string[] Args { get; set; }

        /// <summary>
        /// 同步查询操作
        /// </summary>
        public SyncFindMessage() : base()
        {
            this.OperType = OperType.Query;
        }
        /// <summary>
        /// 同步查询操作
        /// </summary>
        public SyncFindMessage(Type type, string find, int count, params string[] args) : this()
        {
            this.IType = type;
            this.Count = count;
            this.Find = find;
            this.Args = args;
        }
    }


    /// <summary>
    /// 同步执行SQL
    /// </summary>
    [Serializable]
    public class SyncExecuteScalarMessage : ClientMessage
    {
        /// <summary>
        /// SQL语句
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 同步执行SQL
        /// </summary>
        public SyncExecuteScalarMessage() : base(CommType.SyncExecuteScalar) { }
        /// <summary>
        /// 同步执行SQL
        /// </summary>
        public SyncExecuteScalarMessage(CommType type) : base(type) { }
        /// <summary>
        /// 同步执行SQL
        /// </summary>
        public SyncExecuteScalarMessage(string sql) : this()
        {
            this.Sql = sql;
        }
    }
    /// <summary>
    /// 同步查询列表
    /// </summary>
    [Serializable]
    public class SyncExecuteListMessage : SyncExecuteScalarMessage
    {
        /// <summary>
        /// 查询类型
        /// </summary>
        public Type ObjType { get; set; }

        /// <summary>
        /// 同步查询列表
        /// </summary>
        public SyncExecuteListMessage() : base(CommType.SyncExecuteList) { }
        /// <summary>
        /// 同步查询列表
        /// </summary>
        public SyncExecuteListMessage(Type type, string sql) : this()
        {
            this.ObjType = type;
            this.Sql = sql;
        }
    }
}
