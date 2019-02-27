using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using DrinksVendingMachine.Models.Interfaces;

namespace DrinksVendingMachine.Models.Classes.Loggers
{
     public class DelegatedLogger : ISimpleLogger
     {
         public delegate void LogDelegate(string message);

         private readonly LogDelegate _logDelegate;

         public DelegatedLogger(LogDelegate logDelegate)
         {
             _logDelegate = logDelegate;
         }

         public void WriteLine(string mess)
         {
             _logDelegate(mess);
         }
     }

    public class IndentedLogger : ISimpleLogger
    {
        private readonly ISimpleLogger _logger;
        private readonly string _indent;

        public IndentedLogger(ISimpleLogger logger, string indent)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            _logger = logger;
            _indent = indent;
        }

        public void WriteLine(string mess)
        {
            if (string.IsNullOrEmpty(_indent))
            {
                _logger.WriteLine(mess);
            }
            else
            {
                _logger.WriteLine(_indent + mess);
            }
        }
    }

    public class NLogDebugWrapper : DelegatedLogger
    {
        public NLogDebugWrapper(NLog.Logger nLogger)
            : base(nLogger.Debug)
        {
        }
    }

    public class NLogInfoWrapper : DelegatedLogger
    {
        public NLogInfoWrapper(NLog.Logger nLogger)
            : base(nLogger.Info)
        {
        }
    }

    public class NLogWarnWrapper : DelegatedLogger
    {
        public NLogWarnWrapper(NLog.Logger nLogger)
            : base(nLogger.Warn)
        {
        }
    }

    public class NLogErrorWrapper : DelegatedLogger
    {
        public NLogErrorWrapper(NLog.Logger nLogger)
            : base(nLogger.Error)
        {
        }
    }
}