using Lemon.Core;

namespace Lemon.Storage
{
    public class MongoDBPersistenceProvider : IPersistenceProvider
    {
        public IDocumentPersistence Get(string name)
        {
            return new MongoDBPersistence(name);
        }
    }
}
