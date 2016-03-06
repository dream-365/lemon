using System.Collections.Generic;

namespace Lemon.Core.Model
{
    public class DownloadContentMessageBody
    {
        public string Url { get; set; }

        public IDictionary<string, string> Context { get; set; }

        public DownloadContentMessageBody()
        {
            Context = new Dictionary<string, string>();
        }
    }
}
