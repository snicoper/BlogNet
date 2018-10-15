using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Services.ImageServices
{
    public interface IUploadImageService
    {
        Task<UploadImageResult> UploadAsync(IFormFile imageFile, string uploadTo);
        Task<UploadImageResult> UploadAndResizeAsync(IFormFile imageFile, string uploadTo, int maxWidth, int maxHeight);
    }
}
