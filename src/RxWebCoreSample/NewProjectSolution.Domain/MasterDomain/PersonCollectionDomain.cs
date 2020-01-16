using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RxWeb.Core;
using NewProjectSolution.UnitOfWork.Main;
using NewProjectSolution.Models.Main;

namespace NewProjectSolution.Domain.MasterModule
{
    public class PersonCollectionDomain : IPersonCollectionDomain
    {
        public PersonCollectionDomain(IMasterUow uow) {
            this.Uow = uow;
        }

        public HashSet<string> AddValidation(List<Person> entities)
        {
            return ValidationMessages;
        }

        public async Task AddAsync(List<Person> entities)
        {
            throw new NotImplementedException();
        }

        public HashSet<string> UpdateValidation(List<Person> entities)
        {
            return ValidationMessages;
        }

        public async Task UpdateAsync(List<Person> entities)
        {
            throw new NotImplementedException();
        }

        public IMasterUow Uow { get; set; }

        private HashSet<string> ValidationMessages { get; set; } = new HashSet<string>();
    }

    public interface IPersonCollectionDomain : ICoreCollectionDomain<Person> { }
}
