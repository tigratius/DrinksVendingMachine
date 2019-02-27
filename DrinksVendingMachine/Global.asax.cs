using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DrinksVendingMachine.Models.Classes;
using DrinksVendingMachine.Models.Classes.Utils;
using DrinksVendingMachine.Models.DB;
using NLog;

namespace DrinksVendingMachine
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger logger = LogManager.GetLogger("Global");

        protected void Application_Start()
        {
            logger.Info("Application_Start called...");

/*            logger.Info("Database.SetInitializer called...");
            Database.SetInitializer(new VengingMachineDbInitializer());*/

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            logger.Error("Application_Error called...");
            Exception ex = Server.GetLastError();
            SaveError(ex);
        }

        protected void SaveError(Exception ex)
        {
            logger.Error("");
            ExceptionWriter.WriteErrorDetailed(logger, ex);
        }
    }
}
