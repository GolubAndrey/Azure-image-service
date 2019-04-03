using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureStorage
{
	public class ImageBlobRepository:IImageRepository
	{
        private readonly string _connectionString;

        private readonly string _containerName;

        private readonly string _queueName;

        public ImageBlobRepository(string connectionString, string containerName, string queueName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
            _queueName = queueName;
        }

        public async Task LoadImageAsync(string blobName, Stream stream, string contenType)
        {
            CloudBlockBlob blob = await GetCloudBlockBlobAsync(blobName);
            
            blob.Properties.ContentType = contenType;
            
            await blob.UploadFromStreamAsync(stream);
        }

        public async Task GetImageAsync(string blobName, Stream stream)
        {
            
            try
            {
                CloudBlockBlob blob = await GetCloudBlockBlobAsync(blobName);

                await blob.DownloadToStreamAsync(stream);
            }
            catch(Microsoft.WindowsAzure.Storage.StorageException ex)
            {
                throw new FileNotFoundException($"Image with name {blobName} not found in blob storage");
            }
        }

        public async Task EnqueueMessage(string stringMessage)
        {
            var queue = await GetQueue();
            
            CloudQueueMessage message = new CloudQueueMessage(stringMessage);
            queue.AddMessage(message);
        }

        private async Task<CloudBlobContainer> GetCludBlobContainerAsync()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_containerName);

            await cloudBlobContainer.CreateIfNotExistsAsync();

            return cloudBlobContainer;
        }

        private async Task<CloudBlockBlob> GetCloudBlockBlobAsync(string blobName)
        {
            CloudBlobContainer blobContainer = await this.GetCludBlobContainerAsync();
            
            CloudBlockBlob cloudBlockBlob = blobContainer.GetBlockBlobReference(blobName);

            return cloudBlockBlob;
        }

        private async Task<CloudQueue> GetQueue()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference(_queueName);

            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();

            return queue;
        }
	}
}
