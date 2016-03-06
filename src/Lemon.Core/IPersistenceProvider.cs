namespace Lemon.Core
{
    public interface IPersistenceProvider
    {
        IDocumentPersistence Get(string name);
    }
}
