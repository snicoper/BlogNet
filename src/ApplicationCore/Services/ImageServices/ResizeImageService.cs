using ImageMagick;
using System.IO;

namespace ApplicationCore.Services.ImageServices
{
    public class ResizeImageService
    {
        /// <summary>
        /// Redimensiona una imagen.
        /// Si la imagen es menor a width y height, la imagen no se redimensionara.
        /// </summary>
        /// <param name="imagePath">Full path imagen a redimensionar</param>
        /// <param name="outputPath">Full path destino</param>
        /// <param name="width">Ancho máximo</param>
        /// <param name="height">Alto máximo</param>
        /// <exception cref="FileNotFoundException"></exception>
        public void Resize(string imagePath, string outputPath, int width, int height)
        {
            if (File.Exists(imagePath) is false)
            {
                var error = $"La imagen que intenta cargar no existe en {imagePath}";
                throw new FileNotFoundException(error);
            }

            using (var image = new MagickImage(imagePath))
            {
                if (image.Width > width || image.Height > height)
                {
                    int resizeWidth;
                    int resizeHeight;

                    if (image.Width > image.Height)
                    {
                        resizeWidth = width;
                        resizeHeight = (int)(image.Height * ((decimal)width / image.Width));
                    }
                    else
                    {
                        resizeHeight = height;
                        resizeWidth = (int)(image.Height * ((decimal)height / image.Height));
                    }

                    image.Resize(resizeWidth, resizeHeight);
                    image.Write(outputPath);
                }
            }
        }
    }
}
