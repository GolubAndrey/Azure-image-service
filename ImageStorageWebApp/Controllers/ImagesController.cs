using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureImageService;
using Microsoft.AspNetCore.Mvc;
using ImageStorageWebApp.ViewModel;
using System.IO;
using NLog;

namespace ImageStorageWebApp.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly Logger _logger;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.Info($"Loading image by id.");
            try
            {
                await _imageService.GetImage(id);
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error($"Image with id {id} not found");
                return BadRequest();
            }
            return Ok();
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ImageRequestBody imageRequestBody)
        {
            _logger.Info($"Upload image by id.");
            try
            {
                await _imageService.LoadImage(imageRequestBody.Url);
            }
            catch(FileNotFoundException ex)
            {
                _logger.Error($"File with url {imageRequestBody.Url} not found");
                return BadRequest();
            }
            return Ok();
        }
    }
}