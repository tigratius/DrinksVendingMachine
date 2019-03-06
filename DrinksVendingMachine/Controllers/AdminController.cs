using System;
using System.Web;
using System.Web.Mvc;
using DrinksVendingMachine.Models.Attributes;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.json;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.Classes.Utils.Validating;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Interfaces;
using DrinksVendingMachine.Models.Services;

namespace DrinksVendingMachine.Controllers
{
    [AuthorizeByToken]
    public class AdminController : Controller
    {
        private const string ImageStoragePath = "/Content/Images/";
        private readonly IAdminService _adminService;
        private readonly IVendingMachineLogger _logger;

        public AdminController(IAdminService adminService, IVendingMachineLogger logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        // GET: Admin
        public ActionResult Index()
        {
            _logger.Info("");
            _logger.Info(" Index() called...");

            ModelView model = _adminService.GetViewModel();

            return View("Index", model);
        }

        [HttpPost]
        public JsonResult AddDrink(string name, int cost, int count, string imgPath)
        {
            _logger.Info("");
            _logger.Info(" AddDrink() called...");
            _logger.Info("   name = " + name);
            _logger.Info("   cost = " + cost);
            _logger.Info("   count = " + count);
            _logger.Info("   img = " + imgPath);

            if (!Validate.ValidateCost(cost))
            {
                return JsonErrorCost();
            }

            if (!Validate.ValidateCount(count))
            {
                return JsonErrorCount();
            }

            DrinkOperationInfo operationInfo = _adminService.AddDrink(name, cost, count, imgPath);

            return Json(new { success = operationInfo.Success, message = operationInfo.Msg,
                                id = operationInfo.Drink.Id, name = operationInfo.Drink.Name,
                                    cost = operationInfo.Drink.CostPrice, count = operationInfo.Drink.Count, path = operationInfo.Drink.ImagePath });
        }

        [HttpPost]
        public JsonResult RemoveDrink(Guid id)
        {
            _logger.Info("");
            _logger.Info(" RemoveDrink() called...");
            _logger.Info("   id = " + id);
            _adminService.RemoveDrink(id);
            return Json(new { id });
        }

        [HttpPost]
        public void ChangeDrinkName(Guid id, string name)
        {
            _logger.Info("");
            _logger.Info(" ChangeDrinkName() called...");
            _logger.Info("   id = " + id);
            _logger.Info("   name = " + name);

            _adminService.ChangeDrinkName(id, name);
        }

        [HttpPost]
        public JsonResult ChangeDrinkCount(Guid id, int count)
        {
            _logger.Info("");
            _logger.Info(" ChangeDrinkName() called...");
            _logger.Info("   id = " + id);
            _logger.Info("   count = " + count);

            if (!Validate.ValidateCount(count))
            {
                return JsonErrorCount();
            }

            _adminService.ChangeDrinkCount(id, count);

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public JsonResult ChangeDrinkCost(Guid id, int cost)
        {
            _logger.Info("");
            _logger.Info(" ChangeDrinkName() called...");
            _logger.Info("   id = " + id);
            _logger.Info("   cost = " + cost);

            if (!Validate.ValidateCost(cost))
            {
                return JsonErrorCost();
            }

            _adminService.ChangeDrinkCost(id, cost);

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public JsonResult ChangeDrinkImage(Guid id, string path)
        {
            _logger.Info("");
            _logger.Info(" ChangeDrinkImage() called...");
            _logger.Info("   id = " + id);
            _logger.Info("   path = " + path);

            _adminService.ChangeDrinkImage(id, path);

            return Json(new { id, path });
        }

        [HttpPost]
        public JsonResult ChangeCoinCount(Guid id, int count)
        {
            _logger.Info("");
            _logger.Info(" ChangeCoinCount() called...");
            _logger.Info("   id = " + id);
            _logger.Info("   count = " + count);

            if (!Validate.ValidateCount(count))
            {
                return JsonErrorCount();
            }

            _adminService.ChangeCoinCount(id, count);

            return Json(new JsonSuccess());
        }

        [HttpPost]
        public void ChangeBlocking(Guid id, bool isBlocking)
        {
            _logger.Info("");
            _logger.Info(" ChangeBlocking() called...");
            _logger.Info("   id = " + id);
            _logger.Info("   isBlocking = " + isBlocking);

            _adminService.ChangeBlocking(id, isBlocking);
        }

        [HttpPost]
        public JsonResult Upload()
        {
            _logger.Info("");
            _logger.Info(" Upload() called...");

            var upload = Request.Files[0];
            string path;
            if (upload != null)
            {
                if (Validate.IsImage(upload))
                {
                    path = SaveFile(upload);
                    _logger.Info(" path = " + path);
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
            _logger.Info("");
            _logger.Info(" Import() called...");

            HttpPostedFileBase upload = Request.Files[0];

            if (upload != null)
            {
                string result = _adminService.Import(upload, ImageStoragePath);

                return !String.IsNullOrEmpty(result) ? JsonErrorLog(result) : Json(new JsonSuccess());
            }

            return JsonErrorEmptyData();
        }

        #region Utils
        private string SaveFile(HttpPostedFileBase postedFile)
        {
            _logger.Info(" SaveFile() called");
            _logger.Info(" postedFile.FileName = " + postedFile.FileName);
            var fileName = System.IO.Path.GetFileName(postedFile.FileName);
            var path = ImageStoragePath + fileName;
            postedFile.SaveAs(Server.MapPath(path));
            return path;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error("");
            _logger.Error(" ERROR in " + filterContext.Controller);
            ExceptionWriter.WriteErrorDetailed(_logger, filterContext.Exception);
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
            _logger.Error(msg);
            return Json(new JsonError(msg));
        }

        #endregion
    }
}