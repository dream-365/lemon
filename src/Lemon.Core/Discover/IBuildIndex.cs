using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Core.Discover
{
    public interface IBuildIndex
    {
        IEnumerable<BsonDocument> Build(string text);
    }
}
