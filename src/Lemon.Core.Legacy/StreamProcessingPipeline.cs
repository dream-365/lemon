using System.Collections.Generic;
using System.IO;

namespace Lemon.Core
{
    public class StreamProcessingPipeline
    {
        private IEnumerable<IStreamProcessingModule> _modules;

        public StreamProcessingPipeline(IEnumerable<IStreamProcessingModule> modules)
        {
            _modules = modules;
        }

        public Dictionary<string, object> Process(Stream stream)
        {
            var metadata = new Dictionary<string, object>();

            foreach(var module in _modules)
            {
                module.OnProcess(metadata, stream);
            }

            return metadata;
        }
    }
}
