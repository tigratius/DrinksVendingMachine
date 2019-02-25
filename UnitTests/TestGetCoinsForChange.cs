using System.Collections.Generic;
using System.Linq;
using DrinksVendingMachine.Models.BL;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestGetCoinsForChange
    {

        private readonly VengineMachineTest _vmt = new VengineMachineTest();

        /// <summary>
        /// Обычный тест для проверки сдачи
        /// </summary>
        [TestMethod]
        public void CalculateChangeCommon()
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
            IList<Coin> results = _vmt.GetCoins(orderByDescending.ToList(), 33);
            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(Expects(results, 10, 1));
            Assert.IsTrue(Expects(results, 5, 2));
            Assert.IsTrue(Expects(results, 2, 5));
            Assert.IsTrue(Expects(results, 1, 3));
        }


        /// <summary>
        /// Проверка, если сдача меньше монеты достоинстов 10, то ее нет в сдаче
        /// </summary>
        [TestMethod]
        public void ChangeLowerThanBigCoin()
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
            IList<Coin> results = _vmt.GetCoins(orderByDescending.ToList(), 9);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(Expects(results, 5, 1));
            Assert.IsTrue(Expects(results, 2, 2));
        }

        /// <summary>
        /// Проверка, если сдача меньше монеты достоинстов 10, то ее нет в сдаче
        /// </summary>
        [TestMethod]
        public void NoCoinsFiveAndTen()
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
                    Count = 3
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Five,
                    Count = 0
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Ten,
                    Count = 0
                },
            };

            IOrderedEnumerable<CoinEntity> orderByDescending = coins.OrderByDescending(d => (int)d.Value);
            IList<Coin> results = _vmt.GetCoins(orderByDescending.ToList(), 9);
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(Expects(results, 2, 3));
            Assert.IsTrue(Expects(results, 1, 3));
        }

        /// <summary>
        /// Проверка, если в автомате нет вообще монет
        /// </summary>
        [TestMethod]
        public void NoMatchDueToNoCoins()
        {
            List<CoinEntity> coins = new List<CoinEntity>
            {
                new CoinEntity()
                {
                    Value = ValueCoins.One,
                    Count = 0
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Two,
                    Count = 0
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Five,
                    Count = 0
                },
                new CoinEntity()
                {
                    Value = ValueCoins.Ten,
                    Count = 0
                },
            };

            Assert.IsNull(_vmt.GetCoins(coins, 10));
        }

        /// <summary>
        /// Проверка ситуации, если в автомате нет достаточного количества монет
        /// </summary>
        [TestMethod]
        public void NoMatchDueToNotEnoughCoins()
        {
            List<CoinEntity> coins = new List<CoinEntity>
            {
                new CoinEntity()
                {
                    Value = ValueCoins.One,
                    Count = 0
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
            IList<Coin> results = _vmt.GetCoins(orderByDescending.ToList(), 8);
            Assert.IsNull(results);
        }

        private bool Expects(IList<Coin> coins, int denomination, int count)
        {
            Coin c = coins.FirstOrDefault(x => x.Value == denomination);
            return c != null && c.Count == count;
        }
    }

    public class VengineMachineTest : VengineMachine
    {
        public IList<Coin> GetCoins(List<CoinEntity> coins, int change)
        {
            return GetCoinsForChange(coins, change);
        }
    }
}
