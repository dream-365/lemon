using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public class LinksBuilder
    {
        public LinksBuilder SuccessTo(LinkObject linkObject)
        {
            return this;
        }


        public LinksBuilder ErrorTo(LinkObject linkObject)
        {
            return this;
        }


        public void End()
        {
            throw new NotImplementedException();
        }
    }
}
