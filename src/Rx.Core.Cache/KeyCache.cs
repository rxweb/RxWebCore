using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Cache
{
    public class KeyCache : ActionFilterAttribute
    {
        private IMemoryCache MemoryCache;

        private int Days { get; set; }
        public KeyCache(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
            Days = 10;
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cacheKeyName = GetCacheKeyName(context.HttpContext.Request);
            object value;
            if (MemoryCache.TryGetValue(cacheKeyName, out value))
            {
                SetResponseCacheControl(context.HttpContext.Response);
                context.Result = new ObjectResult(value);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var cacheKeyName = GetCacheKeyName(context.HttpContext.Request);
            MemoryCache.Set<object>(cacheKeyName, ((ObjectResult)context.Result).Value);
            SetResponseCacheControl(context.HttpContext.Response);
        }

        private string GetCacheKeyName(HttpRequest request)
        {
            if (request.QueryString != null)
                return (request.Path.ToString() + request.QueryString.Value).ToLower();
            return request.Path.ToString().ToLower();
        }

        private void SetResponseCacheControl(HttpResponse response)
        {
            response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromDays(Days),
                MustRevalidate = false,
                Private = true
            };
        }

    }
}
