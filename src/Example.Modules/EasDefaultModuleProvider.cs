using Lemon.Core;
using System;

namespace eas.modules
{
    public class EasDefaultModuleProvider : INormaliztionProvider
    {
        public INormalize Activate(string name)
        {
            if(name == "msdn")
            {
                return new MSDNXMLNormalization();
            } else if(name == "powerbi")
            {
                return new PowerBIHtmlNormalization2();
            }

            throw new NotImplementedException(name);
        }
    }
}
