using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using PostSharp.Aspects;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Aspects.Postsharp.CacheAspects
{
    [Serializable]
    public class CacheAspect : MethodInterceptionAspect
    {
        private Type cacheType;
        private int cacheMinutes;
        private ICacheManager cacheManager;

        public CacheAspect(Type cacheType = null, int cacheMinutes = 60)
        {
            this.cacheType = cacheType;
            this.cacheMinutes = cacheMinutes;
        }

        public override void RuntimeInitialize(MethodBase method)
        {
            if (cacheType == null) cacheType = typeof(MemoryCacheManager);

            if (typeof(ICacheManager).IsAssignableFrom(cacheType) == false)
            {
                throw new Exception("Wrong Cache Manager");
            }

            cacheManager = (ICacheManager)Activator.CreateInstance(cacheType);

            base.RuntimeInitialize(method);
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var methodName = string.Format("{0}.{1}.{2}",
                args.Method.ReflectedType.Namespace,
                args.Method.ReflectedType.Name,
                args.Method.Name);

            var arguments = args.Arguments.ToList();

            var key = string.Format("{0}({1})", methodName,
                string.Join(",", arguments.Select(x => x != null ? x.ToString() : "<Null>")));

            if (cacheManager.Contains(key))
            {
                args.ReturnValue = cacheManager.Get<object>(key);
            }

            base.OnInvoke(args);

            cacheManager.Add(key, args.ReturnValue, cacheMinutes);
        }
    }
}
