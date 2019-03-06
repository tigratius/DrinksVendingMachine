using System;
using System.Web;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Services;

namespace DrinksVendingMachine.Models.Interfaces
{
    public interface IAdminService
    {
        ModelView GetViewModel();
        DrinkOperationInfo AddDrink(string name, int cost, int count, string imgPath);

        void RemoveDrink(Guid id);

        void ChangeDrinkName(Guid id, string name);

        void ChangeDrinkCount(Guid id, int count);

        void ChangeDrinkCost(Guid id, int cost);

        void ChangeDrinkImage(Guid id, string path);

        void ChangeCoinCount(Guid id, int count);

        void ChangeBlocking(Guid id, bool isBlocking);

        string Import(HttpPostedFileBase upload, string imageStoragePath);
    }
}
