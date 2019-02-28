using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrinksVendingMachine.Models.Attributes;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.Importers;
using DrinksVendingMachine.Models.Classes.json;
using DrinksVendingMachine.Models.Classes.Loggers;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.Classes.Utils.Validating;
using DrinksVendingMachine.Models.Core;
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

        private readonly FileContext _fileContext;
        private readonly VengineMachine _vengineMachine;

        private readonly IStrategy _strategy = new CsvImport();
        private const string ImageStoragePath = "/Content/Images/";

        private static readonly Logger Logger = LogManager.GetLogger("AdminController");

        public AdminController()
        {
            _vengineMachine = new VengineMachine(new DbSetRepository<DrinkEntity>(_db.DrinkEntities), new DbSetRepository<CoinEntity>(_db.CoinsEntities));
            _fileContext = new FileContext(_strategy);
        }

        // GET: Admin
        public ActionResult Index()
        {
            Logger.Info("");
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
            Logger.Info("");
            Logger.Info(" AddDrink() called...");
            Logger.Info("   name = " + name);
            Logger.Info("   cost = " + cost);
            Logger.Info("   count = " + count);
            Logger.Info("   img = " + imgPath);

            if (!Validate.ValidateCost(cost))
            {
                return JsonErrorCost();
            }

            if (!Validate.ValidateCount(count))
            {
                return JsonErrorCount();
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
            Logger.Info("");
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
            Logger.Info("");
            Logger.Info(" ChangeDrinkName() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   name = " + name);

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);
            Info.DrinkInfo(drinkEntity, new NLogInfoWrapper(Logger));

            _vengineMachine.ChangeName(drinkEntity, name);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();
        }

        [HttpPost]
        public JsonResult ChangeDrinkCount(Guid id, int count)
        {
            Logger.Info("");
            Logger.Info(" ChangeDrinkName() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   count = " + count);

            if (!Validate.ValidateCount(count))
            {
                return JsonErrorCount();
            }

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);
            Info.DrinkInfo(drinkEntity, new NLogInfoWrapper(Logger));

            _vengineMachine.ChangeCount(drinkEntity, count);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public JsonResult ChangeDrinkCost(Guid id, int cost)
        {
            Logger.Info("");
            Logger.Info(" ChangeDrinkName() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   cost = " + cost);

            if (!Validate.ValidateCost(cost))
            {
                return JsonErrorCost();
            }

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);
            Info.DrinkInfo(drinkEntity, new NLogInfoWrapper(Logger));

            _vengineMachine.ChangeCost(drinkEntity, cost);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public JsonResult ChangeDrinkImage(Guid id, string path)
        {
            Logger.Info("");
            Logger.Info(" ChangeDrinkImage() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   path = " + path);

            DrinkEntity drinkEntity = _db.DrinkEntities.First(d => d.Id == id);
            Info.DrinkInfo(drinkEntity, new NLogInfoWrapper(Logger));

            _vengineMachine.ChangeImage(drinkEntity, path);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new { id, path });
        }

        [HttpPost]
        public JsonResult ChangeCoinCount(Guid id, int count)
        {
            Logger.Info("");
            Logger.Info(" ChangeCoinCount() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   count = " + count);

            if (!Validate.ValidateCount(count))
            {
                return JsonErrorCount();
            }

            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);
            Info.CoinInfo(coinEntity, new NLogInfoWrapper(Logger));

            _vengineMachine.ChangeCoinCount(coinEntity, count);

            Logger.Info(" before _db.SaveChanges()...");
            _db.SaveChanges();

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public void ChangeBlocking(Guid id, bool isBlocking)
        {
            Logger.Info("");
            Logger.Info(" ChangeBlocking() called...");
            Logger.Info("   id = " + id);
            Logger.Info("   isBlocking = " + isBlocking);

            CoinEntity coinEntity = _db.CoinsEntities.First(c => c.Id == id);
            Info.CoinInfo(coinEntity, new NLogInfoWrapper(Logger));

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
            Logger.Info("");
            Logger.Info(" Upload() called...");

            var upload = Request.Files[0];
            string path;
            if (upload != null)
            {
                if (Validate.IsImage(upload))
                {
                    path = SaveFile(upload);
                    Logger.Info(" path = " + path);
                }
                else
                {
                    return JsonErrorLog(" Загружаемый файл " + upload.FileName + " не является картинкой!");
                }
            }
            else
            {
                return JsonErrorEmptyData();
            }

            return Json(new { success = true, path });
        }

        [HttpPost]
        public JsonResult Import()
        {
            Logger.Info("");
            Logger.Info(" Import() called...");

            var upload = Request.Files[0];

            if (upload != null)
            {
                Logger.Info(" upload.FileName = " + upload.FileName);

                var ext = System.IO.Path.GetExtension(upload.FileName);

                if (_fileContext.IsAllowedExtension(ext))
                {
                    Logger.Info(" import started...");
                    List<Drink> drinks = null;
                    try
                    {
                        drinks = _fileContext.Import(upload.InputStream);
                    }
                    catch (Exception e)
                    {
                        Logger.Error(" Неправильная структура файла!");
                        ExceptionWriter.WriteErrorDetailed(Logger, e);
                    }

                    Logger.Info(" import ended...");

                    if (drinks != null)
                    {
                        foreach (var drink in drinks)
                        {
                            
                            //Если по каким-то причинам при импорте не нашлась картинка по указанному пути берем дефолтную
                            string path=GetPathDefaultImg();
                            try
                            {
                                //Сохраняем файл в хранилище
                                path = SaveFile(drink.ImgPath);
                            }
                            catch (Exception e)
                            {
                                Logger.Error(" Указан неверный путь к файлу!");
                                ExceptionWriter.WriteErrorDetailed(Logger, e);
                            }

                            DrinkEntity drinkEntity = new DrinkEntity
                            {
                                Id = Guid.NewGuid(),
                                ImagePath = path,
                                Name = drink.Name,
                                Count = drink.Count,
                                CostPrice = drink.Cost
                            };
                            Info.DrinkInfo(drinkEntity, new NLogInfoWrapper(Logger));

                            _vengineMachine.Add(drinkEntity);
                        }

                        Logger.Info(" before _db.SaveChanges()...");
                        _db.SaveChanges();
                    }
                    else
                    {
                        return JsonErrorLog(" Пустые данные!");
                    }
                }
                else
                {
                    return JsonErrorLog(" Файл имеет неверный формат!");
                }
            }
            else
            {
                return JsonErrorEmptyData();
            }

            return Json(new JsonSuccess());
        }

        #region Utils
        private string SaveFile(HttpPostedFileBase postedFile)
        {
            Logger.Info(" SaveFile() called");
            Logger.Info(" postedFile.FileName = " + postedFile.FileName);
            var fileName = System.IO.Path.GetFileName(postedFile.FileName);
            var path = ImageStoragePath + fileName;
            postedFile.SaveAs(Server.MapPath(path));
            return path;
        }

        private string SaveFile(string fileSource)
        {
            Logger.Info(" SaveFile() called");
            Logger.Info(" fileSource = " + fileSource);
            var fileName = System.IO.Path.GetFileName(fileSource);
            var path = ImageStoragePath + fileName;
            System.IO.File.Copy(fileSource, Server.MapPath(path), true);
            return path;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Logger.Error("");
            Logger.Error(" ERROR in " + filterContext.Controller);
            ExceptionWriter.WriteErrorDetailed(Logger, filterContext.Exception);
        }

       private JsonResult JsonErrorCost()
        {
            return JsonErrorLog(" Ошибка в данных! Стоимость не может быть меньше, либо равна 0");
        }
        private JsonResult JsonErrorCount()
        {
            return JsonErrorLog(" Ошибка в данных! Количество не может быть меньше 0");
        }

        private JsonResult JsonErrorEmptyData()
        {
            return JsonErrorLog(" Нет данных для загрузки!");
        }

        private JsonResult JsonErrorLog(string msg)
        {
            Logger.Error(msg);
            return Json(new JsonError(msg));
        }

        private string GetPathDefaultImg()
        {
            return ImageStoragePath + "default/def.jpg";
        }
        #endregion
    }
}