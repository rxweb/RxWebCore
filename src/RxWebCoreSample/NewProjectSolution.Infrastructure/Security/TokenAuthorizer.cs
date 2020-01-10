using Microsoft.AspNetCore.Authentication.JwtBearer;
using RxWeb.Core.Security;
using System;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NewProjectSolution.Infrastructure.Singleton;
using NewProjectSolution.UnitOfWork.Main;
using System.Collections.Generic;

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
                List<Claim> claimCollection = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Andras")
                , new Claim(ClaimTypes.Country, "Sweden")
                , new Claim(ClaimTypes.Gender, "M")
                , new Claim(ClaimTypes.Surname, "Nemes")
                , new Claim(ClaimTypes.Email, "hello@me.com")
                , new Claim(ClaimTypes.Role, "IT")
            };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimCollection, "My e-commerce website");
                context.Principal = new ClaimsPrincipal(claimsIdentity);
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
            if (context.Request.Headers.TryGetValue(AUTHORIZATION_HEADER,out var token) && context.Request.Cookies.TryGetValue("request_identity", out var requestIdentity))
            {
                var loginUow = context.RequestServices.GetService(typeof(ILoginUow)) as ILoginUow;
                var dbToken = await UserAccessConfigInfo.GetTokenAsync(requestIdentity, loginUow);
                return string.IsNullOrEmpty(dbToken) ? null : this.TokenProvider.ValidateToken(requestIdentity, token);
            }
            return null;
        }

        public ClaimsPrincipal AnonymousUserValidateToken(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(AUTHORIZATION_HEADER, out var token) && context.Request.Cookies.TryGetValue("anonymous", out var anonymousUser))
                return this.TokenProvider.ValidateToken(anonymousUser, token);
            return null;
        }

        private IJwtTokenProvider TokenProvider { get; set; }

        private UserAccessConfigInfo UserAccessConfigInfo { get; set; }

        private const string AUTHORIZATION_HEADER = "Authorization";

    }
}


