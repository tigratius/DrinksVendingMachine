using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinksVendingMachine.Classes.Entities;
using DrinksVendingMachine.Models;

namespace DrinksVendingMachine.Classes
{
    public class CoinManager
    {
        private Repository<CoinEntity> repository;

        public CoinManager(Repository<CoinEntity> repository)
        {
            this.repository = repository;
        }

        public void ChangeCoinCount(CoinEntity coinEntity, int count)
        {
            coinEntity.Count = count;
        }

        public void Block(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = true;
        }

        public void UnBlock(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = false;
        }
    }

    public static class VendingMachine
    {
        public static IList<Coin> Calculate(List<CoinEntity> coins, int change)
        {
            List<Coin> matches = new List<Coin>();
            int changeLeft = change;

            foreach (var coin in coins)
            {
                var denomination = (int)coin.Value;

                if (coin.Count > 0 && denomination <= changeLeft)
                {
                    int remainder = changeLeft % denomination;
                    if (remainder < changeLeft)
                    {
                        int howMany = Math.Min(coin.Count,
                            (changeLeft - remainder) / denomination);

                        matches.Add(new Coin()
                        {
                            Value = denomination,
                            Count = howMany
                        });

                        int amount = howMany * denomination;
                        changeLeft -= amount;
                        if (changeLeft == 0)
                        {
                            return matches;
                        }
                    }
                }
            }

            return null;
        }
    }
}
