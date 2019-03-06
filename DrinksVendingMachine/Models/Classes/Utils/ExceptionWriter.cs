using System;
using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.Classes.Utils
{
    public static class ExceptionWriter
    {
        public static void WriteErrorDetailed(IVendingMachineLogger logger, Exception ex)
        {
            logger.Error("ERROR (" + ex.GetType().Name + ")");
            WriteErrorInternal(logger, ex, 1);
        }

        private static void WriteErrorInternal(IVendingMachineLogger logger, Exception ex, int depth)
        {
            logger.Error(ex.Message);
            logger.Error(ex.StackTrace);
            if (ex.InnerException != null && depth < 10)
            {
                logger.Error("InnerException (depth=" + depth + ")");
                WriteErrorInternal(logger, ex.InnerException, depth + 1);
            }
        }
    }
}