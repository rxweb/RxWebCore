using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RxWeb.Core;
using NewProjectSolution.UnitOfWork.Main;
using NewProjectSolution.Models.Main;

namespace NewProjectSolution.Domain.MasterModule
{
    public class PersonDomain : IPersonDomain
    {
        public PersonDomain(IMasterUow uow) {
            this.Uow = uow;
        }

        public Task<object> GetAsync(Person parameters)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetBy(Person parameters)
        {
            throw new NotImplementedException();
        }
        

        public HashSet<string> AddValidation(Person entity)
        {
            return ValidationMessages;
        }

        public async Task AddAsync(Person entity)
        {
            await Uow.RegisterNewAsync(entity);
            await Uow.CommitAsync();
        }

        public HashSet<string> UpdateValidation(Person entity)
        {
            return ValidationMessages;
        }

        public async Task UpdateAsync(Person entity)
        {
            await Uow.RegisterDirtyAsync(entity);
            await Uow.CommitAsync();
        }

        public HashSet<string> DeleteValidation(Person parameters)
        {
            return ValidationMessages;
        }

        public Task DeleteAsync(Person parameters)
        {
            throw new NotImplementedException();
        }

        public IMasterUow Uow { get; set; }

        private HashSet<string> ValidationMessages { get; set; } = new HashSet<string>();
    }

    public interface IPersonDomain : ICoreDomain<Person,Person> { }
}
