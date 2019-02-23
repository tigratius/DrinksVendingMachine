using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrinksVendingMachine.Classes;
using DrinksVendingMachine.Classes.Entities;
using DrinksVendingMachine.Models;

namespace DrinksVendingMachine.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly VengingMachineDbContext _db = new VengingMachineDbContext();

        private readonly DrinkManager drinkManager;
        private readonly CoinManager coinManager;

        public UserController()
        {
            drinkManager = new DrinkManager(new DbSetRepository<DrinkEntity>(_db.DrinkEntities));
            coinManager = new CoinManager(new DbSetRepository<CoinEntity>(_db.CoinsEntities));
        }

        // GET: User
        public ActionResult Index()
        {
            ModelView model =
                new ModelView
                {
                    Drinks = new List<DrinkEntity>(_db.DrinkEntities.OrderBy(d => d.Name).ToList()),
                    Coins = new List<CoinEntity>(_db.CoinsEntities.OrderBy(c => c.Value).ToList()),
                    Deposit = _db.CurrentStateEntities.First().Deposit,
                    Change = _db.CurrentStateEntities.First().Change
                };

            return View("Index", model);
        }

        public JsonResult AddCoin(Guid id)
        {
            CoinEntity coinEntity = _db.CoinsEntities.FirstOrDefault(c => c.Id == id);
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();

            if (coinEntity != null)
            {
                coinEntity.Count++;
                currentState.Deposit += (int)coinEntity.Value;
            }
            _db.SaveChanges();

            return Json(new { deposit = currentState.Deposit });
        }


        public JsonResult BuyDrink(Guid id)
        {
            DrinkEntity drink = _db.DrinkEntities.FirstOrDefault(d => d.Id == id);
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();

            if (drink != null)
            {
                drinkManager.BuyDrink(drink);
                currentState.Change += currentState.Deposit - drink.CostPrice;
                currentState.Deposit = 0;
            }

            _db.SaveChanges();

            return Json(new { change = currentState.Change });
        }

        public void GetChange()
        {
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();
            List<CoinEntity> coins = _db.CoinsEntities.OrderByDescending(c=> (int) c.Value).ToList();

            var change = currentState.Change;
            IList<Coin> results = VendingMachine.Calculate(coins, change);

            if (results != null)
            {
                foreach (var coin in results)
                {
                    CoinEntity coinEntity = coins.FirstOrDefault(c => (int) c.Value == coin.Value);
                    if (coinEntity != null) coinEntity.Count -= coin.Count;
                }

                currentState.Change = 0;
                _db.SaveChanges();
            }
        }
    }
}