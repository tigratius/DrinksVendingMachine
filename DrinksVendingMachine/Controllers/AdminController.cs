using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrinksVendingMachine.Models.Attributes;
using DrinksVendingMachine.Models.BL;
using DrinksVendingMachine.Models.BL.Importers;
using DrinksVendingMachine.Models.BL.Managers;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Entities;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Controllers
{
    /*[AuthorizeByToken]*/
    public class AdminController : Controller
    {
        private readonly VengingMachineDbContext _db = new VengingMachineDbContext();

        private readonly FileManager _fileManager;
        private readonly VengineMachine _vengineMachine;

        private readonly IStrategy _strategy = new CsvImport();
        private const string ImageStoragePath = "~/Content/Images/";

        public AdminController()
        {
            _vengineMachine = new VengineMachine(new DbSetRepository<DrinkEntity>(_db.DrinkEntities), new DbSetRepository<CoinEntity>(_db.CoinsEntities));
            _fileManager = new FileManager(_strategy);
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
            if (cost <= 0 || count < 0)
            {
                return Json("Error");
            }

            DrinkEntity drinkEntity = new DrinkEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Count = count,
                Image = img,
                CostPrice = cost
            };

            _vengineMachine.Add(drinkEntity);

            _db.SaveChanges();

            return Json(new { id = drinkEntity.Id, name, cost, count, img });
        }

        [HttpPost]
        public JsonResult RemoveDrink(Guid id)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.Remove(drinkEntity);

            _db.SaveChanges();

            return Json(new {id });
        }

        [HttpPost]
        public void ChangeDrinkName(Guid id, string name)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeName(drinkEntity, name);

            _db.SaveChanges();
        }

        [HttpPost]
        public void ChangeDrinkCount(Guid id, int count)
        {
            if (count < 0)
            {
              return;
            }

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeCount(drinkEntity, count);

            _db.SaveChanges();
        }

        [HttpPost]
        public void ChangeDrinkCost(Guid id, int cost)
        {
            if (cost <= 0)
            {
                return;
            }
            
            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeCost(drinkEntity, cost);

            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult ChangeDrinkImage(Guid id, string filename)
        {
            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeImage(drinkEntity, filename);

            _db.SaveChanges();

            return Json(new { id, filename });
        }

        [HttpPost]
        public void ChangeCoinCount(Guid id, int count)
        {
            if (count <= 0)
            {
                return;
            }

            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);

            _vengineMachine.ChangeCoinCount(coinEntity, count);

            _db.SaveChanges();
        }

        [HttpPost]
        public void ChangeBlocking(Guid id, bool isBlocking)
        {
            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);

            if (isBlocking)
            {
                _vengineMachine.Block(coinEntity);
            }
            else
            {
                _vengineMachine.UnBlock(coinEntity);
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
                if (IsImage(upload))
                {
                    fileName = SaveFile(upload);
                }
                else
                {
                    //вывести ошибку
                }
            }

            return Json(new { filename = fileName });
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                List<Drink> drinks = _fileManager.Import(upload.InputStream);

                if (drinks != null)
                {
                    foreach (var drink in drinks)
                    {
                        //Сохраняем файл в хранилище
                        var fileName = SaveFile(drink.ImgPath);

                        DrinkEntity drinkEntity = new DrinkEntity
                        {
                            Id = Guid.NewGuid(),
                            Image = fileName,
                            Name = drink.Name,
                            Count = drink.Count,
                            CostPrice = drink.Cost
                        };

                        _vengineMachine.Add(drinkEntity);
                    }

                    _db.SaveChanges();
                }
                else
                {
                    //вывести ошибку
                }
            }

            return RedirectToAction("Index");
        }

        private string SaveFile(HttpPostedFileBase postedFile)
        {
            var fileName = System.IO.Path.GetFileName(postedFile.FileName);
            postedFile.SaveAs(Server.MapPath(ImageStoragePath + fileName));
            return fileName;
        }

        private string SaveFile(string fileSource)
        {
            var fileName = System.IO.Path.GetFileName(fileSource);
            System.IO.File.Copy(fileSource, Server.MapPath(ImageStoragePath + fileName), true);
            return fileName;
        }

        private bool IsImage(HttpPostedFileBase postedFile)
        {
            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var postedFileExtension = System.IO.Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}