#region Namespace
using Microsoft.Extensions.DependencyInjection;
using NewProjectSolution.Infrastructure.Security;
using RxWeb.Core.Data;
using RxWeb.Core.Security;
using RxWeb.Core.Annotations;
using RxWeb.Core;
using NewProjectSolution.UnitOfWork.DbEntityAudit;
using NewProjectSolution.BoundedContext.Main;
            using NewProjectSolution.UnitOfWork.Main;
            using NewProjectSolution.Domain.MasterModule;
#endregion Namespace




namespace NewProjectSolution.Api.Bootstrap
{
    public static class ScopedExtension
    {

        public static void AddScopedService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepositoryProvider, RepositoryProvider>();
            serviceCollection.AddScoped<ITokenAuthorizer, TokenAuthorizer>();
            serviceCollection.AddScoped<ILocalizationInfo, LocalizationInfo>();
            serviceCollection.AddScoped<IModelValidation, ModelValidation>();
			serviceCollection.AddScoped<IAuditLog, AuditLog>();
			serviceCollection.AddScoped<IApplicationTokenProvider, ApplicationTokenProvider>();

            #region ContextService

                        serviceCollection.AddScoped<ILoginContext, LoginContext>();
            serviceCollection.AddScoped<ILoginUow, LoginUow>();
                        serviceCollection.AddScoped<IMasterContext, MasterContext>();
            serviceCollection.AddScoped<IMasterUow, MasterUow>();
            #endregion ContextService




            #region DomainService
            
            
            serviceCollection.AddScoped<IUserRoleDomain, UserRoleDomain>();
            #endregion DomainService




        }
    }
}




