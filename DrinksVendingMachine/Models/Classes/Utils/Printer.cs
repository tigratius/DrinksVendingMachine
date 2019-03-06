using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.Classes.Utils
{
    public class Printer
    {
        private readonly IVendingMachineLogger _logger;

        public Printer(IVendingMachineLogger vendingMachineLogger)
        {
            _logger = vendingMachineLogger;
        }

        public void DrinkInfo(DrinkEntity drink)
        {
            _logger.Info(" ");
            _logger.Info("  DrinkInfo:");
            _logger.Info("    Id = " + drink.Id);
            _logger.Info("    Name = " + drink.Name);
            _logger.Info("    Count = " + drink.Count);
            _logger.Info("    CostPrice = " + drink.CostPrice);
            _logger.Info("    ImagePath = " + drink.ImagePath);
            _logger.Info(" ");
        }

        public void StateInfo(CurrentStateEntity currentState)
        {
            _logger.Info(" ");
            _logger.Info("  StateInfo:");
            _logger.Info("    Deposit = " + currentState.Deposit);
            _logger.Info("    Change = " + currentState.Change);
            _logger.Info(" ");
        }

        public void CoinInfo(CoinEntity coin)
        {
            _logger.Info(" ");
            _logger.Info("  CoinInfo:");
            _logger.Info("    Id = " + coin.Id);
            _logger.Info("    Count = " + coin.Count);
            _logger.Info("    Value = " + coin.Value);
            _logger.Info("    IsBlocking = " + coin.IsBlocking);
            _logger.Info(" ");
        }

        public void DrinkAndStateInfo(DrinkEntity drink, CurrentStateEntity state)
        {
            DrinkInfo(drink);
            StateInfo(state);
        }

        public void CoinAndStateInfo(CoinEntity coin, CurrentStateEntity state)
        {
            CoinInfo(coin);
            StateInfo(state);
        }
    }
}