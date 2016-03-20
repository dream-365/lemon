using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Transform
{
    public interface ILookUp
    {
        BsonValue LookUp(object id);
    }
}
