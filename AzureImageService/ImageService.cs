using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureStorage;

namespace AzureImageService
{
    public class ImageService:IImageService
    {
        private readonly IImageRepository _imageRepository;

        private static int _id = 1;
        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task LoadImage(string url)
        {
            var contentType = "image/jpeg";

            using (var fileStream = new FileStream(url, FileMode.Open))
            {
                await _imageRepository.LoadImageAsync(_id.ToString(), fileStream, contentType);
                await _imageRepository.EnqueueMessage(_id.ToString());
            }

            _id++;
        }

        public async Task GetImage(int id)
        {
            using (var fileStream = new FileStream($"{id}.jpg", FileMode.OpenOrCreate))
            {
                try
                {
                    await _imageRepository.GetImageAsync(id.ToString(), fileStream);
                }
                catch(FileNotFoundException ex)
                {
                    File.Delete($"{id}.jpg");
                    throw;
                }
            }
        }
    }
}
