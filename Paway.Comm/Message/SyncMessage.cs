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
        public string ClientId { get; set; }
        public ClientMessage() { }
        public ClientMessage(CommType type) : base(type)
        {
            if (Config.MQClient != null)
            {
                this.ClientId = Config.MQClient.ClientId;
            }
        }
    }
    /// <summary>
    /// 同步基础消息
    /// </summary>
    [Serializable]
    public class SyncBaseMessage : ClientMessage
    {
        public OperType OperType { get; set; }
        public SyncBaseMessage() : base(CommType.Sync) { }
    }
    /// <summary>
    /// 同步消息
    /// </summary>
    [Serializable]
    public class SyncMessage : SyncBaseMessage
    {
        public Type IType { get; set; }
        public IList List { get; set; }

        public SyncMessage() : base() { }
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
        public SyncInsertMessage() : base() { }
        public SyncInsertMessage(object obj) : base(obj, OperType.Insert) { }
    }
    /// <summary>
    /// 同步更新操作
    /// </summary>
    [Serializable]
    public class SyncUpdateMessage : SyncMessage
    {
        public string[] Args { get; set; }
        public SyncUpdateMessage() : base() { }
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
        public SyncDeleteMessage() : base() { }
        public SyncDeleteMessage(object obj) : base(obj, OperType.Delete) { }
    }

    /// <summary>
    /// 同步查询操作
    /// </summary>
    [Serializable]
    public class SyncFindMessage : SyncBaseMessage
    {
        public Type IType { get; set; }
        public int Count { get; set; }
        public string Find { get; set; }
        public string[] Args { get; set; }

        public SyncFindMessage() : base()
        {
            this.OperType = OperType.Query;
        }
        public SyncFindMessage(Type type, string find, int count, params string[] args) : this()
        {
            this.IType = type;
            this.Count = count;
            this.Find = find;
            this.Args = args;
        }
    }


    [Serializable]
    public class SyncExecuteScalarMessage : ClientMessage
    {
        public string Sql { get; set; }

        public SyncExecuteScalarMessage() : base(CommType.SyncExecuteScalar) { }
        public SyncExecuteScalarMessage(CommType type) : base(type) { }
        public SyncExecuteScalarMessage(string sql) : this()
        {
            this.Sql = sql;
        }
    }
    [Serializable]
    public class SyncExecuteListMessage : SyncExecuteScalarMessage
    {
        public Type ObjType { get; set; }

        public SyncExecuteListMessage() : base(CommType.SyncExecuteList) { }
        public SyncExecuteListMessage(Type type, string sql) : this()
        {
            this.ObjType = type;
            this.Sql = sql;
        }
    }
}
