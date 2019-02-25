using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DrinksVendingMachine.Models.BL;
using DrinksVendingMachine.Models.BL.Managers;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;

namespace DrinksVendingMachine.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly VengingMachineDbContext _db = new VengingMachineDbContext();
        private readonly VengineMachine _vengineMachine;

        public UserController()
        {
            _vengineMachine = new VengineMachine(new DbSetRepository<DrinkEntity>(_db.DrinkEntities), new DbSetRepository<CoinEntity>(_db.CoinsEntities));
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

        [HttpPost]
        public JsonResult AddCoin(Guid id)
        {
            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();

            _vengineMachine.AddCoin(coinEntity, currentState);
            _db.SaveChanges();

            return Json(new { deposit = currentState.Deposit });
        }

        [HttpPost]
        public JsonResult BuyDrink(Guid id)
        {
            DrinkEntity drink = _db.DrinkEntities.First(d => d.Id == id);
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();

            _vengineMachine.BuyDrink(drink, currentState);
            _db.SaveChanges();

            return Json(new { change = currentState.Change });
        }

        [HttpGet]
        public void GetChange()
        {
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();
            _vengineMachine.GetChange(currentState);
            _db.SaveChanges();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
//            ExceptionHelper.HandleException(filterContext, logger);
        }
    }
}