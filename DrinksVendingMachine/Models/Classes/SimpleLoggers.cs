using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using NLog;

namespace DrinksVendingMachine.Models.Classes
{
    public interface ISimpleLogger
    {
        void WriteLine(string mess);
    }

    public class FileLogger : ISimpleLogger, IDisposable
    {
        public FileLogger(string loggerFileName, bool append)
            : this(loggerFileName, append, false)
        {
        }

        public FileLogger(string loggerFileName, bool append, bool noDateTime)
        {
            fileInfo = new FileInfo(loggerFileName);
            this.append = append;
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            NoDate = noDateTime;
            writer = new StreamWriter(loggerFileName, append, encoding) { AutoFlush = true };
        }

        protected StreamWriter writer;
        private readonly FileInfo fileInfo;
        protected bool append;
        protected Encoding encoding = Encoding.GetEncoding("Windows-1251");
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("ru-RU"); // "en-US"
        public bool NoDate { get; set; }

        public FileInfo FileInfo
        {
            get { return fileInfo; }
        }

        public void Close()
        {
            writer.Close();
        }

        public void Flush()
        {
            writer.Flush();
        }

        #region ISimpleLogger Members

        public void WriteLine(string mess)
        {
            try
            {
                if (NoDate)
                {
                    writer.WriteLine(mess);
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    string time = string.Format(cultureInfo, "{0}.{1:000}", dt, dt.Millisecond);
                    writer.WriteLine(time + ": " + mess);
                }
            }
            catch
            {
                TraceLogger.Instance.WriteLine("FileLogger.WriteLine() ERROR: " + FileInfo.FullName);
                throw;
            }
        }

        #endregion

        public void Dispose()
        {
            //            _writer.WriteLine(DateTime.Now + ": Logger Disposed");
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
        }
    }


    public class CompositeLogger : ISimpleLogger, IDisposable
    {
        private ISimpleLogger[] _loggers;

        public CompositeLogger(params ISimpleLogger[] loggers)
        {
            _loggers = loggers;
        }

        public void WriteLine(string mess)
        {
            foreach (ISimpleLogger logger in _loggers)
            {
                logger.WriteLine(mess);
            }
        }

        public void Dispose()
        {
            if (_loggers == null)
            {
                return;
            }

            foreach (ISimpleLogger logger in _loggers)
            {
                IDisposable disposable = logger as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }


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

    public class ConsoleLogger : ISimpleLogger
    {
        private bool printTime;

        public ConsoleLogger()
            : this(true)
        {
        }

        public ConsoleLogger(bool printTime)
        {
            this.printTime = printTime;
        }

        #region ISimpleLogger Members

        public void WriteLine(string mess)
        {
            string time = (printTime ? DateTime.Now + ": " : "");
            Console.WriteLine(time + mess);
        }

        #endregion
    }

    public class TraceLogger : ISimpleLogger
    {
        public readonly static TraceLogger Instance = new TraceLogger();

        public void WriteLine(string mess)
        {
            Trace.WriteLine(mess);
        }
    }

    public class EmptyLogger : ISimpleLogger
    {
        public readonly static EmptyLogger Instance = new EmptyLogger();

        public void WriteLine(string mess)
        {
            // Do nothing
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
        public NLogDebugWrapper(Logger nLogger)
            : base(nLogger.Debug)
        {
        }
    }

    public class NLogInfoWrapper : DelegatedLogger
    {
        public NLogInfoWrapper(Logger nLogger)
            : base(nLogger.Debug)
        {
        }
    }

    public class NLogWarnWrapper : DelegatedLogger
    {
        public NLogWarnWrapper(Logger nLogger)
            : base(nLogger.Warn)
        {
        }
    }

    public class NLogErrorWrapper : DelegatedLogger
    {
        public NLogErrorWrapper(Logger nLogger)
            : base(nLogger.Error)
        {
        }
    }
}