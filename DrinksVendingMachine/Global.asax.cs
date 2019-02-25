using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NLog;

namespace DrinksVendingMachine
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger logger = LogManager.GetLogger("Global");

        protected void Application_Start()
        {
            logger.Info("Application_Start called...");

            /*Database.SetInitializer(new VengingMachineDbInitializer());*/

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            logger.Error("Application_Error called...");
            Exception ex = Server.GetLastError();
            SaveError(ex);
        }

        protected void SaveError(Exception ex)
        {
         /*   logger.Error("");
            ExceptionWriter.WriteErrorDetailed(logger, ex);*/
        }
    }
}
