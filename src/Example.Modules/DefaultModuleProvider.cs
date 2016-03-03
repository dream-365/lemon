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
            if(name == "msdn")
            {
                return new MSDNMetadataModule();
            } else if(name == "powerbi")
            {
                return new PowerBIMeatadataModule();
            }

            throw new NotImplementedException(name);
        }
    }
}
