using MongoDB.Bson;

namespace Lemon.Core
{
    public interface IBuildDocument
    {
        BsonDocument Build(string text);
    }
}
