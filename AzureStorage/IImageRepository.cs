using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AzureStorage
{
    public interface IImageRepository
    {
        Task LoadImageAsync(string blobName, Stream stream, string contenType);

        Task GetImageAsync(string blobName, Stream stream);

        Task EnqueueMessage(string stringMessage);
    }
}
