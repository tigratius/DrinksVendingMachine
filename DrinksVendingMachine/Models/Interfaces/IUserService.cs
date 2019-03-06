using System;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.json;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Services;

namespace DrinksVendingMachine.Models.Interfaces
{
    public interface IUserService
    {
        int AddCoin(Guid id);

        DrinkOperationInfo BuyDrink(Guid id);

        JsonBase IsCanBuy(Guid id);

        void GetChange();

        ModelView GetViewModel();
    }
}