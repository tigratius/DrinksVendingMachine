using System;
using System.Web.Mvc;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Interfaces;
using DrinksVendingMachine.Models.Services;

namespace DrinksVendingMachine.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {

        private readonly IVendingMachineLogger _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IVendingMachineLogger logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: User
        public ActionResult Index()
        {
            _logger.Info("");
            _logger.Info(" Index() called...");

            ModelView model = _userService.GetViewModel();

            return View("Index", model);
        }

        [HttpPost]
        public JsonResult AddCoin(Guid id)
        {
            _logger.Info("");
            _logger.Info(" AddCoin() called...");
            _logger.Info(" id = " + id);

            int deposit = _userService.AddCoin(id);

            return Json(new { deposit });
        }

        [HttpPost]
        public JsonResult BuyDrink(Guid id)
        {
            _logger.Info("");
            _logger.Info(" BuyDrink() called...");
            _logger.Info(" id = " + id);

            DrinkOperationInfo drinkOperationInfo = _userService.BuyDrink(id);

            return Json(new {success = drinkOperationInfo.Success, msg = drinkOperationInfo.Msg, change = drinkOperationInfo.Change, id=drinkOperationInfo.Drink.Id, count = drinkOperationInfo.Drink.Count});
        }

        [HttpPost]
        public JsonResult IsCanBuy(Guid id)
        {
            _logger.Info("");
            _logger.Info(" IsCanBuy() called...");

            return Json(_userService.IsCanBuy(id));
        }

        [HttpGet]
        public void GetChange()
        {
            _logger.Info("");
            _logger.Info(" GetChange() called...");

            _userService.GetChange();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error("");
            _logger.Error(" ERROR in " + filterContext.Controller);
            ExceptionWriter.WriteErrorDetailed(_logger, filterContext.Exception);
        }
    }
}