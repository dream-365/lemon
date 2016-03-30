using MongoDB.Bson;
using System.Collections.Generic;
using System.IO;

namespace Lemon.Core
{
    public interface INormalize
    {
        BsonDocument Normalize(Stream stream, IDictionary<string, object> context);
    }
}
