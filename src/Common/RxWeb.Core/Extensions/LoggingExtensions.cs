using Microsoft.AspNetCore.Builder;
using RxWeb.Core.Logging;
using System;

namespace RxWeb.Core.Extensions
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder builder, Type databaseFacade)
        {
            return builder.UseMiddleware<RequestLogMiddleware>(databaseFacade);
        }
    }

}
