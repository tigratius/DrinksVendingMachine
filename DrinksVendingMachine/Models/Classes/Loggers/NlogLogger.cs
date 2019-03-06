using DrinksVendingMachine.Models.DB;
using DrinksVendingMachine.Models.Interfaces;
using NLog;

namespace DrinksVendingMachine.Models.Classes.Loggers
{
    public class NlogLogger : IVendingMachineLogger
    {
        private readonly NLog.Logger _logger;

        public NlogLogger(string logName)
        {
            _logger = LogManager.GetLogger(logName);
        }

        public void Info(string mes)
        {
            _logger.Info(mes);
        }

        public void Error(string mes)
        {
            _logger.Error(mes);
        }

        public void Warning(string mes)
        {
            _logger.Warn(mes);
        }
    }
}