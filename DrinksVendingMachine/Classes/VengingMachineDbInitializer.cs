﻿using System;
using System.Data.Entity;
using DrinksVendingMachine.Classes.Entities;

namespace DrinksVendingMachine.Classes
{
    public class VengingMachineDbInitializer : DropCreateDatabaseAlways<VengingMachineDbContext>
    {
        protected override void Seed(VengingMachineDbContext db)
        {
            db.CoinsEntities.Add(new CoinEntity() { Id = Guid.NewGuid(), Value = ValueCoins.One, Count = 10, IsBlocking = false });
            db.CoinsEntities.Add(new CoinEntity() { Id = Guid.NewGuid(), Value = ValueCoins.Two, Count = 10, IsBlocking = true });
            db.CoinsEntities.Add(new CoinEntity() { Id = Guid.NewGuid(), Value = ValueCoins.Five, Count = 10, IsBlocking = false });
            db.CoinsEntities.Add(new CoinEntity() { Id = Guid.NewGuid(), Value = ValueCoins.Ten, Count = 10, IsBlocking = false });

            db.DrinkEntities.Add(new DrinkEntity()
            {
                Id = Guid.NewGuid(),
                CostPrice = 10,
                Count = 5,
                Image = "cola.jpg",
                Name = "Cola"
            });

            db.DrinkEntities.Add(new DrinkEntity()
            {
                Id = Guid.NewGuid(),
                CostPrice = 5,
                Count = 5,
                Image = "pepsi.png",
                Name = "Pepsi"
            });

            db.CurrentStateEntities.Add(new CurrentStateEntity()
            {
                Id = Guid.NewGuid(),
                Deposit = 0,
                Change = 0
            });

            base.Seed(db);
        }
    }
}