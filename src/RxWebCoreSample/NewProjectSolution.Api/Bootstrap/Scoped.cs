#region Namespace
using Microsoft.Extensions.DependencyInjection;
using NewProjectSolution.BoundedContext.Main;
using NewProjectSolution.Infrastructure.Security;
using NewProjectSolution.UnitOfWork.DbEntityAudit;
using NewProjectSolution.UnitOfWork.Main;
using RxWeb.Core.Annotations;
using RxWeb.Core.Data;
using RxWeb.Core.Security;
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
            serviceCollection.AddScoped<IModelValidation, ModelValidation>();
            serviceCollection.AddScoped<IAuditLog, AuditLog>();
            serviceCollection.AddScoped<IApplicationTokenProvider, ApplicationTokenProvider>();

            #region ContextService

            serviceCollection.AddScoped<ILoginContext, LoginContext>();
            serviceCollection.AddScoped<ILoginUow, LoginUow>();
                        serviceCollection.AddScoped<IMasterContext, MasterContext>();
            serviceCollection.AddScoped<IMasterUow, MasterUow>();
                        serviceCollection.AddScoped<IMasterContext, MasterContext>();
            serviceCollection.AddScoped<IMasterUow, MasterUow>();
                        serviceCollection.AddScoped<IMasterContext, MasterContext>();
            serviceCollection.AddScoped<IMasterUow, MasterUow>();
            #endregion ContextService








            #region DomainService
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonCollectionDomain, PersonCollectionDomain>();
            
            serviceCollection.AddScoped<IPersonCollectionDomain, PersonCollectionDomain>();
            
            serviceCollection.AddScoped<IPersonCollectionDomain, PersonCollectionDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            
            serviceCollection.AddScoped<IPersonCollectionDomain, PersonCollectionDomain>();
            
            serviceCollection.AddScoped<IPersonCollectionDomain, PersonCollectionDomain>();
            
            serviceCollection.AddScoped<IPersonCollectionDomain, PersonCollectionDomain>();
            
            serviceCollection.AddScoped<IPersonDomain, PersonDomain>();
            #endregion DomainService



















        }
    }
}




