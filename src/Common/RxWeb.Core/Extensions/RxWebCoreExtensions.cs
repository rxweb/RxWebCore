using Microsoft.Extensions.DependencyInjection;
using RxWeb.Core.Singleton;
using System.Collections.Generic;

namespace RxWeb.Core.Extensions
{
    public static class RxWebLocalizationExtensions
    {
        public static void AddRxWebLocalization(this IServiceCollection serviceCollection, Dictionary<string, Dictionary<string, string>>  localizeKeys = null)
        {
            if (localizeKeys != null) {
                ApplicationLocalizationInfo.IsStaticMessages = true;
                ApplicationLocalizationInfo.LocalizeKeys = localizeKeys;
            }
            serviceCollection.AddScoped<ILocalizationInfo, LocalizationInfo>();
        }
    }
}
