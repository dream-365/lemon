using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Lemon.Core
{
    public class LogService
    {
        static LogService()
        {
            SetupLog4Net();
        }

        private LogService() {}

        public ILog GetLog(string name)
        {
            return LogManager.GetLogger(name);
        }

        public static LogService Default = new LogService();

        protected static void SetupLog4Net()
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
                DatePattern = "yyyyMMddHHmmss'.log'",
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
