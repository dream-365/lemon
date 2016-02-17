using Lemon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Modules
{
    public class DefaultModuleProvider : IStreamProcessingModuleProvider
    {
        public IStreamProcessingModule Activate(string name)
        {
            return new MSDNMetadataModule();
        }
    }
}
