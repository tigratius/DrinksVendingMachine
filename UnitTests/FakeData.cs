using System;
using System.Collections.Generic;
using DrinksVendingMachine.Models.Core;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;

namespace UnitTests
{
    public class FakeData
    {
        protected List<DrinkEntity> DrinkEntitiesList;

        protected DrinkEntity Drink1;
        protected DrinkEntity Drink2;

        protected CoinEntity Coin1;
        protected CoinEntity Coin2;
        protected CoinEntity Coin3;
        protected CoinEntity Coin4;

        protected CurrentStateEntity CurrentState;
        protected VengineMachine VengineMachine;

        protected FakeRepository<DrinkEntity> RepositoryDrink;

        public virtual void Init()
        {
            Drink1 = new DrinkEntity()
            {
                CostPrice = 10,
                Count = 2,
                Id = Guid.Parse("FFDF9A9C-1FC1-4DEB-B552-45EB5E0EC48C"),
                ImagePath = "image1",
                Name = "drink1"
            };

            Drink2 = new DrinkEntity()
            {
                CostPrice = 20,
                Count = 4,
                Id = Guid.NewGuid(),
                ImagePath = "image2",
                Name = "drink2"
            };

            Coin1 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 10,
                IsBlocking = false,
                Value = ValueCoins.One
            };

            Coin2 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 10,
                IsBlocking = true,
                Value = ValueCoins.Two
            };

            CurrentState = new CurrentStateEntity()
            {
                Change = 0,
                Deposit = 0
            };

            DrinkEntitiesList = new List<DrinkEntity> { Drink1, Drink2 };
            var coinEntitiesList = new List<CoinEntity> { Coin1, Coin2 };

            RepositoryDrink = new FakeRepository<DrinkEntity>(DrinkEntitiesList);
            var repositoryCoin = new FakeRepository<CoinEntity>(coinEntitiesList);

            VengineMachine = new VengineMachine(RepositoryDrink, repositoryCoin);

        }
    }
}
