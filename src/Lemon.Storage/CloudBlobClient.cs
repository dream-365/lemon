using Lemon.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using System.IO;

namespace Lemon.Storage
{
    public class AzureBlobClient : IBlobClient
    {
        private CloudBlobContainer _container;

        public AzureBlobClient(string containerName)
        {
            var connectionString = ConfigurationManager.AppSettings.Get("StorageConnectionString");

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            _container = blobClient.GetContainerReference(containerName);

            _container.CreateIfNotExists();
        }

        public Stream Download(string path)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(path);

            var stream = new MemoryStream();

            blockBlob.DownloadToStream(stream);

            stream.Position = 0;

            return stream;
        }

        public bool Upload(Stream stream, string path)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(path);

            blockBlob.UploadFromStream(stream);

            return true;
        }
    }
}
