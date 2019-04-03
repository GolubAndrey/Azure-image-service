using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using AzureStorage;
using System.Drawing;
using StackBlur.Extensions;

namespace WebJob
{
    public class Functions
    {

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("images")] string imageId,
            [Blob("images/{queueTrigger}", FileAccess.Read)] Stream inputStream,
            [Blob("images/{queueTrigger}", FileAccess.Write)] Stream outputStream)
        {
            using (var bitmap = new Bitmap(inputStream))
            {
                int radius = 60;

                bitmap.StackBlur(radius);

                bitmap.Save(outputStream, bitmap.RawFormat);
            }
        }
    }
}
