using System.Collections.Generic;

namespace Lemon.Core.Model
{
    public class ProcessContentMessageBody
    {
        public string OrignalUrl { get; set; }

        public string BlobPath { get; set; }

        public IDictionary<string, string> Context { get; set; }

        public ProcessContentMessageBody()
        {
            Context = new Dictionary<string, string>();
        }
    }
}
