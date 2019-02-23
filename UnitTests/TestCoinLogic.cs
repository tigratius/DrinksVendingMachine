
using System;
using System.Collections.Generic;
using System.Linq;
using DrinksVendingMachine.Classes;
using DrinksVendingMachine.Classes.Entities;
using DrinksVendingMachine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestCoinLogic
    {
        private FakeRepository<CoinEntity> repositoryCoins;
        private List<CoinEntity> coinEntitiesList;

        [TestInitialize]
        public void Init()
        {
            var coin1 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 100,
                IsBlocking = false,
                Value = ValueCoins.One
            };

            var coin2 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 50,
                IsBlocking = true,
                Value = ValueCoins.Two
            };

            var coin3 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 20,
                IsBlocking = false,
                Value = ValueCoins.Five
            };

            var coin4 = new CoinEntity()
            {
                Id = Guid.NewGuid(),
                Count = 10,
                IsBlocking = false,
                Value = ValueCoins.Ten
            };

            coinEntitiesList = new List<CoinEntity> { coin1, coin2, coin3, coin4 };

            repositoryCoins = new FakeRepository<CoinEntity>(coinEntitiesList);
        }

        private CoinEntity getFirstCoin()
        {
            return coinEntitiesList[0];
        }

        private CoinEntity getSecondCoin()
        {
            return coinEntitiesList[1];
        }


        [TestMethod]
        public void TestChangeCoinCount()
        {
            Init();

            CoinManager coinManager = new CoinManager(repositoryCoins);

            var coin1 = getFirstCoin();
            coinManager.ChangeCoinCount(coin1, 100);

            Assert.AreEqual(100, coin1.Count);
        }

        [TestMethod]
        public void TestBlockCoin()
        {
            Init();

            CoinManager coinManager = new CoinManager(repositoryCoins);

            var coin1 = getFirstCoin();
            coinManager.Block(coin1);

            Assert.IsTrue(coin1.IsBlocking);
        }

        [TestMethod]
        public void TestUnlockCoin()
        {
            Init();

            CoinManager coinManager = new CoinManager(repositoryCoins);

            var coin2 = getSecondCoin();
            coinManager.UnBlock(coin2);

            Assert.IsFalse(coin2.IsBlocking);
        }

        [TestMethod]
        public void TestCalculateChange()
        {
            List<CoinEntity> coins = new List<CoinEntity>
            {
                new CoinEntity()
                {
                    Value = ValueCoins.One,
                    Count = 10
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Two,
                    Count = 5
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Five,
                    Count = 2
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Ten,
                    Count = 1
                },
            };

            IOrderedEnumerable<CoinEntity> orderByDescending = coins.OrderByDescending(d => (int) d.Value);
            IList<Coin> results = VendingMachine.Calculate(orderByDescending.ToList(), 33);
            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(Expects(results, 10, 1));
            Assert.IsTrue(Expects(results, 5, 2));
            Assert.IsTrue(Expects(results, 2, 5));
            Assert.IsTrue(Expects(results, 1, 3));
        }

        [TestMethod]
        public void TestCalculateChange1()
        {
            List<CoinEntity> coins = new List<CoinEntity>
            {
                new CoinEntity()
                {
                    Value = ValueCoins.One,
                    Count = 10
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Two,
                    Count = 5
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Five,
                    Count = 2
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Ten,
                    Count = 1
                },
            };

            IOrderedEnumerable<CoinEntity> orderByDescending = coins.OrderByDescending(d => (int)d.Value);
            IList<Coin> results = VendingMachine.Calculate(orderByDescending.ToList(), 9);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(Expects(results, 5, 1));
            Assert.IsTrue(Expects(results, 2, 2));
        }

        private bool Expects(IList<Coin> coins, int denomination, int count)
        {
            Coin c = coins.FirstOrDefault(x => x.Value == denomination);
            return c != null && c.Count == count;
        }
    }
}

