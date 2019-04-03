using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureImageService
{
    public interface IImageService
    {
        Task LoadImage(string url);

        Task GetImage(int id);
    }
}
