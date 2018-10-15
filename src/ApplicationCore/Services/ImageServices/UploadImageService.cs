using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ApplicationCore.Services.ImageServices
{
    public class UploadImageService : IUploadImageService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public UploadImageService(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        /// <summary>
        /// Sube una imagen.
        /// </summary>
        /// <param name="imageFile">Imagen del formulario</param>
        /// <param name="uploadTo">Path relativo a IConfiguration["Images:Path"]</param>
        /// <returns>UploadReturnResult, si es success ReturnMessages contiene el fullPath y filename</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public async Task<UploadImageResult> UploadAsync(IFormFile imageFile, string uploadTo)
        {
            var returnResult = new UploadImageResult { Errors = new List<string>() };
            var imagesPath = _configuration["Images:Path"].Trim('/').Replace('/', Path.DirectorySeparatorChar);
            var allowedExtensions = _configuration["Images:Extensions"];

            if (imagesPath is null || allowedExtensions is null)
            {
                throw new ArgumentNullException();
            }

            var extensionParts = allowedExtensions.ToLower().Split(',');
            var timeTicks = DateTime.Now.Ticks.ToString();
            var imageName = $"{timeTicks}-{imageFile.FileName}";
            var contentType = imageFile.ContentType;

            if (string.IsNullOrEmpty(contentType) || !extensionParts.Contains(contentType.ToLower()))
            {
                returnResult.Success = false;
                returnResult.Errors.Add("Tipo de archivo no valido");
            }
            else
            {
                var uploadToFullPath = Path.Combine(_env.WebRootPath, imagesPath, uploadTo);
                if (!Directory.Exists(uploadToFullPath))
                {
                    if (!Directory.CreateDirectory(uploadToFullPath).Exists)
                    {
                        var message = $"El directorio {uploadToFullPath} no existe y no se ha podido crear";
                        throw new DirectoryNotFoundException(message);
                    }
                }

                var imageFullPath = Path.Combine(uploadToFullPath, imageName);
                using (var fileStream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                returnResult.ReturnMessages = new Dictionary<string, string>
                {
                    ["fullPath"] = imageFullPath,
                    ["filename"] = imageName
                };
                returnResult.Success = true;
            }

            return returnResult;
        }

        /// <summary>
        /// Sube una imagen y la redimensiona
        /// </summary>
        /// <param name="imageFile">Imagen del formulario</param>
        /// <param name="uploadTo">Path relativo a IConfiguration["Images:Path"]</param>
        /// <param name="maxWidth">Ancho max</param>
        /// <param name="maxHeight">Alto max</param>
        /// <returns>UploadReturnResult, si es success ReturnMessages contiene el fullPath y filename</returns>
        public async Task<UploadImageResult> UploadAndResizeAsync(
            IFormFile imageFile,
            string uploadTo,
            int maxWidth,
            int maxHeight)
        {
            var result = await UploadAsync(imageFile, uploadTo);
            if (result.Success)
            {
                var fullPath = result.ReturnMessages["fullPath"];
                var resizeImage = new ResizeImageService();
                resizeImage.Resize(fullPath, fullPath, maxWidth, maxHeight);
            }

            return result;
        }
    }
}
