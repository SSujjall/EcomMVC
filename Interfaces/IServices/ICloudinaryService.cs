using EcomSiteMVC.Models.Enums;

namespace EcomSiteMVC.Interfaces.IServices
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, FolderName folderName);
        public Task DeleteImageAsync(string publicId);
    }
}