using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinksVendingMachine.Classes.Entities;

namespace DrinksVendingMachine.Classes
{
    public class DrinkManager
    {
        private Repository<DrinkEntity> repository;

        public DrinkManager(Repository<DrinkEntity> repository)
        {
            this.repository = repository;
        }

        public void Add(DrinkEntity drinkEntity)
        {
            repository.Add(drinkEntity);
        }

        public void Remove(DrinkEntity drinkEntity)
        {
            repository.Remove(drinkEntity);
        }

        public void ChangeCount(DrinkEntity drinkEntity, int count)
        {
            drinkEntity.Count = count;
        }

        public void BuyDrink(DrinkEntity drinkEntity)
        {
            drinkEntity.Count--;
        }

        public void ChangeCost(DrinkEntity drinkEntity, int cost)
        {
            drinkEntity.CostPrice = cost;
        }

        public void ChangeImage(DrinkEntity drinkEntity, string filename)
        {
            drinkEntity.Image = filename;
        }

        public void ChangeName(DrinkEntity drinkEntity, string name)
        {
            drinkEntity.Name = name;
        }
    }
}
