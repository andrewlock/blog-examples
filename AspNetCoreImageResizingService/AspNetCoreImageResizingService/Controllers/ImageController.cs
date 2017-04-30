// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ImageSharp;
using ImageSharp.Formats;
using ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreImageResizingService
{
  public class ImageController :  Controller
  {
    public async Task<IActionResult> Index(string url, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int destinationWidth, int destinationHeight)
    {
      Image sourceImage = await this.LoadImageFromUrl(url);

      if (sourceImage != null)
      {
        try
        {

          Image<Rgba32> destinationImage = this.CropImage(sourceImage, sourceX, sourceY, sourceWidth, sourceHeight, destinationWidth, destinationHeight);
          Stream outputStream = new MemoryStream();

          destinationImage.Save(outputStream, new JpegEncoder());
          outputStream.Seek(0, SeekOrigin.Begin);
          return this.File(outputStream, "image/jpg");
        }

        catch
        {
          // Add error logging here
        }
      }

      return this.NotFound();
    }

    private async Task<Image> LoadImageFromUrl(string url)
    {
      Image image = null;

      try
      {
        //Note: don't new up HttpClient in practice
        //This should be stored in a shared field in practice
        HttpClient httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(url);
        Stream inputStream = await response.Content.ReadAsStreamAsync();

        image = Image.Load(inputStream);
      }

      catch
      {
        // Add error logging here
      }

      return image;
    }

    private Image<Rgba32> CropImage(Image sourceImage, int sourceX, int sourceY, int sourceWidth, int sourceHeight, int destinationWidth, int destinationHeight)
    {
      return sourceImage
        .Crop(new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight))
        .Resize(destinationWidth, destinationHeight);
    }
  }
}