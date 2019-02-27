using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.Core;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using NLog;

namespace DrinksVendingMachine.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly VengingMachineDbContext _db = new VengingMachineDbContext();
        private readonly VengineMachine _vengineMachine;

        private static readonly Logger Logger = LogManager.GetLogger("UserController");

        public UserController()
        {
            _vengineMachine = new VengineMachine(new DbSetRepository<DrinkEntity>(_db.DrinkEntities), new DbSetRepository<CoinEntity>(_db.CoinsEntities));
        }

        // GET: User
        public ActionResult Index()
        {
            Logger.Info(" Index() called...");

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
            Logger.Info(" AddCoin() called...");
            Logger.Info(" id = " + id);

            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();

            _vengineMachine.AddCoin(coinEntity, currentState);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new { deposit = currentState.Deposit });
        }

        [HttpPost]
        public JsonResult BuyDrink(Guid id)
        {
            Logger.Info(" BuyDrink() called...");
            Logger.Info(" id = " + id);

            DrinkEntity drink = _db.DrinkEntities.First(d => d.Id == id);
            CurrentStateEntity currentState = _db.CurrentStateEntities.First();

            bool success = true;
            var msg = "";

            if (!_vengineMachine.BuyDrink(drink, currentState))
            {
                success = false;
                msg = "Невозможно совершить покупку! В автомате нет сдачи! Деньги возвращены полностью в качестве сдачи";
                Logger.Warn(msg);
            }

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new {success, msg, change = currentState.Change });
        }

        [HttpGet]
        public void GetChange()
        {
            Logger.Info(" GetChange() called...");

            CurrentStateEntity currentState = _db.CurrentStateEntities.First();
            _vengineMachine.GetChange(currentState);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Logger.Error("");
            Logger.Error("ERROR in " + filterContext.Controller);
            ExceptionWriter.WriteErrorDetailed(Logger, filterContext.Exception);
        }
    }
}