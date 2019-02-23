using System;
using System.Collections.Generic;
using DrinksVendingMachine.Classes;
using DrinksVendingMachine.Classes.Entities;
using DrinksVendingMachine.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TestDrinkLogic
    {
        private FakeRepository<DrinkEntity> repositoryDrink;
        private List<DrinkEntity> drinkEntitiesList;

        [TestInitialize]
        public void Init()
        {
            var drink1 = new DrinkEntity()
            {
                CostPrice = 10,
                Count = 2,
                Id = Guid.Parse("FFDF9A9C-1FC1-4DEB-B552-45EB5E0EC48C"),
                Image = "image1",
                Name = "drink1"
            };

            var drink2 = new DrinkEntity()
            {
                CostPrice = 20,
                Count = 4,
                Id = Guid.NewGuid(),
                Image = "image2",
                Name = "drink2"
            };

            drinkEntitiesList = new List<DrinkEntity> {drink1, drink2};

            repositoryDrink = new FakeRepository<DrinkEntity>(drinkEntitiesList);
        }
        

        [TestMethod]
        public void TestDrinkAdd()
        {
            Init();

            DrinkManager drinkManager = new DrinkManager(repositoryDrink);

            //Добавление напитка

            var drink3 = new DrinkEntity()
            {
                CostPrice = 30,
                Count = 1,
                Id = Guid.NewGuid(),
                Image = "image3",
                Name = "drink3"
            };

            drinkManager.Add(drink3);

            repositoryDrink.SaveChanges();

            Assert.AreEqual(3, drinkEntitiesList.Count);
        }

        [TestMethod]
        public void TestDrinkRemove()
        {
            Init();

            DrinkManager drinkManager = new DrinkManager(repositoryDrink);

            DrinkEntity drink1 = drinkEntitiesList[0];
            drinkManager.Remove(drink1);

            repositoryDrink.SaveChanges();

            Assert.AreEqual(1, drinkEntitiesList.Count);
        }

        [TestMethod]
        public void TestDrinkChangeCount()
        {
            Init();

            DrinkManager drinkManager = new DrinkManager(repositoryDrink);

            DrinkEntity drink1 = drinkEntitiesList[0];
            drinkManager.ChangeCount(drink1, 8);

            Assert.AreEqual(8, drink1.Count);
        }

        [TestMethod]
        public void TestDrinkChangeCost()
        {
            Init();

            DrinkManager drinkManager = new DrinkManager(repositoryDrink);

            DrinkEntity drink1 = drinkEntitiesList[0];
            drinkManager.ChangeCost(drink1, 50);

            Assert.AreEqual(50, drink1.CostPrice);
        }

        [TestMethod]
        public void TestDrinkChangeImage()
        {
            Init();

            DrinkManager drinkManager = new DrinkManager(repositoryDrink);

            DrinkEntity drink1 = drinkEntitiesList[0];
            drinkManager.ChangeImage(drink1, "filename1");

            Assert.AreEqual("filename1", drink1.Image);
        }

    }
}
