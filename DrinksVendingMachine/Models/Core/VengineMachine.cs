using System;
using System.Collections.Generic;
using System.Linq;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Models.Core
{
    public class VengineMachine
    {
        private readonly Repository<DrinkEntity> _drinksRepo;
        private readonly Repository<CoinEntity> _coinsRepo;

        public VengineMachine()
        {}

        public VengineMachine(Repository<DrinkEntity> drinksRepo, Repository<CoinEntity> coinsRepo)
        {
            _drinksRepo = drinksRepo;
            _coinsRepo = coinsRepo;
        }

        public void Add(DrinkEntity drinkEntity)
        {
            _drinksRepo.Add(drinkEntity);
        }

        public void Remove(DrinkEntity drinkEntity)
        {
            _drinksRepo.Remove(drinkEntity);
        }

        public void ChangeCount(DrinkEntity drinkEntity, int count)
        {
            drinkEntity.Count = count;
        }

        public void ChangeCost(DrinkEntity drinkEntity, int cost)
        {
            drinkEntity.CostPrice = cost;
        }

        public void ChangeImage(DrinkEntity drinkEntity, string path)
        {
            drinkEntity.ImagePath = path;
        }

        public void ChangeName(DrinkEntity drinkEntity, string name)
        {
            drinkEntity.Name = name;
        }

        public void ChangeCoinCount(CoinEntity coinEntity, int count)
        {
            coinEntity.Count = count;
        }

        public void AddCoin(CoinEntity coinEntity, CurrentStateEntity currentState)
        {
            coinEntity.Count++;
            currentState.Deposit += (int)coinEntity.Value;
        }

        public void Block(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = true;
        }

        public void UnBlock(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = false;
        }

        public bool BuyDrink(DrinkEntity drink, CurrentStateEntity currentState)
        {
            List<CoinEntity> coins = _coinsRepo.Queryable().ToList();
            var deposit = currentState.Deposit;
            var change = deposit - drink.CostPrice;
            currentState.Deposit = 0;

            if (IsHaveCoinsForChange(coins, change))
            {
                drink.Count--;
                currentState.Change += change;
                return true;
            }
            
            /*Ситуация, когда в автомате нет сдачи 
            Нужно предупердить что пр покупке нету сдачи и отдать внесенные средства в качестве сдачи
            */
            currentState.Change += deposit;

            return false;
        }

        public void GetChange(CurrentStateEntity currentState)
        {
            List<CoinEntity> coins = _coinsRepo.Queryable().ToList();
            var change = currentState.Change;
            IList<Coin> coinsForChange = GetCoinsForChange(coins, change);

            if (coinsForChange != null)
            {
                foreach (var coin in coinsForChange)
                {
                    CoinEntity coinEntity = coins.FirstOrDefault(c => (int)c.Value == coin.Value);
                    if (coinEntity != null) coinEntity.Count -= coin.Count;
                }

                currentState.Change = 0;
            }
        }
        /// <summary>
        /// Сдачу считаем следующим образом:
        /// Сначала отдаем 10  - потом 5 - потом 2 и в последнюю очередь 1
        /// </summary>
        /// <param name="coins"></param>
        /// <param name="change"></param>
        /// <returns></returns>
        protected IList<Coin> GetCoinsForChange(List<CoinEntity> coins, int change)
        {
            List<Coin> matches = new List<Coin>();
            int changeLeft = change;

            var coinsSorted = coins.OrderByDescending(c =>(int) c.Value);
            foreach (var coin in coinsSorted)
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

        protected bool IsHaveCoinsForChange(List<CoinEntity> coins, int change)
        {
            return GetCoinsForChange(coins, change) != null;
        }
    }
}