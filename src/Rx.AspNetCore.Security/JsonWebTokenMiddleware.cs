using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Rx.AspNetCore.Security.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security
{
    public class JsonWebTokenMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ITokenValidator TokenProvider;

        private readonly IApplicationAuthorization ApplicationAuthorization;

        private HttpContext HttpContext { get; set; }

        internal static List<string> ByPassAuthenticationControllers { get; set; } = new List<string>();

        internal static List<string> ByPassAuthorizationControllers { get; set; } = new List<string>();

        internal static Dictionary<string, List<string>> AnonymousApiControllers { get; set; } = new Dictionary<string, List<string>>();

        public JsonWebTokenMiddleware(RequestDelegate next, ITokenValidator tokenProvider, IApplicationAuthorization applicationAuthorization)
        {
            _next = next;
            TokenProvider = tokenProvider;
            ApplicationAuthorization = applicationAuthorization;
        }

        private bool IsByPassUri(List<string> byPassObject,string path)
        {
            var isMatched = false;
            foreach (var byPass in byPassObject)
            {
                isMatched = path.Contains(byPass);
                if (isMatched) break;
            }
            return isMatched;
        }

        private bool IsAnonymousAccess(string path,string requestMethod) {
            var isMatched = false;
            foreach (var byPass in AnonymousApiControllers) {
                isMatched = path.Contains(byPass.Key);
                if (isMatched)
                {
                    isMatched = byPass.Value.Contains(requestMethod.ToUpper());
                    if (isMatched) break;
                }
            }
            return isMatched;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var next = _next;
            if (httpContext.Request.Headers.Keys.Contains(HeaderNames.Authorization))
            {
                var isSuccess = TokenProvider.Validate(httpContext);
                if (isSuccess)
                {
                    var isAnonymous = Convert.ToString(UserClaim.Get(ClaimTypes.Actor));
                    var isAnonymousPassed = false;
                    if (!string.IsNullOrEmpty(isAnonymous) && Convert.ToBoolean(isAnonymous))
                        isAnonymousPassed = IsAnonymousAccess(httpContext.Request.Path.ToString().ToLower(), httpContext.Request.Method);
                    if (!isAnonymousPassed && !IsByPassUri(JsonWebTokenMiddleware.ByPassAuthorizationControllers, httpContext.Request.Path.ToString().ToLower()))
                    {
                        var isAuthorize = ApplicationAuthorization.Validate(httpContext);
                        if (!(bool)isAuthorize["Status"])
                        {
                            httpContext.Response.StatusCode = isAuthorize.Keys.Contains("StatusCode") ? (int)isAuthorize["StatusCode"] : Convert.ToInt32(HttpStatusCode.Unauthorized);
                            var responseMessage = isAuthorize.Keys.Contains("Message") ? (string)isAuthorize["Message"] : "UnAuthorized Access";
                            await httpContext.Response.WriteAsync(responseMessage).ConfigureAwait(false);
                            return;
                        }
                    }
                }
                else
                {
                    httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.ExpectationFailed);
                    await httpContext.Response.WriteAsync("Token has expired").ConfigureAwait(false);
                    return;
                }
            }
            else if (!IsByPassUri(JsonWebTokenMiddleware.ByPassAuthenticationControllers, httpContext.Request.Path.ToString().ToLower()))
            {
                httpContext.Response.StatusCode = Convert.ToInt32(HttpStatusCode.NotAcceptable);
                await httpContext.Response.WriteAsync("Token is not available").ConfigureAwait(false);
                return;
            }
            await _next(httpContext);
        }
    }

    public static class JsonWebTokenMiddlewareExtension
    {
        public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder builder, List<string> byPassAuthenticationControllers, List<string> byPassAuthorizationControllers, Dictionary<string,List<string>> anonymousControllers)
        {
            byPassAuthenticationControllers.ForEach(t => JsonWebTokenMiddleware.ByPassAuthenticationControllers.Add(t.Replace("Controller", string.Empty).ToLower()));
            byPassAuthorizationControllers.ForEach(t => JsonWebTokenMiddleware.ByPassAuthorizationControllers.Add(t.Replace("Controller", string.Empty).ToLower()));
            foreach(var anonymous in anonymousControllers)
                JsonWebTokenMiddleware.AnonymousApiControllers.Add(string.Format("/{0}", anonymous.Key.Replace("Controller", string.Empty).ToLower()),anonymous.Value);
            return builder.UseMiddleware<JsonWebTokenMiddleware>();
        }
    }
}
