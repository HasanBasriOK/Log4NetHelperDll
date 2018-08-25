using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4netHelper
{
    public class Logger
    {
        public ILog _Logger { get; set; }

        public Logger()
        {
            string loggerName = "MyLogger";
            string logFolderPath = Path.GetTempPath();
            string loggerFileName = logFolderPath + "\\ApplicationLogs\\Logfile.log";
            //string loggerFileName = @"D:\Log\Logfile.log";
            string loggerConversionPattern = "%date %level : %message%newline";
            int loggerMaxSizeRollBackups = 40;
            string loggerDatePattern = "yyyy.MM.dd'.log'";
            string loggerMaximumFileSize = "50MB";
            _Logger = GetLogger(loggerName, loggerFileName, loggerConversionPattern, loggerMaxSizeRollBackups, loggerDatePattern, loggerMaximumFileSize);
        }

        public static ILog GetLogger(string LoggerName, string LogFile, string ConversionPattern, int MaxSizeRollBackups, string DatePattern, string MaximumFileSize)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Threshold = Level.All;

            var loggerA = hierarchy.LoggerFactory.CreateLogger(hierarchy, LoggerName);
            loggerA.Hierarchy = hierarchy;
            loggerA.AddAppender(CreateFileAppender(LoggerName, LogFile, ConversionPattern, MaxSizeRollBackups, DatePattern, MaximumFileSize));
            loggerA.Repository.Configured = true;
            loggerA.Level = Level.Debug;

            ILog logA = new LogImpl(loggerA);

            return logA;
        }

        private static IAppender CreateFileAppender(string name, string fileName, string ConversionPattern, int MaxSizeRollBackups, string DatePattern, string MaximumFileSize)
        {
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = ConversionPattern;
            patternLayout.ActivateOptions();

            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = name;
            appender.File = fileName;
            appender.AppendToFile = true;
            appender.MaxSizeRollBackups = MaxSizeRollBackups;
            appender.DatePattern = DatePattern;
            appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            appender.MaximumFileSize = MaximumFileSize;
            appender.Layout = patternLayout;
            appender.LockingModel = new FileAppender.MinimalLock();
            appender.StaticLogFileName = false;
            appender.ActivateOptions();
            return appender;
        }

        public enum LogLevel
        {
            Fatal,
            Info,
            Warn,
            Error,
            Debug
        }

        public void AddLog(string _LogMessage, LogLevel _LogLevel)
        {
            switch (_LogLevel)
            {
                case LogLevel.Fatal:
                    _Logger.Fatal(_LogMessage);
                    break;
                case LogLevel.Error:
                    _Logger.Error(_LogMessage);
                    break;
                case LogLevel.Warn:
                    _Logger.Warn(_LogMessage);
                    break;
                case LogLevel.Info:
                    _Logger.Info(_LogMessage);
                    break;
                case LogLevel.Debug:
                    _Logger.Debug(_LogMessage);
                    break;
                default:
                    break;
            }
        }
    }
}
