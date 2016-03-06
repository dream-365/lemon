using Lemon.Core.Discover;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples.msdn.discover
{
    public class SampleBuildIndexProvider : IBuildIndexProvider
    {
        public IBuildIndex Get(string name)
        {
            return new MSDNIndexBuilder();
        }
    }
}
