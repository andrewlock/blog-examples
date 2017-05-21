using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageSharp;
using ImageSharp.Formats;
using ImporvedImageSharpUsage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace ImporvedImageSharpUsage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProvider _fileProvider;
        private readonly IJpegEncoderOptions _encoderOptions;
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;

        public HomeController(IFileProvider fileProvider, IJpegEncoderOptions encoderOptions, IDistributedCache cache, ILogger<HomeController> logger)
        {
            _logger = logger;
            _encoderOptions = encoderOptions;
            _fileProvider = fileProvider;
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/image/{*url}")]
        public async Task<IActionResult> ResizeImage(string url, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int width, int height)
        {
            if (sourceWidth <= 0 || sourceHeight <= 0) { return BadRequest(); }
            if (width < 0 || height < 0) { return BadRequest(); }
            if (width == 0 && height == 0) { return BadRequest(); }
            if (height == 0)
            {
                width = SanitizeSize(width);
            }
            else
            {
                width = 0;
                height = SanitizeSize(height);
            }

            var imagePath = PathString.FromUriComponent("/" + url);
            (var isValid, var subPath) = ValidatePath(imagePath);
            if (!isValid) { return NotFound(); }

            var fileInfo = _fileProvider.GetFileInfo(subPath.Value);
            if (!fileInfo.Exists) { return NotFound(); }
            var key = $"{url} {sourceX}x{sourceY}x{sourceWidth}x{sourceHeight} {width}x{height}";
            var data = await _cache.GetAsync(key);
            if (data == null)
            {
                _logger.LogDebug("Cache miss - key: {key}", key);
                using (var outputStream = new MemoryStream())
                {
                    using (var inputStream = fileInfo.CreateReadStream())
                    using (var image = Image.Load(inputStream))
                    {
                        image
                            .Crop(new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight))
                            .Resize(width, height)
                            .SaveAsJpeg(outputStream, _encoderOptions);
                    }

                    data = outputStream.ToArray();
                }
                
                await _cache.SetAsync(key, data);
            }
            else
            {
                _logger.LogDebug("Cache hit - key: {key}", key);
            }

            return File(data, "image/jpg");
        }

        public IActionResult Error()
        {
            return View();
        }

        // Check if the URL matches any expected paths
        (bool isValid, PathString subPath) ValidatePath(PathString path)
        {
            var isValid = Helpers.TryMatchPath(path, PathString.Empty, forDirectory: false, subpath: out PathString subPath);
            return (isValid, subPath);
        }

        private static int[] SupportedSizes = { 480, 960, 1280 };

        private static int SanitizeSize(int value)
        {
            if (value >= 1280) { return 1280; }
            return SupportedSizes.First(size => size >= value);
        }
    }
}
