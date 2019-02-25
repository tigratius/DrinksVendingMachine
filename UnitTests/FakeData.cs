using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinksVendingMachine.Models.BL;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;

namespace UnitTests
{
    public class FakeData
    {
        protected List<DrinkEntity> _drinkEntitiesList;

        protected DrinkEntity _drink1;
        protected DrinkEntity _drink2;

        protected CoinEntity _coin1;
        protected CoinEntity _coin2;
        protected CoinEntity _coin3;
        protected CoinEntity _coin4;

        protected CurrentStateEntity _currentState;
        protected VengineMachine _vengineMachine;

        protected FakeRepository<DrinkEntity> _repositoryDrink;

        public virtual void Init()
        {
            _drink1 = new DrinkEntity()
            {
                CostPrice = 10,
                Count = 2,
                Id = Guid.Parse("FFDF9A9C-1FC1-4DEB-B552-45EB5E0EC48C"),
                Image = "image1",
                Name = "drink1"
            };

            _drink2 = new DrinkEntity()
            {
                CostPrice = 20,
                Count = 4,
                Id = Guid.NewGuid(),
                Image = "image2",
                Name = "drink2"
            };

            _coin1 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 10,
                IsBlocking = false,
                Value = ValueCoins.One
            };

            _coin2 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 10,
                IsBlocking = true,
                Value = ValueCoins.Two
            };

            _currentState = new CurrentStateEntity()
            {
                Change = 0,
                Deposit = 0
            };

            _drinkEntitiesList = new List<DrinkEntity> { _drink1, _drink2 };
            var _coinEntitiesList = new List<CoinEntity> { _coin1, _coin2 };

            _repositoryDrink = new FakeRepository<DrinkEntity>(_drinkEntitiesList);
            var _repositoryCoin = new FakeRepository<CoinEntity>(_coinEntitiesList);

            _vengineMachine = new VengineMachine(_repositoryDrink, _repositoryCoin);

        }
    }
}
