/*
using DrinksVendingMachine.Models.BL.Helpers;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Models.BL.Managers
{
    public class DrinkManager
    {
        private readonly Repository<DrinkEntity> _repository;

        public DrinkManager(Repository<DrinkEntity> repository)
        {
            _repository = repository;
        }

        public void Add(DrinkEntity drinkEntity)
        {
            _repository.Add(drinkEntity);
        }

        public void Remove(DrinkEntity drinkEntity)
        {
            _repository.Remove(drinkEntity);
        }

        public void ChangeCount(DrinkEntity drinkEntity, int count)
        {
            drinkEntity.Count = count;
        }

        public bool BuyDrink(DrinkEntity drink, CurrentStateEntity currentState)
        {
            var deposit = currentState.Deposit;
            var change = deposit - drink.CostPrice;

            currentState.Deposit = 0;

            if (IsHaveCoinsForChange(coins, change))
            {
                drink.Count--;
                currentState.Change += change;
            }
            else
            {
                /*Ситуация, когда в автомате нет сдачи 
                Нужно предупердить что пр покупке нету сдачи и отдать внесенные средства в качестве сдачи
                #1#
                currentState.Change += deposit;
            }

            return false;
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
*/
