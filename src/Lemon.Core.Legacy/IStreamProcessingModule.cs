using System.Collections.Generic;
using System.IO;

namespace Lemon.Core
{
    public interface IStreamProcessingModule
    {
        void OnProcess(IDictionary<string, object> metadata, Stream stream);
    }
}
