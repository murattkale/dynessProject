using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using PostSharp.Aspects;
using System;
using System.Reflection;

namespace Core.Aspects.Postsharp.CacheAspects
{
    [Serializable]
    public class CacheRemoveAspect : OnMethodBoundaryAspect
    {
        private Type cacheType;
        private string cachePattern;
        private ICacheManager cacheManager;

        public CacheRemoveAspect(Type cacheType = null)
        {
            this.cacheType = cacheType;
        }

        public CacheRemoveAspect(string cachePattern, Type cacheType = null) : this(cacheType)
        {
            this.cachePattern = cachePattern;
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
        public override void OnSuccess(MethodExecutionArgs args)
        {
            if (string.IsNullOrEmpty(cachePattern))
                cachePattern = string.Format("{0}.{1}.*", args.Method.ReflectedType.Namespace, args.Method.ReflectedType.Name);

            cacheManager.DeleteByPattern(cachePattern);
        }
    }
}
