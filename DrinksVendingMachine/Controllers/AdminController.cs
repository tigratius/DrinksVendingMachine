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
using NLog;

namespace DrinksVendingMachine.Controllers
{
    [AuthorizeByToken]
    public class AdminController : Controller
    {
        private readonly VengingMachineDbContext _db = new VengingMachineDbContext();

        private readonly FileManager _fileManager;
        private readonly VengineMachine _vengineMachine;

        private readonly IStrategy _strategy = new CsvImport();
        private const string ImageStoragePath = "/Content/Images/";

        private static readonly Logger Logger = LogManager.GetLogger("AdminController");

        public AdminController()
        {
            _vengineMachine = new VengineMachine(new DbSetRepository<DrinkEntity>(_db.DrinkEntities), new DbSetRepository<CoinEntity>(_db.CoinsEntities));
            _fileManager = new FileManager(_strategy);
        }

        // GET: Admin
        public ActionResult Index()
        {
            Logger.Info(" Index() called...");

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
        public JsonResult AddDrink(string name, int cost, int count, string imgPath)
        {
            Logger.Info(" AddDrink() called...");
            Logger.Info("   name = " + name);
            Logger.Info("   cost = " + cost);
            Logger.Info("   count = " + count);
            Logger.Info("   img = " + imgPath);

            if (!ValidateCost(cost))
            {
                return GetJsonErrorResultCost();
            }

            if (!ValidateCount(count))
            {
                return GetJsonErrorResultCount();
            }

            DrinkEntity drinkEntity = new DrinkEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Count = count,
                ImagePath = imgPath,
                CostPrice = cost
            };

            _vengineMachine.Add(drinkEntity);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            //Здесь данные(id = drinkEntity.Id, name, cost, count, img) можно было сериализовать и передать в качестве объекта
            return Json(new { success = true, message = "ok", id = drinkEntity.Id, name, cost, count, path = imgPath });
        }

        [HttpPost]
        public JsonResult RemoveDrink(Guid id)
        {
            Logger.Info(" RemoveDrink() called...");
            Logger.Info("   id = " + id);

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.Remove(drinkEntity);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new { id });
        }

        [HttpPost]
        public void ChangeDrinkName(Guid id, string name)
        {
            Logger.Info(" ChangeDrinkName() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   name = " + name);

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeName(drinkEntity, name);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult ChangeDrinkCount(Guid id, int count)
        {
            Logger.Info(" ChangeDrinkName() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   count = " + count);

            if (!ValidateCount(count))
            {
                return GetJsonErrorResultCount();
            }

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeCount(drinkEntity, count);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public JsonResult ChangeDrinkCost(Guid id, int cost)
        {
            Logger.Info(" ChangeDrinkName() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   cost = " + cost);

            if (!ValidateCount(cost))
            {
                return GetJsonErrorResultCost();
            }

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeCost(drinkEntity, cost);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public JsonResult ChangeDrinkImage(Guid id, string path)
        {
            Logger.Info(" ChangeDrinkImage() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   path = " + path);

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);

            _vengineMachine.ChangeImage(drinkEntity, path);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new { id, path });
        }

        [HttpPost]
        public JsonResult ChangeCoinCount(Guid id, int count)
        {
            Logger.Info(" ChangeCoinCount() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   count = " + count);

            if (!ValidateCount(count))
            {
                return GetJsonErrorResultCount();
            }

            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);

            _vengineMachine.ChangeCoinCount(coinEntity, count);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public void ChangeBlocking(Guid id, bool isBlocking)
        {
            Logger.Info(" ChangeBlocking() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   isBlocking = " + isBlocking);

            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);

            if (isBlocking)
            {
                _vengineMachine.Block(coinEntity);
            }
            else
            {
                _vengineMachine.UnBlock(coinEntity);
            }

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult Upload()
        {
            Logger.Info(" Upload() called...");

            var upload = Request.Files[0];
            string path;
            if (upload != null)
            {
                if (IsImage(upload))
                {
                    path = SaveFile(upload);
                    Logger.Info("path = " + path);
                }
                else
                {
                    var msg = "Загружаемый файл " + upload.FileName + " не является картинкой!";
                    Logger.Error(msg);
                    return Json(new JsonError(msg));
                }
            }
            else
            {
                var msg = "Нет данных для загрузки!";
                Logger.Error(msg);
                return Json(new JsonError(msg));
            }

            return Json(new { success = true, path });
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase upload)
        {
            Logger.Info(" Import() called...");

            if (upload != null)
            {
                Logger.Info(" upload.FileName = " + upload.FileName);

                Logger.Info(" import started...");
                List<Drink> drinks = _fileManager.Import(upload.InputStream);
                Logger.Info(" import ended...");

                if (drinks != null)
                {
                    foreach (var drink in drinks)
                    {
                        //Сохраняем файл в хранилище
                        var path = SaveFile(drink.ImgPath);

                        DrinkEntity drinkEntity = new DrinkEntity
                        {
                            Id = Guid.NewGuid(),
                            ImagePath = path,
                            Name = drink.Name,
                            Count = drink.Count,
                            CostPrice = drink.Cost
                        };

                        _vengineMachine.Add(drinkEntity);
                    }

                    Logger.Info(" before _db.SaveChanges()...");
                    _db.SaveChanges();
                }
                else
                {
                    //Todo: Подумать нужно ли выводить пользователю эту ошибку
                    Logger.Error("Пустые данные!");
                }
            }
            else
            {
                Logger.Error("Нет данных для загрузки!");
            }

            return RedirectToAction("Index", new {token = ValidatingTokens.GetToken()});
        }

        #region Utils
        private string SaveFile(HttpPostedFileBase postedFile)
        {
            Logger.Info("SaveFile() called");
            Logger.Info("postedFile.FileName = " + postedFile.FileName);
            var fileName = System.IO.Path.GetFileName(postedFile.FileName);
            var path = ImageStoragePath + fileName;
            postedFile.SaveAs(Server.MapPath(path));
            return path;
        }

        private string SaveFile(string fileSource)
        {
            Logger.Info("SaveFile() called");
            Logger.Info("fileSource = " + fileSource);
            var fileName = System.IO.Path.GetFileName(fileSource);
            var path = ImageStoragePath + fileName;
            System.IO.File.Copy(fileSource, Server.MapPath(path), true);
            return path;
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

        protected override void OnException(ExceptionContext filterContext)
        {
            Logger.Error("");
            Logger.Error("ERROR in " + filterContext.Controller);
            ExceptionWriter.WriteErrorDetailed(Logger, filterContext.Exception);
        }

        public bool ValidateCount(int count)
        {
            return count >= 0;
        }

        public bool ValidateCost(int cost)
        {
            return cost > 0;
        }

        private JsonResult GetJsonErrorResultCost()
        {
            var msg = " Ошибка в данных! Стоимость не может быть меньше, либо равна 0";
            return GetJsonErrorResult(msg);
        }
        private JsonResult GetJsonErrorResultCount()
        {
            var msg = " Ошибка в данных! Количество не может быть меньше 0";
            return GetJsonErrorResult(msg);
        }

        private JsonResult GetJsonErrorResult(string msg)
        {
            Logger.Error(msg);
            return Json(new JsonError(msg));
        }

        #endregion
    }
}