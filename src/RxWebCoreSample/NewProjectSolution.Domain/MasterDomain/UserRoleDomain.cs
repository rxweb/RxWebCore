using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RxWeb.Core;
using NewProjectSolution.UnitOfWork.Main;
using NewProjectSolution.Models.Main;
using NewProjectSolution.Models.ViewModels;

namespace NewProjectSolution.Domain.MasterModule
{
    public class UserRoleDomain : IUserRoleDomain
    {
        public UserRoleDomain(IMasterUow uow) {
            this.Uow = uow;
        }

        public Task<object> GetAsync(QueryModel parameters)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetBy(QueryModel parameters)
        {
            return Task.FromResult(parameters as object);
        }
        

        public HashSet<string> AddValidation(UserRole entity)
        {
            return ValidationMessages;
        }

        public async Task AddAsync(UserRole entity)
        {
            await Uow.RegisterNewAsync(entity);
            await Uow.CommitAsync();
        }

        public HashSet<string> UpdateValidation(UserRole entity)
        {
            return ValidationMessages;
        }

        public async Task UpdateAsync(UserRole entity)
        {
            await Uow.RegisterDirtyAsync(entity);
            await Uow.CommitAsync();
        }

        public HashSet<string> DeleteValidation(QueryModel parameters)
        {
            return ValidationMessages;
        }

        public Task DeleteAsync(QueryModel parameters)
        {
            throw new NotImplementedException();
        }

        public IMasterUow Uow { get; set; }

        private HashSet<string> ValidationMessages { get; set; } = new HashSet<string>();
    }

    public interface IUserRoleDomain : ICoreDomain<UserRole, QueryModel> { }
}
