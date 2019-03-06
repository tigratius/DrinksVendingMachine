using System;

namespace DrinksVendingMachine.Models.Interfaces
{
    public interface IRepoSave : IDisposable
    {
        void SaveChanges();
    }
}
