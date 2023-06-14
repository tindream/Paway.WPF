using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace WpfWebBrowser
{
    public static class ReflectionService
    {
        public readonly static BindingFlags BindingFlags =
            BindingFlags.Instance |
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.FlattenHierarchy |
            BindingFlags.CreateInstance;

        public static object ReflectGetProperty(this object target, string propertyName)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("propertyName can not be null or whitespace", "propertyName");

            var propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags);
            if (propertyInfo == null)
                throw new ArgumentException(string.Format("Can not find property '{0}' on '{1}'", propertyName, target.GetType()));
            return propertyInfo.GetValue(target, null);
        }

        public static object ReflectInvokeMethod(this object target, string methodName, Type[] argTypes, object[] parameters)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (string.IsNullOrWhiteSpace(methodName))
                throw new ArgumentException("methodName can not be null or whitespace", "methodName");

            var methodInfo = target.GetType().GetMethod(methodName, BindingFlags, null, argTypes, null);
            if (methodInfo == null)
                throw new ArgumentException(string.Format("Can not find method '{0}' on '{1}'", methodName, target.GetType()));
            return methodInfo.Invoke(target, parameters);
        }
    }
}
