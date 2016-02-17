using System.Configuration;

namespace Lemon.Core
{
    public class GeneralSettings
    {
        public static string DownloadDataQueueName { get { return ConfigurationManager.AppSettings["lemon:download"];  } }

        public static string StorageContainerName { get { return ConfigurationManager.AppSettings["lemon:storage"]; } }

        public static string ProcessDataQueueName { get { return ConfigurationManager.AppSettings["lemon:process"]; } }
    }
}
