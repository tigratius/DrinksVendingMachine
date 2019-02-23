using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinksVendingMachine.Classes.Entities;

namespace DrinksVendingMachine.Models
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