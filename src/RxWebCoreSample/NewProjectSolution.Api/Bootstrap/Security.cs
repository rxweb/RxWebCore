using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NewProjectSolution.Infrastructure.Security;
using NewProjectSolution.Models;
using RxWeb.Core.Extensions;
using RxWeb.Core.Security;
using RxWeb.Core.Security.Authorization;
using RxWeb.Core.Security.JwtToken;
using RxWeb.Core;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Linq;
using NewProjectSolution.BoundedContext.SqlContext;
using RxWeb.Core.Logging;

namespace NewProjectSolution.Api.Bootstrap
{
    public static class Security
    {
        readonly static string AllowMySpecificOrigins = "allowMySpecificOrigins";
        readonly static string SecuritySection = "Security";
        private static SecurityConfig SecurityConfig = new SecurityConfig();

        public static void AddSecurity(this IServiceCollection serviceCollection,IConfiguration configuration)
        {
			configuration.GetSection(SecuritySection).Bind(SecurityConfig);

            serviceCollection.AddSingleton<IAuthorizationPolicyProvider, AccessPolicyProvider>();
            serviceCollection.AddSingleton<IAuthorizationHandler, AccessPermissionHandler>();

            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(AllowMySpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(SecurityConfig.AllowedHosts).AllowAnyHeader()
                                .AllowAnyMethod().AllowCredentials();
                });
            });
            
            serviceCollection.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });


            serviceCollection.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
            serviceCollection.AddAuthorization();
            serviceCollection.AddRxWebJwtAuthentication();


        }

        public static void UseSecurity(this IApplicationBuilder applicationBuilder, IWebHostEnvironment environment)
        {

            if (environment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }
            else
            {
				if (SecurityConfig.AllowedIps != null && SecurityConfig.AllowedIps.Length > 0)
					applicationBuilder.SetIpSafeList();
                applicationBuilder.UseHsts();
                applicationBuilder.UseHttpsRedirection();
            }

			applicationBuilder.UseLogging(typeof(ILogDatabaseFacade));
			applicationBuilder.UseCors(AllowMySpecificOrigins);
            applicationBuilder.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
                Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always,
            });
            applicationBuilder.UseRouting();

            applicationBuilder.UseAuthentication();
            applicationBuilder.UseAuthorization();

            applicationBuilder.HandleException();

            applicationBuilder.SetSecurityHeaders();

        }


        private static void HandleException(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;
					var traceId = await exception.LogAsync(context);
                    context.Response.ContentType = "application/json;";
                    await context.Response.WriteAsync("Error Has Occured.");
                    // Log.Error(String.Format("Stacktrace of error: {0}", exception.StackTrace.ToString()));
                });
            });
        }

        private static void SetSecurityHeaders(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use((context, next) =>
            {
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-Content-Type-Options"] = "NOSNIFF";
                context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000";
                context.Response.Headers["X-Permitted-Cross-Domain-Policies"] = "master-only";
                context.Response.Headers["Content-Security-Policy"] = "default-src 'none'; style-src 'self'; img-src 'self'; font-src 'self'; script-src 'self'";
                return next();
            });
        }

        //copied from https://docs.microsoft.com/en-us/aspnet/core/security/ip-safelist?view=aspnetcore-3.0
        private static void SetIpSafeList(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Use(async (context, next) =>
            {
                var remoteIp = context.Connection.RemoteIpAddress;
                string[] ip = SecurityConfig.AllowedIps;

                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;
                foreach (var address in ip)
                {
                    var testIp = IPAddress.Parse(address);
                    if (testIp.GetAddressBytes().SequenceEqual(bytes))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
                await next();
            });
        }
    }
}




