using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface IDocumentTransformProvider
    {
        TransformDefinitions Get(string name);
    }
}
