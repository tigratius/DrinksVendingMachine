using System;
using System.Linq;

namespace DrinksVendingMachine.Models.Interfaces
{
    public interface IRepository<T> : IRepoSave where T : class
    {
        void Add(T add);

        void Remove(T remove);

        T Get(Guid id);

        IQueryable<T> Queryable();
    }
}
