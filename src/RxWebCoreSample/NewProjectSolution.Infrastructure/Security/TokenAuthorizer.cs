using Microsoft.AspNetCore.Authentication.JwtBearer;
using RxWeb.Core.Security;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NewProjectSolution.Infrastructure.Singleton;
using NewProjectSolution.UnitOfWork.Main;

namespace NewProjectSolution.Infrastructure.Security
{
    public class TokenAuthorizer : ITokenAuthorizer
    {
        public TokenAuthorizer(IJwtTokenProvider tokenProvider, UserAccessConfigInfo userAccessConfigInfo)
        {
            TokenProvider = tokenProvider;
            UserAccessConfigInfo = userAccessConfigInfo;
        }

        public Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            throw new NotImplementedException();
        }

        public async Task Challenge(JwtBearerChallengeContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json;";
            await context.Response.WriteAsync("Error Has Occured.");
        }

        public Task MessageReceived(MessageReceivedContext context)
        {
            var principal = this.ValidateTokenAsync(context.HttpContext).Result;
            if (principal != null)
            {
                context.Principal = principal;
                context.Success();
            }
            else
                context.Fail("  Token Not Found");
            return Task.CompletedTask;
        }

        public Task TokenValidated(TokenValidatedContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization") && context.Request.Headers.ContainsKey("x-request"))
            {
                var loginUow = context.RequestServices.GetService(typeof(ILoginUow)) as ILoginUow;
                var securityKey = context.Request.Headers["x-request"];
                var token = context.Request.Headers["Authorization"];
                var dbToken = await UserAccessConfigInfo.GetTokenAsync(securityKey, loginUow);
                return dbToken == string.Empty ? null : this.TokenProvider.ValidateToken(securityKey, token);
            }
            return null;
        }

        private IJwtTokenProvider TokenProvider { get; set; }

        private UserAccessConfigInfo UserAccessConfigInfo { get; set; }

    }
}


