﻿/*
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
    public class TestDrinkLogic 
    {
        private List<DrinkEntity> _drinkEntitiesList;

        private DrinkEntity _drink1;
        private DrinkEntity _drink2;

        private CoinEntity _coin1;
        private CoinEntity _coin2;

        private CurrentStateEntity _currentState;
        private VengineMachine _vengineMachine;

        private FakeRepository<DrinkEntity> _repositoryDrink;

        public void Init()
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

            _drinkEntitiesList = new List<DrinkEntity> {_drink1, _drink2};
            var _coinEntitiesList = new List<CoinEntity> { _coin1, _coin2};

            _repositoryDrink = new FakeRepository<DrinkEntity>(_drinkEntitiesList);
            var _repositoryCoin = new FakeRepository<CoinEntity>(_coinEntitiesList);

            _vengineMachine = new VengineMachine(_repositoryDrink, _repositoryCoin);

        }
        
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

            _vengineMachine.Add(drink3);
            _repositoryDrink.SaveChanges();

            Assert.AreEqual(3, _drinkEntitiesList.Count);
        }

        [TestMethod]
        public void DrinkRemove()
        {
            Init();

            DrinkEntity drink1 = _drinkEntitiesList[0];
            _vengineMachine.Remove(drink1);

            _repositoryDrink.SaveChanges();
            Assert.AreEqual(1, _drinkEntitiesList.Count);
        }

        [TestMethod]
        public void DrinkChangeCount()
        {
            Init();

            _vengineMachine.ChangeCount(_drink1, 8);
            Assert.AreEqual(8, _drink1.Count);
        }

        [TestMethod]
        public void DrinkChangeCost()
        {
            Init();

            DrinkEntity drink1 = _drinkEntitiesList[0];
            _vengineMachine.ChangeCost(drink1, 50);

            Assert.AreEqual(50, drink1.CostPrice);
        }

        [TestMethod]
        public void DrinkChangeImage()
        {
            Init();

            _vengineMachine.ChangeImage(_drink1, "filename1");
            Assert.AreEqual("filename1", _drink1.Image);
        }

        [TestMethod]
        public void BuyDrink()
        {
            Init();

            _currentState.Deposit = 15;
            _vengineMachine.BuyDrink(_drink1, _currentState);
            Assert.AreEqual(1, _drink1.Count);
            Assert.AreEqual(5, _currentState.Change);
        }

        [TestMethod]
        public void BuyDrinkNoChange()
        {
            Init();

            _coin1.Count = 0;
            _currentState.Deposit = 15;

            _vengineMachine.BuyDrink(_drink1, _currentState);
            Assert.AreEqual(2, _drink1.Count);
            Assert.AreEqual(15, _currentState.Change);
        }

        [TestMethod]
        public void BuySomeDrinksBeforeGetChange()
        {
            Init();

            {
                _drink1.CostPrice = 2;
                _drink1.Count = 3;
            }
            {
                _drink2.CostPrice = 3;
                _drink2.Count = 5;
            }

            {
            }
            _vengineMachine.AddCoin(_coin2, _currentState);
            _vengineMachine.AddCoin(_coin2, _currentState);

            _vengineMachine.BuyDrink(_drink1, _currentState);

            Assert.AreEqual(2, _currentState.Change);

            _vengineMachine.AddCoin(_coin2, _currentState);
            _vengineMachine.AddCoin(_coin1, _currentState);
            _vengineMachine.AddCoin(_coin1, _currentState);

            _vengineMachine.BuyDrink(_drink2, _currentState);

            Assert.AreEqual(3, _currentState.Change);

            Assert.AreEqual(2, _drink1.Count);
            Assert.AreEqual(4, _drink2.Count);
            
        }

        [TestMethod]
        public void GetChange()
        {
            Init();
            _currentState.Change = 5;
            _vengineMachine.GetChange(_currentState);
            Assert.AreEqual(9, _coin1.Count);
            Assert.AreEqual(8, _coin2.Count);
        }
    }
}
*/
