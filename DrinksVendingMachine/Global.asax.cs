using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.Ioc;
using DrinksVendingMachine.Models.Classes.Loggers;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Interfaces;
using NLog;

namespace DrinksVendingMachine
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IVendingMachineLogger _logger;

        public MvcApplication()
        {
            _logger = new NlogLogger("Global");
        }

        protected void Application_Start()
        {
            _logger.Info("Application_Start called...");

            /*logger.Info("Database.SetInitializer called...");
            Database.SetInitializer(new VengingMachineDbInitializer());*/

            AreaRegistration.RegisterAllAreas();
            SimpleInjectorConfig.RegisterComponents();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            _logger.Error("Application_Error called...");
            Exception ex = Server.GetLastError();
            SaveError(ex);
        }

        protected void SaveError(Exception ex)
        {
            _logger.Error("");
            ExceptionWriter.WriteErrorDetailed(_logger, ex);
        }
    }
}
