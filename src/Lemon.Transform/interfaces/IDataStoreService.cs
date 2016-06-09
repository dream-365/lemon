using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface IDataStoreService
    {
        DataInputModel GetDataInput(string name);

        DataOutputModel GetDataOutput(string name);
    }
}
