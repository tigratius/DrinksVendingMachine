using System;
using System.Collections.Generic;
using System.Linq;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Models.Core
{

    /// <summary>
    /// Логика автомата по продаже напитков
    /// </summary>
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

        /// <summary>
        /// Добавление нового напитка 
        /// </summary>
        /// <param name="drinkEntity"></param>
        public void Add(DrinkEntity drinkEntity)
        {
            _drinksRepo.Add(drinkEntity);
        }

        /// <summary>
        /// Удаление напитка
        /// </summary>
        /// <param name="drinkEntity"></param>
        public void Remove(DrinkEntity drinkEntity)
        {
            _drinksRepo.Remove(drinkEntity);
        }

        /// <summary>
        /// Изменение кол-ва
        /// </summary>
        /// <param name="drinkEntity"></param>
        /// <param name="count"></param>
        public void ChangeCount(DrinkEntity drinkEntity, int count)
        {
            drinkEntity.Count = count;
        }

        /// <summary>
        /// Изменение стоимости
        /// </summary>
        /// <param name="drinkEntity"></param>
        /// <param name="cost"></param>
        public void ChangeCost(DrinkEntity drinkEntity, int cost)
        {
            drinkEntity.CostPrice = cost;
        }

        /// <summary>
        /// Изменение картинки
        /// </summary>
        /// <param name="drinkEntity"></param>
        /// <param name="path"></param>
        public void ChangeImage(DrinkEntity drinkEntity, string path)
        {
            drinkEntity.ImagePath = path;
        }

        /// <summary>
        /// Изменение имени
        /// </summary>
        /// <param name="drinkEntity"></param>
        /// <param name="name"></param>
        public void ChangeName(DrinkEntity drinkEntity, string name)
        {
            drinkEntity.Name = name;
        }

        /// <summary>
        /// Изменение кол-ва монет
        /// </summary>
        /// <param name="coinEntity"></param>
        /// <param name="count"></param>
        public void ChangeCoinCount(CoinEntity coinEntity, int count)
        {
            coinEntity.Count = count;
        }

        /// <summary>
        /// Добавление монеты в автомат, возможно не самое лучшее название метода
        /// </summary>
        /// <param name="coinEntity"></param>
        /// <param name="currentState"></param>
        public void AddCoin(CoinEntity coinEntity, CurrentStateEntity currentState)
        {
            coinEntity.Count++;
            currentState.Deposit += (int)coinEntity.Value;
        }

        /// <summary>
        /// Блокировка монеты
        /// </summary>
        /// <param name="coinEntity"></param>
        public void Block(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = true;
        }

        /// <summary>
        /// Разблокировка монеты
        /// </summary>
        /// <param name="coinEntity"></param>
        public void UnBlock(CoinEntity coinEntity)
        {
            coinEntity.IsBlocking = false;
        }

        /// <summary>
        /// Покупка напитка
        /// </summary>
        /// <param name="drink"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public bool BuyDrink(DrinkEntity drink, CurrentStateEntity currentState)
        {
            //Если не можем купить выходим
            if (!IsCanBuy(drink, currentState))
                return false;

            List<CoinEntity> coins = _coinsRepo.Queryable().ToList();
            var deposit = currentState.Deposit;
            var change = deposit - drink.CostPrice;
            currentState.Deposit = 0;

            //Если сдача равна 0 или в атомате достаточное кол-во монет для выдачи, то позволяем купить, иначе просто отдаем внесенные деньги 
            if (change == 0 || IsHaveCoinsForChange(coins, change))
            {
                drink.Count--;
                currentState.Change += change;
                return true;
            }
            
            currentState.Change += deposit;

            return false;
        }
        /// <summary>
        /// Проверка на возможность покупки
        /// Нельзя купит если напитка нет или кол-во внесенных средст меньше стоимости напитка
        /// </summary>
        /// <param name="drink"></param>
        /// <param name="currentState"></param>
        /// <returns></returns>
        public bool IsCanBuy(DrinkEntity drink, CurrentStateEntity currentState)
        {
            return drink.Count > 0 && currentState.Deposit >= drink.CostPrice;
        }


        /// <summary>
        /// Выдача сдачи
        /// </summary>
        /// <param name="currentState"></param>
        public void GetChange(CurrentStateEntity currentState)
        {
            List<CoinEntity> coins = _coinsRepo.Queryable().ToList();
            var change = currentState.Change;

            //Подсчет монет  для сдачи
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
        /// Метод считает монеты для сдачи 
        /// Сдачу считаем следующим образом:
        /// Сначала отдаем 10  - потом 5 - потом 2 и в последнюю очередь 1, так называемый жадный алгоритм, при желании можно оптимизировать
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

        /// <summary>
        /// Проверка хватит ли монет для сдачи
        /// </summary>
        /// <param name="coins"></param>
        /// <param name="change"></param>
        /// <returns></returns>
        protected bool IsHaveCoinsForChange(List<CoinEntity> coins, int change)
        {
            return GetCoinsForChange(coins, change) != null;
        }
    }
}