using System.IO;

namespace Lemon.Core
{
    public class LocalBlobClient : IBlobClient
    {
        private string _root;

        public LocalBlobClient(string root)
        {
            _root = root;
        }

        public Stream Download(string path)
        {
            var fullPath = Path.Combine(_root, path);

            var stream = new MemoryStream();

            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                fs.CopyTo(stream);
            }

            stream.Position = 0;

            return stream;
        }

        public bool Upload(Stream stream, string path)
        {
            var fullPath = Path.Combine(_root, path);

            var dir = Path.GetDirectoryName(fullPath);

            if(!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                stream.CopyTo(fs);
            }

            return true;
        }
    }
}
