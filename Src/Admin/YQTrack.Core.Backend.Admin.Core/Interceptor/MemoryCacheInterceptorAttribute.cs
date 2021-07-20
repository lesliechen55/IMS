using System;
using System.Linq;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Caching.Memory;

namespace YQTrack.Core.Backend.Admin.Core.Interceptor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class MemoryCacheInterceptorAttribute : AbstractInterceptorAttribute
    {
        private readonly int _seconds;
        private readonly bool _ignoreParameter;
        private readonly bool _isAbsoluteExpire;

        /// <summary>
        /// 进程内存级别缓存拦截器,默认缓存时间为30秒
        /// </summary>
        /// <param name="seconds">缓存时间秒级别</param>
        /// <param name="ignoreParameter">缓存key是否忽略参数</param>
        /// <param name="isAbsoluteExpire">是否绝对过期</param>
        public MemoryCacheInterceptorAttribute(int seconds = 30, bool ignoreParameter = false, bool isAbsoluteExpire = true)
        {
            if (seconds <= 0) seconds = 30;
            _seconds = seconds;
            _ignoreParameter = ignoreParameter;
            _isAbsoluteExpire = isAbsoluteExpire;
        }

        [FromContainer]
        public IMemoryCache MemoryCache { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var cacheKey = GenerateCacheKey($"{context.ServiceMethod.DeclaringType.FullName}_{context.ProxyMethod.Name}", context.Parameters, _ignoreParameter);
            if (MemoryCache.TryGetValue(cacheKey, out var value))
            {
                context.ReturnValue = value;
            }
            else
            {
                await next(context);
                var item = context.ReturnValue;
                var options = new MemoryCacheEntryOptions();
                if (_isAbsoluteExpire)
                {
                    options.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(_seconds);
                }
                else
                {
                    options.SlidingExpiration = TimeSpan.FromSeconds(_seconds);
                }
                MemoryCache.Set(cacheKey, item, options);
            }
        }

        private static string GenerateCacheKey(string name, object[] arguments, bool ignoreParameter)
        {
            if (arguments == null || arguments.Length == 0 || ignoreParameter)
                return name;
            var key = $"{name}_{string.Join("_", arguments.Where(x => x != null).Select(a => a.ToString()).ToArray())}";
            return key;
        }
    }
}