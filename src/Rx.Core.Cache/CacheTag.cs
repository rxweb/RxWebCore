using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Rx.AspNetCore.Cache.Constants;
using Rx.AspNetCore.Cache.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Rx.AspNetCore.Cache
{
    public class CacheTag : ActionFilterAttribute
    {

        public Type Controller { get; set; }

        public int CacheMinutes { get; set; }

        public CacheTag()
        {
        }

        public static ConcurrentDictionary<string, TagCache> etags = new ConcurrentDictionary<string, TagCache>();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (Controller == null)
                Controller = context.Controller.GetType();
            TagCache tagCache;
            var request = context.HttpContext.Request;
            if (request.Method.ToUpper() == RequestMethod.Get)
            {
                var key = GetKey(request);
                if (etags.TryGetValue(key, out tagCache))
                {
                    if (context.HttpContext.Request.Headers.Keys.Contains(HeaderNames.IfNoneMatch) && context.HttpContext.Request.Headers[HeaderNames.IfNoneMatch].ToString() == tagCache.Etag)
                    {
                        context.Result = new StatusCodeResult(304);
                    }
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var key = GetKey(request);
            string etag = string.Empty;
            var isGet = request.Method == RequestMethod.Get;
            var isPutOrPost = request.Method == RequestMethod.Put || request.Method == RequestMethod.Post || request.Method == RequestMethod.Patch || request.Method == RequestMethod.Delete;
            TagCache tagCache;
            if (context.HttpContext.Response.StatusCode == 200 && (isGet) || isPutOrPost)
            {
                if (isGet)
                {
                    if (!etags.TryGetValue(key, out tagCache))
                    {
                        etag = Guid.NewGuid().ToString();
                        tagCache = new TagCache
                        {
                            Etag = etag,
                            Controller = Controller
                        };
                        etags.AddOrUpdate(key, tagCache, (k, val) => tagCache);
                    }
                    else {
                        etag = tagCache.Etag;
                    }
                    if (!context.HttpContext.Response.Headers.Keys.Contains("ETag"))
                        context.HttpContext.Response.Headers.Add("ETag", etag);
                    SetResponseCacheControl(context.HttpContext.Response);
                }
                else
                {
                    RemoveRelatedKeys();
                }
            }
        }

        private string GetKey(HttpRequest request)
        {
            return request.Path.ToString().ToLower();
        }

        private void RemoveRelatedKeys()
        {
            var controllerKeys = etags.Where(t => t.Value.Controller == Controller).ToList();
            foreach (var key in controllerKeys)
            {
                TagCache tagCache;
                etags.TryRemove(key.Key, out tagCache);
            }
        }

        private void SetResponseCacheControl(HttpResponse response)
        {
            if (CacheMinutes > 0) {
                response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
                {
                    MaxAge = TimeSpan.FromMinutes(CacheMinutes),
                    MustRevalidate = false,
                    Private = true
                };
            }
            
        }
    }
}
