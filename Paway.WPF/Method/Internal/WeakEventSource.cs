using Paway.Helper;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Paway.WPF
{
    /// <summary>
    /// 弱引用事件源
    /// <para>PConfig 的主题颜色/字体事件是静态事件，委托链本身就是 GC Root，
    /// 订阅者若不显式退订会永久存活（BrushEXT、ThemeForeground 等按控件实例创建，会逐次累积）。</para>
    /// <para>注意：不能直接用 WeakReference 包裹委托——委托对象只被弱引用持有时会被立即回收，
    /// 订阅会静默失效。因此这里弱引用的是方法的 Target 对象，方法本身用 MethodInfo 记录。</para>
    /// </summary>
    /// <typeparam name="T">事件参数类型</typeparam>
    internal sealed class WeakEventSource<T>
    {
        /// <summary>
        /// 订阅项：弱引用目标对象 + 方法信息
        /// </summary>
        private sealed class Subscription
        {
            private readonly WeakReference target;
            private readonly MethodInfo method;

            public Subscription(Delegate handler)
            {
                this.target = new WeakReference(handler.Target);
                this.method = handler.Method;
            }
            public bool IsAlive
            {
                get { return this.method.IsStatic || this.target.IsAlive; }
            }
            public bool IsSame(Delegate handler)
            {
                return this.method == handler.Method && ReferenceEquals(this.target.Target, handler.Target);
            }
            public void Invoke(T arg)
            {
                try
                {
                    this.method.Invoke(this.method.IsStatic ? null : this.target.Target, new object[] { arg });
                }
                catch (TargetInvocationException ex)
                {
                    //单个订阅者异常不应中断其余订阅者；反射调用会包一层壳，取内层真实异常
                    (ex.InnerException ?? ex).Log();
                }
                catch (Exception ex)
                {
                    ex.Log();
                }
            }
        }

        private readonly List<Subscription> list = new List<Subscription>();

        /// <summary>
        /// 弱引用订阅
        /// </summary>
        /// <param name="handler">事件处理方法，需为实例方法（静态方法按强引用处理）</param>
        public void Subscribe(Action<T> handler)
        {
            if (handler == null) return;
            lock (this.list)
            {
                for (var i = this.list.Count - 1; i >= 0; i--)
                {
                    //去重并顺手清理已回收项，避免同一实例重复回调
                    if (this.list[i].IsSame(handler)) return;
                    if (!this.list[i].IsAlive) this.list.RemoveAt(i);
                }
                this.list.Add(new Subscription(handler));
            }
        }

        /// <summary>
        /// 触发事件，同时清理已回收的订阅者
        /// </summary>
        /// <param name="arg">事件参数</param>
        public void Raise(T arg)
        {
            Subscription[] snapshot;
            lock (this.list)
            {
                for (var i = this.list.Count - 1; i >= 0; i--)
                {
                    if (!this.list[i].IsAlive) this.list.RemoveAt(i);
                }
                if (this.list.Count == 0) return;
                snapshot = this.list.ToArray();
            }
            //在锁外回调，避免回调内部再次订阅造成死锁
            for (var i = snapshot.Length - 1; i >= 0; i--)
            {
                snapshot[i].Invoke(arg);
            }
        }
    }
}
