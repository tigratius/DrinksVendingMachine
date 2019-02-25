using System.Collections.Generic;
using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Models.Classes
{
    public class ModelView
    {
        public List<DrinkEntity> Drinks { get; set; }
        public List<CoinEntity> Coins { get; set; }

        public int Deposit { get; set; }
        public int Change { get; set; }

        public string Token { get; set; }
    }
}