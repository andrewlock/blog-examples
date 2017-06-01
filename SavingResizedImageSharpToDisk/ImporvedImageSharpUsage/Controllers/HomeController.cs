using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageSharp;
using ImageSharp.Formats;
using ImageSharp.Processing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace ImporvedImageSharpUsage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProvider _fileProvider;
        private readonly IJpegEncoderOptions _encoderOptions;
        private readonly ILogger _logger;

        public HomeController(IHostingEnvironment env, IJpegEncoderOptions encoderOptions, ILogger<HomeController> logger)
        {
            _encoderOptions = encoderOptions;
            _fileProvider = env.WebRootFileProvider;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/resized/{width}/{height}/{*url}")]
        public IActionResult ResizeImage(string url, int width, int height)
        {
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

            var originalPath = PathString.FromUriComponent("/" + url);
            var fileInfo = _fileProvider.GetFileInfo(originalPath);
            if (!fileInfo.Exists) { return NotFound(); }

            var tempPath = $"/resized/{width}/{height}/{url}";
            var resizedPath = Path.Combine(Path.GetDirectoryName(tempPath), Path.GetFileNameWithoutExtension(tempPath)) + ".jpg";
            var resizedInfo = _fileProvider.GetFileInfo(resizedPath);
            if (resizedInfo.Exists)
            {
                _logger.LogDebug("Previously resized image found");
            }
            else
            {
                _logger.LogDebug("Image not found - resizing");
                Directory.CreateDirectory(Path.GetDirectoryName(resizedInfo.PhysicalPath));
                using (var outputStream = new FileStream(resizedInfo.PhysicalPath, FileMode.CreateNew))
                using (var inputStream = fileInfo.CreateReadStream())
                using (var image = Image.Load(inputStream))
                {
                    image
                        .Resize(width, height)
                        .SaveAsJpeg(outputStream, _encoderOptions);
                }
            }

            return PhysicalFile(resizedInfo.PhysicalPath, "image/jpg");
        }

        public IActionResult Error()
        {
            return View();
        }

        private static int[] SupportedSizes = { 100, 480, 960, 1280 };
        private int SanitizeSize(int value)
        {
            if (value >= 1280) { return 1280; }
            return SupportedSizes.First(size => size >= value);
        }
    }
}
