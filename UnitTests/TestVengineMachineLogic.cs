using System;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestVengineMachineLogic : FakeData
    {
        [TestMethod]
        public void DrinkAdd()
        {
            Init();

            var drink3 = new DrinkEntity()
            {
                CostPrice = 30,
                Count = 1,
                Id = Guid.NewGuid(),
                Image = "image3",
                Name = "drink3"
            };

            VengineMachine.Add(drink3);
            RepositoryDrink.SaveChanges();

            Assert.AreEqual(3, DrinkEntitiesList.Count);
        }

        [TestMethod]
        public void DrinkRemove()
        {
            Init();

            DrinkEntity drink1 = DrinkEntitiesList[0];
            VengineMachine.Remove(drink1);

            RepositoryDrink.SaveChanges();
            Assert.AreEqual(1, DrinkEntitiesList.Count);
        }

        [TestMethod]
        public void DrinkChangeCount()
        {
            Init();

            VengineMachine.ChangeCount(Drink1, 8);
            Assert.AreEqual(8, Drink1.Count);
        }

        [TestMethod]
        public void DrinkChangeCost()
        {
            Init();

            DrinkEntity drink1 = DrinkEntitiesList[0];
            VengineMachine.ChangeCost(drink1, 50);

            Assert.AreEqual(50, drink1.CostPrice);
        }

        [TestMethod]
        public void DrinkChangeImage()
        {
            Init();

            VengineMachine.ChangeImage(Drink1, "filename1");
            Assert.AreEqual("filename1", Drink1.Image);
        }

        [TestMethod]
        public void BuyDrink()
        {
            Init();

            CurrentState.Deposit = 15;
            VengineMachine.BuyDrink(Drink1, CurrentState);
            Assert.AreEqual(1, Drink1.Count);
            Assert.AreEqual(5, CurrentState.Change);
        }

        [TestMethod]
        public void BuyDrinkNoChange()
        {
            Init();

            Coin1.Count = 0;
            CurrentState.Deposit = 15;

            VengineMachine.BuyDrink(Drink1, CurrentState);
            Assert.AreEqual(2, Drink1.Count);
            Assert.AreEqual(15, CurrentState.Change);
        }

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

            VengineMachine.BuyDrink(Drink1, CurrentState);

            Assert.AreEqual(2, CurrentState.Change);

            VengineMachine.AddCoin(Coin2, CurrentState);
            VengineMachine.AddCoin(Coin1, CurrentState);
            VengineMachine.AddCoin(Coin1, CurrentState);

            VengineMachine.BuyDrink(Drink2, CurrentState);

            Assert.AreEqual(3, CurrentState.Change);

            Assert.AreEqual(2, Drink1.Count);
            Assert.AreEqual(4, Drink2.Count);

        }

        [TestMethod]
        public void GetChange()
        {
            Init();
            CurrentState.Change = 5;
            VengineMachine.GetChange(CurrentState);
            Assert.AreEqual(9, Coin1.Count);
            Assert.AreEqual(8, Coin2.Count);
        }

        [TestMethod]
        public void ChangeCoinCount()
        {
            Init();

            VengineMachine.ChangeCoinCount(Coin1, 20);

            Assert.AreEqual(20, Coin1.Count);
        }

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

        [TestMethod]
        public void BlockUnblock()
        {
            Init();

            VengineMachine.Block(Coin1);
            VengineMachine.UnBlock(Coin2);

            Assert.IsTrue(Coin1.IsBlocking);
            Assert.IsFalse(Coin2.IsBlocking);
        }
    }
}
