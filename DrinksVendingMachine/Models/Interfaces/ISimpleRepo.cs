namespace DrinksVendingMachine.Models.Interfaces
{
    public interface ISimpleRepo<T> : IRepoSave where T : class
    {
        T GetFirst();
    }
}
