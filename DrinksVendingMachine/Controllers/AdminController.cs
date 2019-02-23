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
    [AuthorizeByToken]
    public class AdminController : Controller
    {
        private readonly VengingMachineDbContext _db = new VengingMachineDbContext();

        private readonly DrinkManager drinkManager;
        private readonly CoinManager coinManager;

        public AdminController()
        {
            drinkManager = new DrinkManager(new DbSetRepository<DrinkEntity>(_db.DrinkEntities));
            coinManager = new CoinManager(new DbSetRepository<CoinEntity>(_db.CoinsEntities));
        }

        // GET: Admin
        public ActionResult Index()
        {
            ModelView model =
                new ModelView
                {
                    Drinks = new List<DrinkEntity>(_db.DrinkEntities.OrderBy(d => d.Name).ToList()),
                    Coins = new List<CoinEntity>(_db.CoinsEntities.OrderBy(c => c.Value).ToList()),
                    Token = ValidatingTokens.GetToken()
                };

            return View("Index", model);
        }

        [HttpPost]
        public JsonResult AddDrink(string name, int cost, int count, string img)
        {
            DrinkEntity drinkEntity = new DrinkEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Count = count,
                Image = img,
                CostPrice = cost
            };

            drinkManager.Add(drinkEntity);

            _db.SaveChanges();

            return Json(new { id = drinkEntity.Id, name, cost, count, img });
        }

        [HttpPost]
        public JsonResult RemoveDrink(Guid id)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.FirstOrDefault(d => d.Id == id);

            drinkManager.Remove(drinkEntity);

            _db.SaveChanges();

            return Json(new { id = id });
        }

        [HttpPost]
        public void ChangeDrinkName(Guid id, string name)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.FirstOrDefault(d => d.Id == id);

            drinkManager.ChangeName(drinkEntity, name);

            _db.SaveChanges();
        }

        [HttpPost]
        public void ChangeDrinkCount(Guid id, int count)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.FirstOrDefault(d => d.Id == id);

            drinkManager.ChangeCount(drinkEntity, count);

            _db.SaveChanges();
        }

        [HttpPost]
        public void ChangeDrinkCost(Guid id, int cost)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.FirstOrDefault(d => d.Id == id);

            drinkManager.ChangeCost(drinkEntity, cost);

            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult ChangeDrinkImage(Guid id, string filename)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.FirstOrDefault(d => d.Id == id);

            drinkManager.ChangeImage(drinkEntity, filename);

            _db.SaveChanges();

            return Json(new {id, filename});
        }

        [HttpPost]
        public void ChangeCoinCount(Guid id, int count)
        {
            CoinEntity coinEntity = _db.CoinsEntities.FirstOrDefault(c => c.Id == id);

            coinManager.ChangeCoinCount(coinEntity, count);

            _db.SaveChanges();
        }

        [HttpPost]
        public void ChangeBlocking(Guid id, bool isBlocking)
        {
            CoinEntity coinEntity = _db.CoinsEntities.FirstOrDefault(c => c.Id == id);

            if (isBlocking)
            {
                coinManager.Block(coinEntity);
            }
            else
            {
                coinManager.UnBlock(coinEntity);
            }

            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            var upload = Request.Files[0];
            string fileName = "";
            if (upload != null)
            {
                fileName = System.IO.Path.GetFileName(upload.FileName);
                upload.SaveAs(Server.MapPath("~/Content/Images/" + fileName));
            }
            return Json(new {filename = fileName });
        }
    }
}