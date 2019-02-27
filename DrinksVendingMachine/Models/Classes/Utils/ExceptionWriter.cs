﻿using System;
using DrinksVendingMachine.Models.Classes.Loggers;
using DrinksVendingMachine.Models.Interfaces;
using NLog;

namespace DrinksVendingMachine.Models.Classes.Utils
{
    public static class ExceptionWriter
    {
        public static void WriteErrorDetailed(Logger nLogger, Exception ex)
        {
            WriteErrorDetailed(new NLogErrorWrapper(nLogger), ex);
        }

        public static void WriteErrorDetailed(ISimpleLogger logger, Exception ex)
        {
            logger.WriteLine("ERROR (" + ex.GetType().Name + ")");
            WriteErrorInternal(logger, ex, 1);
        }

        private static void WriteErrorInternal(ISimpleLogger logger, Exception ex, int depth)
        {
            logger.WriteLine(ex.Message);
            logger.WriteLine(ex.StackTrace);
            if (ex.InnerException != null && depth < 10)
            {
                var indentedLogger = new IndentedLogger(logger, "    ");
                indentedLogger.WriteLine("InnerException (depth=" + depth + ")");
                WriteErrorInternal(indentedLogger, ex.InnerException, depth + 1);
            }
        }
    }
}