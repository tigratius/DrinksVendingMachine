using System;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    /// <summary>
    /// Тестируем логику автомата
    /// </summary>
    [TestClass]
    public class TestVengineMachineLogic : FakeData
    {

        /*/// <summary>
        /// Добавляем новы напиток
        /// </summary>
        [TestMethod]
        public void DrinkAdd()
        {
            Init();

            var drink3 = new DrinkEntity()
            {
                CostPrice = 30,
                Count = 1,
                Id = Guid.NewGuid(),
                ImagePath = "image3",
                Name = "drink3"
            };

            VengineMachine.Add(drink3);
            RepositoryDrink.SaveChanges();

            Assert.AreEqual(3, DrinkEntitiesList.Count);
        }*/

        /*/// <summary>
        /// Удаляем напиток
        /// </summary>
        [TestMethod]
        public void DrinkRemove()
        {
            Init();

            DrinkEntity drink1 = DrinkEntitiesList[0];
            VengineMachine.Remove(drink1);

            RepositoryDrink.SaveChanges();
            Assert.AreEqual(1, DrinkEntitiesList.Count);
        }*/

        /// <summary>
        /// изменение кол-ва напитков
        /// </summary>
        [TestMethod]
        public void DrinkChangeCount()
        {
            Init();

            VengineMachine.ChangeCount(Drink1, 8);
            Assert.AreEqual(8, Drink1.Count);
        }

        /// <summary>
        /// изменение стоимости напитка
        /// </summary>
        [TestMethod]
        public void DrinkChangeCost()
        {
            Init();

            DrinkEntity drink1 = DrinkEntitiesList[0];
            VengineMachine.ChangeCost(drink1, 50);

            Assert.AreEqual(50, drink1.CostPrice);
        }


        /// <summary>
        /// смена картинки
        /// </summary>
        [TestMethod]
        public void DrinkChangeImage()
        {
            Init();

            VengineMachine.ChangeImage(Drink1, "filename1");
            Assert.AreEqual("filename1", Drink1.ImagePath);
        }


        /// <summary>
        /// Покупка напитка
        /// </summary>
        [TestMethod]
        public void BuyDrink()
        {
            Init();

            CurrentState.Deposit = 15;
            VengineMachine.BuyDrink(Drink1, CurrentState, CoinEntitiesList);
            Assert.AreEqual(1, Drink1.Count);
            Assert.AreEqual(5, CurrentState.Change);
        }

        /// <summary>
        /// нет сдачи
        /// </summary>
        [TestMethod]
        public void BuyDrinkNoChange()
        {
            Init();

            Coin1.Count = 0;
            CurrentState.Deposit = 15;

            VengineMachine.BuyDrink(Drink1, CurrentState, CoinEntitiesList);
            Assert.AreEqual(2, Drink1.Count);
            Assert.AreEqual(15, CurrentState.Change);
        }

        //Имитация покупки нескольких напитков перед получение сдачи
        [TestMethod]
        public void BuySomeDrinksBeforeGetChange()
        {
            Init();

            {
                Drink1.CostPrice = 2;
                Drink1.Count = 3;
            }
            {
                Drink2.CostPrice = 3;
                Drink2.Count = 5;
            }

            {
            }
            VengineMachine.AddCoin(Coin2, CurrentState);
            VengineMachine.AddCoin(Coin2, CurrentState);

            VengineMachine.BuyDrink(Drink1, CurrentState, CoinEntitiesList);

            Assert.AreEqual(2, CurrentState.Change);

            VengineMachine.AddCoin(Coin2, CurrentState);
            VengineMachine.AddCoin(Coin1, CurrentState);
            VengineMachine.AddCoin(Coin1, CurrentState);

            VengineMachine.BuyDrink(Drink2, CurrentState, CoinEntitiesList);

            Assert.AreEqual(3, CurrentState.Change);

            Assert.AreEqual(2, Drink1.Count);
            Assert.AreEqual(4, Drink2.Count);

        }
        
        /// <summary>
        /// Сдача
        /// </summary>
        [TestMethod]
        public void GetChange()
        {
            Init();
            CurrentState.Change = 5;
            VengineMachine.GetChange(CurrentState, CoinEntitiesList);
            Assert.AreEqual(9, Coin1.Count);
            Assert.AreEqual(8, Coin2.Count);
        }


        /// <summary>
        /// Изменение кол-ва монет
        /// </summary>
        [TestMethod]
        public void ChangeCoinCount()
        {
            Init();

            VengineMachine.ChangeCoinCount(Coin1, 20);

            Assert.AreEqual(20, Coin1.Count);
        }

        /// <summary>
        /// Добавление монет
        /// </summary>
        [TestMethod]
        public void AddThreeCoin()
        {
            Init();

            VengineMachine.AddCoin(Coin1, CurrentState);
            VengineMachine.AddCoin(Coin1, CurrentState);
            VengineMachine.AddCoin(Coin2, CurrentState);

            Assert.AreEqual(12, Coin1.Count);
            Assert.AreEqual(11, Coin2.Count);
            Assert.AreEqual(4, CurrentState.Deposit);
        }

        /// <summary>
        /// Блокировка монет
        /// </summary>
        [TestMethod]
        public void BlockUnblock()
        {
            Init();

            VengineMachine.Block(Coin1);
            VengineMachine.UnBlock(Coin2);

            Assert.IsTrue(Coin1.IsBlocking);
            Assert.IsFalse(Coin2.IsBlocking);
        }

        /// <summary>
        /// Не можем купить если внесенные средства меньше стоимости напитка
        /// </summary>
        [TestMethod]
        public void CantBuyIfDepositLowerThanCost()
        {
            Init();

            Drink1.CostPrice = 10;
            Drink1.Count = 1;

            Coin2.Count = 10;
            Coin2.Value = ValueCoins.Two;

            VengineMachine.AddCoin(Coin2, CurrentState);
            Assert.IsFalse(VengineMachine.IsCanBuy(Drink1, CurrentState));

            Coin1.Count = 10;
            Coin1.Value = ValueCoins.Ten; 

            VengineMachine.AddCoin(Coin1, CurrentState);
            Assert.IsTrue(VengineMachine.IsCanBuy(Drink1, CurrentState));

         
        }
        /// <summary>
        /// Не можем купить когда кол-во напитков равно 0
        /// </summary>
        [TestMethod]
        public void CantBuyIfCountEqualsZero()
        {
            Init();

            Drink1.CostPrice = 10;
            Drink1.Count = 0;

            Coin1.Count = 10;
            Coin1.Value = ValueCoins.Ten;

            VengineMachine.AddCoin(Coin1, CurrentState);
            Assert.IsFalse(VengineMachine.IsCanBuy(Drink1, CurrentState));
        }
    }
}
