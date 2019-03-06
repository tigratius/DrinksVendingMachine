namespace DrinksVendingMachine.Models.Interfaces
{
    public interface IVendingMachineLogger
    {
        void Info(string mes);

        void Error(string mes);

        void Warning(string mes);
    }
}
