using System.IO;

namespace Lemon.Core
{
    public interface IBlobClient
    {
        bool Upload(Stream stream, string path);

        Stream Download(string path);
    }
}
