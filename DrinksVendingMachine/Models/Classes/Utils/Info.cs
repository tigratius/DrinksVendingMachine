using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinksVendingMachine.Models.Entities;
using DrinksVendingMachine.Models.Interfaces;
using NLog;

namespace DrinksVendingMachine.Models.Classes.Utils
{
    public static class Info
    {
        public static void DrinkInfo(DrinkEntity drink, ISimpleLogger logger)
        {
            logger.WriteLine(" ");
            logger.WriteLine("  DrinkInfo:");
            logger.WriteLine("    Id = " + drink.Id);
            logger.WriteLine("    Name = " + drink.Name);
            logger.WriteLine("    Count = " + drink.Count);
            logger.WriteLine("    CostPrice = " + drink.CostPrice);
            logger.WriteLine("    ImagePath = " + drink.ImagePath);
            logger.WriteLine(" ");
        }

        public static void StateInfo(CurrentStateEntity currentState, ISimpleLogger logger)
        {
            logger.WriteLine(" ");
            logger.WriteLine("  StateInfo:");
            logger.WriteLine("    Deposit = " + currentState.Deposit);
            logger.WriteLine("    Change = " + currentState.Change);
            logger.WriteLine(" ");
        }

        public static void CoinInfo(CoinEntity coin, ISimpleLogger logger)
        {
            logger.WriteLine(" ");
            logger.WriteLine("  CoinInfo:");
            logger.WriteLine("    Id = " + coin.Id);
            logger.WriteLine("    Count = " + coin.Count);
            logger.WriteLine("    Value = " + coin.Value);
            logger.WriteLine("    IsBlocking = " + coin.IsBlocking);
            logger.WriteLine(" ");
        }

        public static void DrinkAndState(DrinkEntity drink, CurrentStateEntity state, ISimpleLogger logger)
        {
            DrinkInfo(drink, logger);
            StateInfo(state, logger);
        }

        public static void CoinAndState(CoinEntity coin, CurrentStateEntity state, ISimpleLogger logger)
        {
            CoinInfo(coin, logger);
            StateInfo(state, logger);
        }

    }
}