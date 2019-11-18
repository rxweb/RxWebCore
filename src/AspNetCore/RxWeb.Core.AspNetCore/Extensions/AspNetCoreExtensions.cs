using Microsoft.AspNetCore.Mvc;
using RxWeb.Core.AspNetCore.Conventions;
using RxWeb.Core.AspNetCore.Formatters;
using RxWeb.Core.Filters;

namespace RxWeb.Core.AspNetCore.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static void AddRxWebSanitizers(this MvcOptions options)
        {
            options.InputFormatters.Insert(0, new InputJsonFormatter());
        }

        public static void AddValidation(this MvcOptions options) {
            options.Conventions.Add(new ModelValidationFilterConvention());
        }

        public static void AddTracing(this MvcOptions options)
        {
            options.Filters.Add(new RequestTracing());
        }


    }
}
