using MongoDB.Bson;
using System.IO;

namespace Lemon.Core
{
    public interface INormalize
    {
        BsonDocument Normalize(Stream stream);
    }
}
