using Microsoft.Extensions.DependencyInjection;
using NewProjectSolution.Infrastructure.Singleton;
using NewProjectSolution.BoundedContext.Singleton;

namespace NewProjectSolution.Api.Bootstrap
{
    public static class Singleton
    {
        public static void AddSingletonService(this IServiceCollection serviceCollection) {
            serviceCollection.AddSingleton(typeof(TenantDbConnectionInfo));
            serviceCollection.AddSingleton(typeof(UserAccessConfigInfo));
        }

    }
}




