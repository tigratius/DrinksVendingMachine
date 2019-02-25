/*

using System;
using System.Collections.Generic;
using DrinksVendingMachine.Models.BL;
using DrinksVendingMachine.Models.BL.Managers;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestCoinLogic 
    {
        private CoinEntity _coin1;
        private CoinEntity _coin2;
        private CoinEntity _coin3;
        private CoinEntity _coin4;
        private CurrentStateEntity _currentState;
        private VengineMachine _vengineMachine;

        public void Init()
        {
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

            _coin3 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 10,
                IsBlocking = false,
                Value = ValueCoins.Five
            };

            _coin4 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 10,
                IsBlocking = false,
                Value = ValueCoins.Ten
            };

            _currentState = new CurrentStateEntity()
            {
                Change = 0,
                Deposit = 0
            };

            var _drinkEntitiesList = new List<DrinkEntity>();
            var _coinEntitiesList = new List<CoinEntity>();

            var _repositoryDrink = new FakeRepository<DrinkEntity>(_drinkEntitiesList);
            var _repositoryCoin = new FakeRepository<CoinEntity>(_coinEntitiesList);

            _vengineMachine = new VengineMachine(_repositoryDrink, _repositoryCoin);
        }

        [TestMethod]
        public void ChangeCoinCount()
        {
            Init();

            _vengineMachine.ChangeCoinCount(_coin1, 20);

            Assert.AreEqual(20, _coin1.Count);
        }

        [TestMethod]
        public void AddThreeCoin()
        {
            Init();

            _vengineMachine.AddCoin(_coin1, _currentState);
            _vengineMachine.AddCoin(_coin1, _currentState);
            _vengineMachine.AddCoin(_coin2, _currentState);

            Assert.AreEqual(12, _coin1.Count);
            Assert.AreEqual(11, _coin2.Count);
            Assert.AreEqual(4, _currentState.Deposit);
        }

        [TestMethod]
        public void BlockUnblock()
        {
            Init();

            _vengineMachine.Block(_coin1);
            _vengineMachine.UnBlock(_coin2);

            Assert.IsTrue(_coin1.IsBlocking);
            Assert.IsFalse(_coin2.IsBlocking);
        }
    }
}

*/
