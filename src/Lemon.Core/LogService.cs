using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;

namespace Lemon.Core
{
    public class LogService
    {
        static LogService()
        {
            SetupLog4Net();
        }

        private ILog _logger;

        private LogService()
        {
            _logger = LogManager.GetLogger("lemon.log");
        }

        public static LogService Default = new LogService();

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Error(string message, Exception ex = null)
        {
            _logger.Error(message, ex);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        private static void SetupLog4Net()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };

            patternLayout.ActivateOptions();

            RollingFileAppender rollingFileAppender = new RollingFileAppender
            {
                AppendToFile = true,
                File = @"logs\",
                Layout = patternLayout,
                MaxSizeRollBackups = 5,
                DatePattern = "yyyyMMddHHss'.log'",
                MaximumFileSize = "1GB",
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                StaticLogFileName = false
            };

            rollingFileAppender.ActivateOptions();

            hierarchy.Root.AddAppender(rollingFileAppender);

            hierarchy.Root.Level = Level.Info;

            hierarchy.Configured = true;
        }
    }
}
