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

namespace ImporvedImageSharpUsage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProvider _fileProvider;
        private readonly IJpegEncoderOptions _encoderOptions;

        public HomeController(IHostingEnvironment env, IJpegEncoderOptions encoderOptions)
        {
            _encoderOptions = encoderOptions;
            _fileProvider = env.WebRootFileProvider;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/image/{width}/{height}/{*url}")]
        public IActionResult ResizeImage(string url, int width, int height)
        {
            if (width < 0 || height < 0 ) { return BadRequest(); }

            var imagePath = PathString.FromUriComponent("/" + url);
            var fileInfo = _fileProvider.GetFileInfo(imagePath);
            if (!fileInfo.Exists) { return NotFound(); }

            var outputStream = new MemoryStream();
            using (var inputStream = fileInfo.CreateReadStream())
            using (var image = Image.Load(inputStream))
            {
                image
                    .Resize(width, height)
                    .SaveAsJpeg(outputStream, _encoderOptions);
            }

            outputStream.Seek(0, SeekOrigin.Begin);

            return File(outputStream, "image/jpg");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
